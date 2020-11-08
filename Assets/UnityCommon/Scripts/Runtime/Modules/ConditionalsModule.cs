using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Modules
{
	public class ConditionalsModule : Module<ConditionalsModule>
	{
		internal List<Conditional> conditionals = new List<Conditional>(64);


		public override void OnEnable()
		{
			if (conditionals == null)
				conditionals = new List<Conditional>(64);
		}

		public override void OnDisable()
		{
			conditionals?.Clear();
			conditionals = null;
		}

		public override void Update()
		{
			for (int i = conditionals.Count - 1; i >= 0; i--)
			{
				try
				{
					conditionals[i].Update();
					if (conditionals[i].IsDone)
						conditionals.RemoveAt(i);
				}
				catch (Exception ex)
				{
					if (Application.isEditor == false || conditionals[i].Suppress)
					{
						UnityEngine.Debug.Log($"Exception encountered in conditional, removing chain: {ex.ToString()}");
					}
					else
					{
						UnityEngine.Debug.LogError(
							$"Exception encountered in conditional, removing chain: {ex.ToString()}");
					}

					conditionals.RemoveAt(i);
				}
			}
		}

		public override void LateUpdate()
		{
		}
	}


	[System.Serializable]
	public abstract class Conditional
	{
		protected Func<bool> cond, cancelCond;
		protected Action act;

		private Conditional next;

		private bool suppress = false;
		internal bool Suppress => suppress;

		private bool isDone = false;

		internal bool IsDone
		{
			get => isDone;
			set
			{
				isDone = value;

				if (isDone && next != null)
				{
					next.Start();
					ConditionalsModule.Instance.conditionals.Add(next);
				}
			}
		}

		public abstract void Start();

		public abstract void Update();


		public void SuppressExceptions(bool val = true)
		{
			suppress = val;
		}

		public void Cancel()
		{
			IsDone = true;
		}


		/// <summary>
		/// Execute action depending on the previously set conditions.
		/// </summary>
		/// <param name="act"></param>
		/// <returns></returns>
		public Conditional Do(Action act)
		{
			this.act = act;
			return this;
		}


		public Conditional CancelIf(Func<bool> func)
		{
			cancelCond = func;
			return this;
		}


		public ConditionalContinuous ThenWhile(Func<bool> func)
		{
			var cond = new ConditionalContinuous();
			cond.cond = func;
			this.next = cond;
			return cond;
		}

		public ConditionalOnce ThenIf(Func<bool> func)
		{
			var cond = new ConditionalOnce();
			cond.cond = func;
			this.next = cond;
			return cond;
		}

		public ConditionalWait ThenWait(float seconds)
		{
			var cond = new ConditionalWait(seconds);
			this.next = cond;
			return cond;
		}

		public ConditionalOnce ThenDo(Action action)
		{
			var cond = new ConditionalOnce();
			cond.cond = () => true;
			this.next = cond;
			return cond;
		}


		/// <summary>
		/// Every frame for which the condition holds. Does not cancel if the condition does not hold. Must be canceled manually.
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public static ConditionalContinuous While(Func<bool> func)
		{
			var cond = new ConditionalContinuous();
			cond.cond = func;
			ConditionalsModule.Instance.conditionals.Add(cond);
			return cond;
		}

		/// <summary>
		/// Every frame, for 'seconds' seconds.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static ConditionalContinuous For(float seconds)
		{
			var t1 = Time.time + seconds;
			var cond = Conditional.While(() => Time.time <= t1);
			return cond;
		}

		/// <summary>
		/// Repeats action 'repetitions' times, waiting 'interval' seconds in between.
		/// First invokation is delayed by 'interval'.
		/// For immediate first invocation, see 'RepeatNow'.
		/// </summary>
		/// <param name="interval"></param>
		/// <param name="repetitions"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Conditional Repeat(float interval, int repetitions, Action action)
		{
			var cond = Conditional.Wait(interval).Do(action);
			for (int i = 0; i < repetitions - 1; i++)
			{
				cond = cond.ThenWait(interval).Do(action);
			}

			return cond;
		}

		/// <summary>
		/// Same as 'Repeat' but the first invocation is not delayed, it gets invoked immediately.
		/// </summary>
		/// <param name="interval"></param>
		/// <param name="repetitions"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Conditional RepeatNow(float interval, int repetitions, Action action)
		{
			action.Invoke();
			return Repeat(interval, repetitions, action);
		}

		/// <summary>
		/// Waits for a condition to be true. Cancels self after one invocation. Polls until condition holds or manual cancellation.
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public static ConditionalOnce If(Func<bool> func)
		{
			var cond = new ConditionalOnce();
			cond.cond = func;
			ConditionalsModule.Instance.conditionals.Add(cond);
			return cond;
		}

		/// <summary>
		/// Waits for 'seconds' seconds.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static ConditionalWait Wait(float seconds)
		{
			var cond = new ConditionalWait(seconds);
			ConditionalsModule.Instance.conditionals.Add(cond);
			cond.Start();
			return cond;
		}

		public static ConditionalWaitFrame WaitFrames(int frames)
		{
			var cond = new ConditionalWaitFrame(frames);
			ConditionalsModule.Instance.conditionals.Add(cond);
			cond.Start();
			return cond;
		}
	}


	public class ConditionalOnce : Conditional
	{
		public override void Start()
		{
		}

		public override void Update()
		{
			if (cancelCond != null && cancelCond.Invoke())
			{
				IsDone = true;
				return;
			}

			if (cond.Invoke())
			{
				act?.Invoke();
				IsDone = true;
			}
		}
	}

	public class ConditionalContinuous : Conditional
	{
		public override void Start()
		{
		}

		public override void Update()
		{
			if (cancelCond != null && cancelCond.Invoke())
			{
				IsDone = true;
				return;
			}

			if (cond.Invoke())
			{
				act?.Invoke();
			}
		}
	}


	public class ConditionalWaitFrame : Conditional
	{
		private int frames;


		public ConditionalWaitFrame(int frames)
		{
			this.frames = frames;
		}

		public override void Start()
		{
		}

		public override void Update()
		{
			frames -= 1;

			if (frames <= 0)
			{
				act?.Invoke();
				IsDone = true;
			}
		}
	}

	public class ConditionalWait : Conditional
	{
		private float delay;
		private float timer;
		private float endTime;

		private bool useUnscaledTime = false;

		public ConditionalWait(float delay)
		{
			this.delay = delay;
		}

		public override void Start()
		{
			timer = 0f;
		}

		public ConditionalWait UnscaledTime(bool enabled = true)
		{
			useUnscaledTime = enabled;
			return this;
		}

		public override void Update()
		{
			timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

			if (timer >= delay)
			{
				act?.Invoke();
				IsDone = true;
			}
		}
	}
}
