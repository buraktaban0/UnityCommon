using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityCommon.Threading
{

	public static class BackgroundTaskFactory
	{

		private static List<BackgroundTask> tasks = new List<BackgroundTask>();

		public static BackgroundTask Start(Action<BackgroundTask> job, Action onCompleted = null)
		{
			BackgroundTask task = new BackgroundTask(onCompleted);

			tasks.Add(task);

			Task.Run(() => { job.Invoke(task); onCompleted?.Invoke(); }).ConfigureAwait(false);

			return task;
		}


		public static void Abort(BackgroundTask task)
		{
			task.Aborted = true;

			tasks.Remove(task);

		}


	}

}
