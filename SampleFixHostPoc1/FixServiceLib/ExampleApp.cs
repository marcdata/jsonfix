using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX42;
using static QuickFix.FIX42.Message;

public class MyApplication : MessageCracker, IApplication
{
    public void OnMessage(
        NewOrderSingle ord,
        SessionID sessionID)
    {
        ProcessOrder(ord.Price, ord.OrderQty, ord.Account);
    }

    protected void ProcessOrder(
        Price price,
        OrderQty quantity,
        Account account)
    {
        //...
    }

    #region Application Methods

    public void FromApp(QuickFix.Message msg, SessionID sessionID)
    {
        Crack(msg, sessionID);
    }
    public void OnCreate(SessionID sessionID) { }
    public void OnLogout(SessionID sessionID) { }
    public void OnLogon(SessionID sessionID) { }
    public void FromAdmin(QuickFix.Message msg, SessionID sessionID)
    { }
    public void ToAdmin(QuickFix.Message msg, SessionID sessionID)
    { }
    public void ToApp(QuickFix.Message msg, SessionID sessionID)
    { }

    #endregion
}