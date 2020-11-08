using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommon.Modules
{
	abstract class Lerper
	{
	}

	abstract class Lerper<T> : Lerper
	{
		public abstract T Lerp(T a, T b, float t);
	}


	class FloatLerper : Lerper<float>
	{
		public override float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}
	}

	class DoubleLerper : Lerper<double>
	{
		public override double Lerp(double a, double b, float t)
		{
			return a + (b - a) * t;
		}
	}

	class Vector3Lerper : Lerper<Vector3>
	{
		public override Vector3 Lerp(Vector3 a, Vector3 b, float t)
		{
			return a + (b - a) * t;
		}
	}

	class Vector2Lerper : Lerper<Vector2>
	{
		public override Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			return a + (b - a) * t;
		}
	}

	class QuaternionLerper : Lerper<Quaternion>
	{
		public override Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			return Quaternion.Slerp(a, b, t);
		}
	}

	class ColorLerper : Lerper<Color>
	{
		public override Color Lerp(Color a, Color b, float t)
		{
			return a + (b - a) * t;
		}
	}


	public class AnimationModule : Module<AnimationModule>
	{
		internal static Dictionary<Type, Lerper> Lerpers { get; } = new Dictionary<Type, Lerper>
		                                                            {
			                                                            [typeof(float)] = new FloatLerper(),
			                                                            [typeof(double)] = new DoubleLerper(),
			                                                            [typeof(Vector3)] = new Vector3Lerper(),
			                                                            [typeof(Vector2)] = new Vector2Lerper(),
			                                                            [typeof(Quaternion)] = new QuaternionLerper(),
			                                                            [typeof(Color)] = new ColorLerper(),
		                                                            };


		private List<Animation> animations = new List<Animation>(32);


		public override void OnEnable()
		{
			animations = new List<Animation>(16);
		}

		public override void OnDisable()
		{
			animations?.Clear();
			animations = null;
		}


		public override void Update()
		{
		}

		public override void LateUpdate()
		{
			for (int i = animations.Count - 1; i >= 0; i--)
			{
				if (animations[i].IsDone)
				{
					try
					{
						animations[i].onCompleted?.Invoke();
					}
					catch (Exception ex)
					{
						Debug.LogError(
							$"Exception encountered in 'onCompleted' callback of animation: {ex.ToString()}");
					}
					finally
					{
						animations.RemoveAt(i);
					}

					continue;
				}

				try
				{
					animations[i].Update();
				}
				catch (Exception ex)
				{
					Debug.Log($"Exception encountered in update of animation: {ex.ToString()}");
					animations.RemoveAt(i);
				}
			}
		}


		public static Animation<T> Animate<T>(Action<T> setter)
		{
			var anim = new Animation<T>(setter);
			//Instance.animations.Add(anim);
			return anim;
		}

		public static void AddAnimation(Animation anim)
		{
			Instance.animations.Add(anim);
		}

		public static void RemoveAnimation(Animation anim)
		{
			Instance.animations.Remove(anim);
		}
	}

	public class Interpolator
	{
		private Func<float, float> func;

		public Interpolator(AnimationCurve curve)
		{
			this.func = curve.Evaluate;
		}

		public Interpolator(Func<float, float> func)
		{
			this.func = func;
		}

		public float Interpolate(float t) => func.Invoke(t);

		public static Interpolator FromCurve(AnimationCurve curve)
		{
			return new Interpolator(curve.Evaluate);
		}


		public static Interpolator Linear()
		{
			return new Interpolator(t => t);
		}

		public static Interpolator Accelerate(float f = 1.0f)
		{
			return new Interpolator(t => UnityEngine.Mathf.Pow(t, 2 * f));
		}

		public static Interpolator Decelerate(float f = 1.0f)
		{
			return new Interpolator(t => 1f - UnityEngine.Mathf.Pow(1f - t, 2 * f));
		}

		public static Interpolator SmoothCos()
		{
			return new Interpolator(t => UnityEngine.Mathf.Cos((t + 1f) * UnityEngine.Mathf.PI) / 2 + 0.5f);
		}

		public static Interpolator Smooth()
		{
			return new Interpolator(t => (float) (-2.0 * (double) t * (double) t * (double) t +
			                                      3.0 * (double) t * (double) t));
		}

		public static Interpolator Bounce()
		{
			return new Interpolator(
				t =>
				{
					var b = 1.0435f;
					var c = 0.95f;
					if (t < 0.31489f)
					{
						b = c = 0f;
					}
					else if (t < 0.6599f)
					{
						b = 0.54719f;
						c = 0.7f;
					}
					else if (t < 0.85908f)
					{
						b = 0.8526f;
						c = 0.9f;
					}

					var d = 1.1226f * t - b;
					return 8 * d * d + c;
				}
			);
		}

		public static Interpolator Hesitate()
		{
			return new Interpolator(t => 0.5f * (Mathf.Pow(2.0f * t - 1.0f, 3.0f) + 1f));
		}
	}


	public abstract class Animation
	{
		public bool IsDone { get; protected set; } = false;


		internal Action onCompleted;

		public abstract void Reset();

		internal abstract void Update();
	}


	public class Animation<T> : Animation
	{
		private Action<T> setter;


		private float duration = 1.0f;

		// dynamic requires .NET 4.x, but stripping in IL2CPP (therefore iOS) has problems with lambda expressions so we cannot use it (2019.1.2 - Check this for later versions). Thus, 'Lerper' implementation for each possible type is written.
		//private dynamic from;
		//private dynamic to;

		private T from;
		private T to;


		private Interpolator interpolator = Interpolator.Linear();

		private Lerper<T> lerper;

		private float time = 0.0f;

		private bool pingPong = false;
		private bool loop = false;

		private bool unscaledTime = false;

		private List<float> percentSubs = new List<float>();
		private List<Action> percentActions = new List<Action>();


		public Animation(Action<T> setter)
		{
			this.setter = setter;

			lerper = (Lerper<T>) AnimationModule.Lerpers[typeof(T)];
		}

		public override void Reset()
		{
			IsDone = false;
			time = 0.0f;
		}


		bool u = false;

		public Animation<T> DebugUpdate()
		{
			u = true;
			return this;
		}

		public Animation<T> OnCompleted(Action action)
		{
			onCompleted = action;
			return this;
		}

		public Animation<T> Set(Action<T> setter)
		{
			this.setter = setter;
			return this;
		}

		public Animation<T> UnscaledTime(bool unscaledTimeEnabled = true)
		{
			unscaledTime = unscaledTimeEnabled;
			return this;
		}

		public Animation<T> From(T val)
		{
			from = val;
			return this;
		}


		public Animation<T> To(T val)
		{
			to = val;
			return this;
		}

		public Animation<T> For(float seconds)
		{
			duration = seconds;
			return this;
		}


		public Animation<T> With(Interpolator interpolator)
		{
			this.interpolator = interpolator;
			return this;
		}

		public Animation<T> PingPong(bool pingPong = true)
		{
			this.pingPong = pingPong;
			return this;
		}

		internal override void Update()
		{
			time += unscaledTime ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime;

			var t = time / duration;

			for (int i = percentSubs.Count - 1; i >= 0; i -= 1)
			{
				if (t >= percentSubs[i])
				{
					percentSubs.RemoveAt(i);
					var action = percentActions[i];
					percentActions.RemoveAt(i);
					action?.Invoke();
				}
			}

			if (t >= 1.0f)
			{
				if (pingPong)
				{
					var tmp = from;
					from = to;
					to = tmp;
					t = 0.0f;
					time = 0.0f;
				}
				else if (loop)
				{
					t = 0f;
					time = 0.0f;
				}
				else
				{
					IsDone = true;
					t = 1.0f;
				}
			}

			t = interpolator.Interpolate(t);

			T val = lerper.Lerp(from, to, t);

			setter.Invoke(val);
		}


		public Animation<T> Start()
		{
			setter.Invoke(from);
			AnimationModule.AddAnimation(this);

			return this;
		}


		public void Stop()
		{
			AnimationModule.RemoveAnimation(this);
		}

		internal Animation<T> OnPercentCompleted(float v, Action p)
		{
			v = Mathf.Clamp01(v);
			percentSubs.Add(v);
			percentActions.Add(p);

			return this;
		}

		public Animation<T> Loop(bool loop = true)
		{
			this.loop = loop;
			return this;
		}
	}


	/*
	public static class AnimationExtensions
	{


		public static Animation<Vector3> AnimatePosition(this Transform t)
		{
			return AnimationModule.Animate<Vector3>(v => t.position = v).From(t.position).To(Vector3.one * 4f);
		}

	}*/
}
