using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SingeltonApp
{
    public class StartupApprovedArgs
    {
        public StartupApprovedArgs(string[] commandLineArguments)
        {
            this.CommandLineArguments = commandLineArguments;
        }

        public string[] CommandLineArguments { get; private set; }
    }

    public class StartupDisapprovedArgs
    {
        private readonly IEnumerable<Process> processes;

        public StartupDisapprovedArgs(IEnumerable<Process> processes)
        {
            this.processes = processes;
        }

        public IEnumerable<Process> Processes
        {
            get { return processes; }
        }

        public string CurrentProcessName {
            get { return ProcessUtil.GetCurrentProcessName(); }
        }
    }

    public class ApplicationService : IDisposable
    {
        private readonly string[] m_commandLineArguments;
        private readonly Application m_application;

        private string m_applicationId;        
        private Mutex m_mutex;

        public string ApplicationId
        {
            get { return m_applicationId; }
            set { m_applicationId = value; }
        }

        public ApplicationService(Application application)
            : this(application, ProcessUtil.GetCurrentProcessName(), new string[0])
        { }

        public ApplicationService(Application application, StartupEventArgs startupEventArgs)
            :this(application, ProcessUtil.GetCurrentProcessName(), startupEventArgs)
        { }

        public ApplicationService(Application application, string applicationId, StartupEventArgs startupEventArgs)
            : this(application, applicationId, startupEventArgs != null ? startupEventArgs.Args : new string[0])
        { }

        private ApplicationService(Application application, string applicationId, string[] commandLineArguments)
        {
            m_application = application;
            m_applicationId = applicationId;
            m_commandLineArguments = commandLineArguments;
        }        

        public void TryStart(
            Action<StartupApprovedArgs> startupApprovedAction)
        {
            TryStart(
                startupApprovedAction,
                t => t.Processes.First().Focus());
        }

        public void TryStart(
            Action<StartupApprovedArgs> startupApprovedAction,
            Action<StartupDisapprovedArgs> startupDisapprovedAction)
        {
            bool applicationIsNotRunnig;

            m_mutex = new Mutex(true, m_applicationId, out applicationIsNotRunnig);

            if (applicationIsNotRunnig)
            {
                OnStartupApproved(startupApprovedAction);
            }
            else
            {
                OnStartupDisapproved(startupDisapprovedAction);
                m_application.Shutdown();
            }
        }

        protected virtual void OnStartupDisapproved(Action<StartupDisapprovedArgs> startupDisapprovedAction)
        {
            startupDisapprovedAction.Invoke(new StartupDisapprovedArgs(ProcessUtil.GetProcesses()));
        }

        protected virtual void OnStartupApproved(Action<StartupApprovedArgs> startupApprovedAction)
        {
            startupApprovedAction.Invoke(new StartupApprovedArgs(m_commandLineArguments));
        }

        public void Dispose()
        {
            if (m_mutex == null) return;

            m_mutex.Dispose();
            m_mutex = null;
        }
    }
}