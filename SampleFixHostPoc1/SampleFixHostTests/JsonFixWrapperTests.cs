using System;
using Xunit;
using QuickFix.FIX42;
using FixServiceLib.Services;
using FixServiceLib.Contracts;

using System.Linq;

// rename FixService as FixCommService ?

namespace SampleFixHostTests
{

    public class JsonFixWrapperTests
    {
        JsonFixWrapper _jsonFixWrapper; // class under test

        public JsonFixWrapperTests()
        {
            _jsonFixWrapper = new JsonFixWrapper();
        }

        [Fact]
        public void WrapMessage_Ok()
        {
            // get fixMessage
            // wrap into jsonfix
            // assert on jsonfix order

            var orderQty = 10m;
            var ticker = "AAPL";
            var clientOrderId = "0001";
            var handlingInst = "";
            var transactTime = new DateTime(2018, 10, 13).Date;

            // arrange 
            var fm = new QuickFix.FIX42.NewOrderSingle(
                new QuickFix.Fields.ClOrdID(clientOrderId),
                new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.AUTOMATED_EXECUTION_ORDER_PRIVATE_NO_BROKER_INTERVENTION),
                new QuickFix.Fields.Symbol(ticker),
                new QuickFix.Fields.Side(QuickFix.Fields.Side.BUY),
                new QuickFix.Fields.TransactTime(transactTime),
                new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET)
                );
            fm.SetField(new QuickFix.Fields.OrderQty(orderQty));

            // dbg
            Console.WriteLine($"fix message fm: {fm.ToString()}");

            // act 
            var jsonFixMsg = _jsonFixWrapper.Wrap(fm, "myHeaderInfo");

            // assert
            var rawfix = jsonFixMsg.rawFixPayload;
            Assert.False(string.IsNullOrEmpty(rawfix));
            var separator = _jsonFixWrapper.GetSeparator();
            var byParts = rawfix.Split(separator).ToList(); 
            var symbolPart = byParts.FirstOrDefault(x => x.StartsWith("55"));
            Assert.True(symbolPart.EndsWith(ticker));

            Assert.Equal("myHeaderInfo", jsonFixMsg.localPseudoHeader);
        }

        [Fact]
        public void WrapMessage_Fail()
        {
            // arrange 
            var badFormattedFixMessage = "abcdefg";
            var fmNewOrder = new NewOrderSingle();
            fmNewOrder = null;
            // fmNewOrder.FromStringHeader(badFormattedFixMessage);
            var pseudoHeader = "toCounteryPartyInfo";
            // act + assert 
            Assert.Throws<NullReferenceException>(() => _jsonFixWrapper.Wrap(fmNewOrder, pseudoHeader));

        }

        // deserialize

        [Fact]
        public void UnwrapMessage_Ok()
        {
            // arrange
            var jsonFixMsg = new JsonFixMessage
            {
                localPseudoHeader = "myPseudoHeader",
                rawFixPayload = "8=FIX.4.2\u00019=67\u000135=D\u000111=0001\u000121=1\u000138=10\u000140=1\u000154=1\u000155=AAPL\u000160=20181013-00:00:00.000\u000110=078\u0001",
            };

            // act
            QuickFix.Message fixMsg = _jsonFixWrapper.Unwrap(jsonFixMsg);

            // assert
            var expectedTransactTime = new DateTime(2018, 10, 13).Date;
            var expectedTicker = "AAPL";
            var expectedQty = 10m;

            Assert.Equal(expectedTicker, fixMsg.GetField(55));
            Assert.Equal(expectedTransactTime, fixMsg.GetField(new QuickFix.Fields.TransactTime()).getValue());
            Assert.Equal(expectedQty, fixMsg.GetField(new QuickFix.Fields.OrderQty()).getValue());

        }

        [Fact]
        public void UnwrapMessage_Ok_Sample2()
        {
            // arrange
            var jsonFixMsg = new JsonFixMessage
            {
                localPseudoHeader = "myPseudoHeader",
                // rawFixPayload = "8=FIX.4.2\u00019=67\u000135=D\u000111=0001\u000121=1\u000138=10\u000140=1\u000154=1\u000155=AAPL\u000160=20181013-00:00:00.000\u000110=078\u0001",
                rawFixPayload = "8=FIX.4.2\u00019=00\u000135=D\u000149=Sender\u000156=Target\u000134=0\u000152=99990909-17:17:17.777\u000154=1\u000155=ABC\u000111=BUY000000001\u000138=1000\u000140=2\u000144=1001.000\u000159=3\u0001117=A001\u000146=A001\u000110=000\u0001"
            };

            // act
            QuickFix.Message fixMsg = _jsonFixWrapper.Unwrap(jsonFixMsg);

            // assert
            var expectedTicker = "ABC";

            Assert.Equal(expectedTicker, fixMsg.GetField(55));

        }


        //[Fact]
        //public void UnwrapMessage_AsMsgType_Ok()
        //{
        //    // arrange
        //    var jsonFixMsg = new JsonFixMessage
        //    {
        //        localPseudoHeader = "myPseudoHeader",
        //        // rawFixPayload = "8=FIX.4.2\u00019=67\u000135=D\u000111=0001\u000121=1\u000138=10\u000140=1\u000154=1\u000155=AAPL\u000160=20181013-00:00:00.000\u000110=078\u0001",
        //        rawFixPayload = "8=FIX.4.2\u00019=67\u000135=D\u000111=0001\u000121=1\u000138=10\u000140=1\u000154=1\u000155=AAPL\u000160=20181013-00:00:00.000\u000110=078\u0001"

        //    };

        //    // act
        //    // QuickFix.Message fixMsg = _jsonFixWrapper.Unwrap(jsonFixMsg);
        //    QuickFix.FIX42.NewOrderSingle newOrderD = new QuickFix.FIX42.NewOrderSingle();

        //    // issue how to downcast

        //    // assert
        //    var expectedTransactTime = new DateTime(2018, 10, 13).Date;
        //    var expectedTicker = "AAPL";
        //    var expectedQty = 10m;

        //    //Assert.Equal(expectedTicker, fixMsg.GetField(55));
        //    //Assert.Equal(expectedTransactTime, fixMsg.GetField(new QuickFix.Fields.TransactTime()).getValue());
        //    //Assert.Equal(expectedQty, fixMsg.GetField(new QuickFix.Fields.OrderQty()).getValue());

        //}

        [Fact]
        public void UnwrapMessage_AsMsgType_Ok_Sandbox()
        {
            // arrange
            var jsonFixMsg = new JsonFixMessageReceived
            {
                localPseudoHeader = "myPseudoHeader",
                messageTypeCode35 = "D",
                rawFixPayload = "8=FIX.4.2\u00019=00\u000135=D\u000149=Sender\u000156=Target\u000134=0\u000152=99990909-17:17:17.777\u000154=1\u000155=ABC\u000111=BUY000000001\u000138=1000\u000140=2\u000144=1001.000\u000159=3\u0001117=A001\u000146=A001\u000110=000\u0001"
        };

            // act
            QuickFix.Message fixMsg = _jsonFixWrapper.Unwrap(jsonFixMsg);
            // QuickFix.FIX42.NewOrderSingle newOrderD = new QuickFix.FIX42.NewOrderSingle();

            QuickFix.FIX42.NewOrderSingle newOrderD = _jsonFixWrapper.UnwrapAsType<NewOrderSingle>(jsonFixMsg);


            // issue how to downcast

            // assert
            var expectedTicker = "ABC";
            var expectedTarget = "Target";

            var actualTarget = newOrderD.Header.GetField(new QuickFix.Fields.TargetCompID()).getValue();
            var actualTicker = newOrderD.Header.GetField(new QuickFix.Fields.Symbol()).getValue();

            Assert.Equal(expectedTicker, actualTicker);
            Assert.Equal(expectedTarget, actualTarget);


        }


        [Fact] 
        public void Unwrapmessage_Fail()
        {

        }

        // helper logic tests (discard)

        [Fact]
        public void Base_GetSeparator()
        {
            // arrange 
            var sepExpected = '\u0001';

            // act
            var jsonFixSeparator = _jsonFixWrapper.GetSeparator();

            // assert 
            Assert.Equal(sepExpected, jsonFixSeparator);

        }
    }
}
