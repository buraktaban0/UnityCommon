using System;


namespace UnityCommon.Runtime.Utility
{
	public class TimedAction
	{
		public Action action;
		public float countdown;
		public float period;

		private float initialDelay;

		public TimedAction(Action action, float countdown, float period)
		{
			this.action = action;
			this.initialDelay = countdown;
			this.countdown = countdown;
			this.period = period;
		}


		public void Update(float dt)
		{
			countdown -= dt;

			while (countdown < 0f)
			{
				countdown += period;
				action.Invoke();
			}
		}


		public void Execute()
		{
			action.Invoke();
		}


		public void ResetToPeriod()
		{
			countdown = period;
		}

		public void ResetToInitialDelay()
		{
			countdown = initialDelay;
		}
	}
}
