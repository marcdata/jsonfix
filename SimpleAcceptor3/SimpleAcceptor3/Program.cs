using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix.Transport;

namespace SimpleAcceptor3
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("===== Hello proof of concept world. ========");
            Console.WriteLine("===== Simple acceptor (3)           ========");
            Console.WriteLine("============================================");
            Console.WriteLine("This is only an example program.");
            Console.WriteLine("It's a simple server (e.g. Acceptor) app that will let clients (e.g. Initiators)");
            Console.WriteLine("connect to it.  It will accept and display any application-level messages that it receives.");
            Console.WriteLine("Connecting clients should set TargetCompID to 'SIMPLE' and SenderCompID to 'CLIENT1' or 'CLIENT2'.");
            Console.WriteLine("Port is 5001.");
            Console.WriteLine("(see simpleacc.cfg for configuration details)");
            Console.WriteLine("see also, revised and updated from the boilerplate doc...");
            Console.WriteLine("usage: SimpleAcceptor3 cfg/simpleAcceptorThree.cfg");
            Console.WriteLine("=============");

            if (args.Length != 1)
            {
                Console.WriteLine("usage: SimpleAcceptor3 CONFIG_FILENAME");
                Console.WriteLine("usage: SimpleAcceptor3 cfg/simpleAcceptorThree.cfg");
                System.Environment.Exit(2);
            }

            try
            {
                // debug cwd
                var currentDirectory_one = System.Environment.CurrentDirectory;
                var currentDirectory_BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine($"Current directory from system env: {currentDirectory_one}");
                Console.WriteLine($"Current directory from app domain base dir: {currentDirectory_BaseDirectory}");

                /* 
                 * Current directory from system env: C:\Users\marc\source\repos\SimpleAcceptor3\SimpleAcceptor3
                 * So, quickfix is using the system.env.currentdirectory as its base folder when it navigates 
                 * to the DataDictionary file from the relative file path defined in the .cfg config file.
                 * */

                SessionSettings settings = new SessionSettings(args[0]);
                IApplication app = new SimpleAcceptorThreeApp();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);

                // dbg filepath to fix spec files
                Console.WriteLine("Spilling all session ids");
                var sessionsFromSettings = settings.GetSessions();
                foreach(var sess in sessionsFromSettings)
                {
                    Console.WriteLine($"(dbg) session: {sess.ToString()}");

                }
                var sessionId = "FIX.4.2:SIMPLE->CLIENT1";

                var sessionSettingsStringified = settings.ToString();
                Console.WriteLine($"Session settings spill: {sessionSettingsStringified}");

                // problems... 
                // if filepath errors from settings to data dictionary file, this will fault.
                IAcceptor acceptor = new ThreadedSocketAcceptor(app, storeFactory, settings, logFactory);
                // possibly look at lower-level ways to construct gthe socker acceptor (or socker initiator) relying less on files, more programmatic.


                acceptor.Start();
                Console.WriteLine("press <enter> to quit");
                Console.Read();
                acceptor.Stop();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
