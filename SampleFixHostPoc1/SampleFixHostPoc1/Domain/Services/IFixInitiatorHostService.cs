using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuickFix.FIX42;
using QuickFix;

namespace SampleFixHostPoc1.Domain.Services
{
    public interface IInitiatorControl
    {
        void Start();
        void Stop();
        string Status();
        string GetSessionShortname();
    }

    // by intention, this is supposed to have one initiator per one session
    // (one initiator in this case isn't meant to control more than one session)

    public class InitiatorControl : IInitiatorControl
    {
        private IInitiator _initiator;

        public InitiatorControl(QuickFix.IInitiator initiator)
        {
            _initiator = initiator;
        }

        public void Start()
        {
            _initiator.Start();
        }
        public void Stop()
        {
            _initiator.Stop();
        }
        public string Status()
        {
            var fixInitiatorStatus = _initiator.IsStopped;
            return fixInitiatorStatus ? "stopped for sure" : "probably going";
        }

        public string GetSessionShortname()
        {
            return _initiator.GetSessionIDs().FirstOrDefault()?.ToString() ?? "session not found";
        }

    }


    public class StubInitiatorControl : IInitiatorControl
    {
        private string _name;
        private bool _isRunning;

        public StubInitiatorControl(string name)
        {
            _name = name.Trim().Substring(0, Math.Min(50, name.Length));
        }

        public void Start()
        {
            _isRunning = true;
        }
        public void Stop()
        {
            _isRunning = false; 
        }
        public string Status()
        {
            var fixInitiatorStatus = _isRunning;
            return fixInitiatorStatus ? "probably going" : "stopped for sure";
        }

        public string GetSessionShortname()
        {
            return _name;
        }

    }

    public interface IFixInitiatorHostService
    {
        void InitializeSelf();
        IEnumerable<IInitiatorControl> GetInitiators();
        void Add(IInitiatorControl initiatorControl);
    }

    public class FixInitiatorHostService : IFixInitiatorHostService
    {
        private List<IInitiatorControl> _initiators;

        public FixInitiatorHostService()
        {
            _initiators = new List<IInitiatorControl>();
        }

        public void Add(IInitiatorControl initiatorControl)
        {
            _initiators.Add(initiatorControl);
        }

        public IEnumerable<IInitiatorControl> GetInitiators()
        {
            return _initiators.ToList().AsReadOnly();
        }

        public void InitializeSelf()
        {
            // throw new NotImplementedException();

            this.InitializeSelf_Stub();
        }

        public void InitializeSelf_Stub()
        {
            _initiators = new List<IInitiatorControl>();
            _initiators.Add(new StubInitiatorControl("niftyFixSession"));
        }
    }

}
