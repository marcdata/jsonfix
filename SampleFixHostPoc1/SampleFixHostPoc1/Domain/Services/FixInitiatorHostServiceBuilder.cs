using QuickFix;
using SampleFixHostPoc1.Domain.FixApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFixHostPoc1.Domain.Services
{
    // builder from config file

    public class FixInitiatorHostServiceBuilder
    {
        private string _pathToCfgFile;

        public FixInitiatorHostServiceBuilder(string pathToQuickFixConfigFile)
        {
            _pathToCfgFile = pathToQuickFixConfigFile ?? throw new ArgumentNullException();
        }

        public FixInitiatorHostService GetFixInitiatorHostService()
        {
            // build up whatever from the config file 
            // return the actual FixInitiatorHostService

            var initiator = this.BuildInitiatorFromConfigFile();
            var initatorControl = new InitiatorControl(initiator);

            var fixInitiatorHostService = new FixInitiatorHostService();
            fixInitiatorHostService.Add(initatorControl);

            return fixInitiatorHostService;
        }

        public FixInitiatorHostService GetDefaultFixInitiatorHostService()
        {
            // build up whatever from the config file 
            // return the actual FixInitiatorHostService

            var fixInitiatorHostService = new FixInitiatorHostService();
            fixInitiatorHostService.InitializeSelf_Stub();
            return fixInitiatorHostService;

        }

        public QuickFix.IInitiator BuildInitiatorFromConfigFile()
        {
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

                var pathToConfigFile = "cfg/simpleClientForAcceptorThree.cfg";
                var dbgPathToTargetConfigFile = currentDirectory_one + @"/" + pathToConfigFile;
                Console.WriteLine($"Looking here for QuickFix cfg file: {dbgPathToTargetConfigFile}");

                SessionSettings settings = new SessionSettings(pathToConfigFile);
                IApplication app = new SimpleFixAppConsoleLogger();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);

                // dbg filepath to fix spec files
                Console.WriteLine("Spilling all session ids");
                var sessionsFromSettings = settings.GetSessions();
                foreach (var sess in sessionsFromSettings)
                {
                    Console.WriteLine($"(dbg) session: {sess.ToString()}");

                }
                // var sessionId = "FIX.4.2:SIMPLE->CLIENT1";

                var sessionSettingsStringified = settings.ToString();
                Console.WriteLine($"Session settings spill: {sessionSettingsStringified}");

                // problems... 
                // if filepath errors from settings to data dictionary file, this will fault.
                // IInitiator initiator = new ini(app, storeFactory, settings, logFactory);

                QuickFix.Transport.SocketInitiator initiator = new QuickFix.Transport.SocketInitiator(app, storeFactory, settings, logFactory);

                // possibly look at lower-level ways to construct gthe socker acceptor (or socker initiator) relying less on files, more programmatic.

                return initiator;

                // going to separate out initiation control to diff part...
                //acceptor.Start();
                //Console.WriteLine("press <enter> to quit");
                //Console.Read();
                //acceptor.Stop();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());

                return null;
            }

        }

    }
}
