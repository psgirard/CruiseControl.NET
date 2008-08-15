using System;
using System.IO;
using NMock;
using NMock.Constraints;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;

namespace ThoughtWorks.CruiseControl.UnitTests.Core
{
	public class ProcessExecutorTestFixtureBase : IntegrationFixture
	{
		protected const int SuccessfulExitCode = 0;
		protected const int FailedExitCode = -1;
		protected string DefaultWorkingDirectory = @"c:\source\";
		protected int DefaultTimeout = Timeout.DefaultTimeout.Millis;
		protected string ProcessResultOutput = "output";
		protected DateTime testDate = new DateTime(2005, 06, 06, 08, 45, 00);
		protected string testDateString = "2005-06-06";
		protected string testTimeString = "08:45:00";

		protected IMock mockProcessExecutor;
		protected string defaultExecutable;

		protected void CreateProcessExecutorMock(string executable)
		{
			mockProcessExecutor = new DynamicMock(typeof (ProcessExecutor));
			mockProcessExecutor.Strict = true;
			defaultExecutable = executable;
		}

		protected void Verify()
		{
			mockProcessExecutor.Verify();
		}

		protected void ExpectThatExecuteWillNotBeCalled()
		{
			mockProcessExecutor.ExpectNoCall("Execute", typeof (ProcessInfo));
		}

		protected void ExpectToExecuteArguments(string args)
		{
			ExpectToExecute(NewProcessInfo(args));
		}

		protected void ExpectToExecute(ProcessInfo processInfo)
		{
			mockProcessExecutor.ExpectAndReturn("Execute", SuccessfulProcessResult(), processInfo);
		}

		protected void ExpectToExecuteAndReturn(ProcessResult result)
		{
			mockProcessExecutor.ExpectAndReturn("Execute", result, new IsAnything());
		}

		protected void ExpectToExecuteAndThrow()
		{
			mockProcessExecutor.ExpectAndThrow("Execute", new IOException(), new IsAnything());
		}

		protected void ExpectToExecuteWithMonitor(ProcessInfo processInfo, ProcessMonitor monitor)
		{
			mockProcessExecutor.ExpectAndReturn("Execute", SuccessfulProcessResult(), new object[2] { processInfo, monitor });
		}

		protected void ExpectToExecuteAndThrowWithMonitor()
		{
			mockProcessExecutor.ExpectAndThrow("Execute", new IOException(), new object[2] { new IsAnything(), ProcessMonitor.GetProcessMonitorByProject("test") });
		}

		protected void ExpectToExecuteAndReturnWithMonitor(ProcessResult result, ProcessMonitor monitor)
		{
			mockProcessExecutor.ExpectAndReturn("Execute", result, new object[2] { new IsAnything(), monitor });
		}

		protected void ExpectToExecuteArgumentsWithMonitor(string args)
		{
			ExpectToExecuteWithMonitor(NewProcessInfo(args), ProcessMonitor.GetProcessMonitorByProject("test"));
		}

		protected virtual IIntegrationResult IntegrationResult()
		{
			return IntegrationResult(testDate);
		}

		protected IIntegrationResult IntegrationResult(DateTime start)
		{
			IntegrationResult successful = IntegrationResultMother.CreateSuccessful(start);
			successful.WorkingDirectory = DefaultWorkingDirectory;
			return successful;
		}

		protected ProcessResult SuccessfulProcessResult()
		{
			return ProcessResultFixture.CreateSuccessfulResult(ProcessResultOutput);
		}

		protected ProcessResult FailedProcessResult()
		{
			return new ProcessResult(ProcessResultOutput, null, FailedExitCode, false);
		}

		protected ProcessResult TimedOutProcessResult()
		{
			return ProcessResultFixture.CreateTimedOutResult();
		}

		protected ProcessInfo NewProcessInfo(string args)
		{
			ProcessInfo info = new ProcessInfo(defaultExecutable, args, DefaultWorkingDirectory);
			info.TimeOut = DefaultTimeout;
			return info;
		}

		protected IntegrationResult IntegrationResultForWorkingDirectoryTest()
		{
			return (IntegrationResult) Integration("test", "projectWorkingDirectory", "projectArtifactDirectory");
		}
	}
}
