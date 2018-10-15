using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;
using QuickFix.Fields;

namespace SampleFixHostPoc1.Domain.FixApps
{
    /// <summary>
    /// Just a simple server that will let you connect to it and ignore any
    /// application-level messages you send to it.
    /// Note that this app is *NOT* a message cracker.
    /// </summary>

    public class SimpleFixAppConsoleLogger : /*QuickFix.MessageCracker,*/ QuickFix.IApplication
    {
        #region QuickFix.Application Methods

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("App IN:  " + message);
            //Crack(message, sessionID);
        }

        public void ToApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("App OUT: " + message);
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
            Console.WriteLine("Admin IN:  " + message);
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
            Console.WriteLine("Admin OUT:  " + message);
        }
        public void OnCreate(SessionID sessionID)
        {
            Console.WriteLine($"Session created. {sessionID.ToString()}");
        }
        public void OnLogout(SessionID sessionID)
        {
            Console.WriteLine("Logged out: " + sessionID.ToString());
        }
        public void OnLogon(SessionID sessionID)
        {
            Console.WriteLine("Logged on:  " + sessionID.ToString());
        }
        #endregion
    }
}