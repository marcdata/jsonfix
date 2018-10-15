using System;
using System.Collections.Generic;
using System.Text;
using FixServiceLib.Contracts;
using QuickFix.FIX42;
using QuickFix;

namespace FixServiceLib.Services
{
    // design decision: specify fix42 and fix44 when appropriate?
    // try and keep generic across all fix versions?

    public interface IJsonFixWrapper
    {
        QuickFix.Message Unwrap(JsonFixMessage jsonFixMessage);
        JsonFixMessage Wrap(QuickFix.Message fixBaseMessage, string pseudoHeader);
    }

    public interface IJsonFixWrapperAndCracker
    {
        QuickFix.Message Unwrap(JsonFixMessage jsonFixMessage);
        JsonFixMessage Wrap(QuickFix.Message fixBaseMessage, string pseudoHeader);

        // try this.
        T UnwrapAsType<T>(JsonFixMessage jsonFixMessage) where T : QuickFix.FIX42.Message;

        // alternate 
        //QuickFix.FIX42.NewOrderSingle UnwrapAsNewOrder(JsonFixMessage);
        //QuickFix.FIX42.ExecutionReport UnwrapAsExecutionReport(JsonFixMessage);
    }

    //// old
    //public interface IJsonFixWrapper
    //{
    //    QuickFix.Message Unwrap(JsonFixMessage jsonFixMessage);
    //    JsonFixMessage Wrap(QuickFix.Message fixBaseMessage, string pseudoHeader);

    //    // try this.
    //    T UnwrapAsType<T>(JsonFixMessage jsonFixMessage) where T : QuickFix.FIX42.Message;

    //    // alternate 
    //    //QuickFix.FIX42.NewOrderSingle UnwrapAsNewOrder(JsonFixMessage);
    //    //QuickFix.FIX42.ExecutionReport UnwrapAsExecutionReport(JsonFixMessage);
    //}

    public class JsonFixWrapper : IJsonFixWrapper
    {
        public QuickFix.Message Unwrap(JsonFixMessage jsonFixMessage)
        {
            var qfMessage = new QuickFix.Message(jsonFixMessage.rawFixPayload, false);
            return qfMessage;
        }

        public JsonFixMessage Wrap(QuickFix.Message fixBaseMessage, string pseudoHeader)
        {
            var jsonFixMessage = new JsonFixMessage
            {
                localPseudoHeader = pseudoHeader,
                rawFixPayload = fixBaseMessage.ToString()

            };
            return jsonFixMessage;
        }

        // public static char FixSohSeparator = char('\u0001');

        public char GetSeparator()
        {
            return '\u0001';
        }

        public T UnwrapAsType<T>(JsonFixMessageReceived jsonFixMessage) where T: QuickFix.FIX42.Message
        {
            // get tag 35 from header of fix message
            // get messageFactory
            // get typedmessage built from factory
            

            T typedMessage;

            var beginString = "FIX.4.2";
            var messageTypeArg = "D";

            var rawFromString = jsonFixMessage.rawFixPayload;

            var messageFactory = new MessageFactory();
            var pathToFix42DataDictionary = @"C:\Users\marc\Downloads\quickfixn-v1.8.0\spec\fix\FIX42.xml";
            QuickFix.DataDictionary.DataDictionary dd = new QuickFix.DataDictionary.DataDictionary(pathToFix42DataDictionary);

            var x = messageFactory.Create(beginString, messageTypeArg);
            x.FromString(rawFromString, false, dd, dd);

            var er = new ExecutionReport();

            typedMessage = x as T;

            if(typedMessage == null)
            {
                Console.WriteLine("oops");
            }
            return typedMessage;

        }

        public T UnwrapAsType<T>(JsonFixMessage jsonFixMessage) where T : QuickFix.FIX42.Message
        {
            throw new NotImplementedException();
        }
    }

    public class MyMessageCracker : QuickFix.MessageCracker
    {

    }

}
