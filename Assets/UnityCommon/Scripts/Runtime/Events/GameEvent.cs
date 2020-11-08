using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;
using UnityEngine;


namespace UnityCommon.Events
{
	[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
	public class GameEvent : ScriptableObject
	{
#if UNITY_EDITOR

		public List<StackInfo> RaiseHistory { get; private set; } = new List<StackInfo>(16);

		private void OnEnable()
		{
			//RaiseHistory = new List<StackInfo>(16);
		}


		private void OnDisable()
		{
			RaiseHistory = new List<StackInfo>(16);
		}

#endif


		protected List<Action<object>> listeners = new List<Action<object>>();


		public void Invoke()
		{
			Invoke(null);
		}

		public int Invoke(object args)
		{
#if UNITY_EDITOR

			StackInfo info = new StackInfo()
			                 {
				                 raiserName = "Raised  @ " + UnityEngine.Time.realtimeSinceStartup + " s  by " +
				                              ( /*sender ??*/ "unknown")
			                 };

#endif
			int validListenerCount = 0;
			for (int i = listeners.Count - 1; i >= 0; i--)
			{
				if (listeners[i] == null)
				{
					listeners.RemoveAt(i);
					continue;
				}

#if UNITY_EDITOR
				info.listeners.Add("Listener: " + listeners[i].Method.DeclaringType.FullName + "@" +
				                   listeners[i].Method.Name);
#endif

				try
				{
					listeners[i].Invoke(args);
					validListenerCount++;
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception while looping through listeners: {ex.ToString()}");
				}
			}

#if UNITY_EDITOR

			RaiseHistory.Add(info);

#endif

			return validListenerCount;
		}


		public void AddListener(Action<object> listener)
		{
			int index = listeners.IndexOf(listener);
			if (index < 0)
			{
				listeners.Add(listener);
			}
		}

		public void RemoveListener(Action<object> listener)
		{
			int index = listeners.IndexOf(listener);
			if (index >= 0)
			{
				listeners.RemoveAt(index);
			}
		}


		public void ClearListeners()
		{
			if (listeners.Count > 0)
				listeners = new List<Action<object>>(32);
		}
	}


	[System.Serializable]
	public class StackInfo
	{
		public bool foldout = false;

		public string raiserName = "NULL";

		public List<string> listeners = new List<string>();
	}

	public class GameEvent<T>
	{
		private List<Action<T>> listeners = new List<Action<T>>(4);


		public void Invoke(T value) => Invoke(null, value);

		public void Invoke(object sender, T value)
		{
			for (int i = listeners.Count - 1; i >= 0; i--)
			{
				var listener = listeners[i];

				if (listener == null)
				{
					listeners.RemoveAt(i);
					continue;
				}

				try
				{
					listener.Invoke(value);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception while looping through listeners: {ex.ToString()}");
				}
			}
		}


		public void AddListener(Action<T> listener)
		{
			int index = listeners.IndexOf(listener);
			if (index < 0)
			{
				listeners.Add(listener);
			}
		}

		public void RemoveListener(Action<T> listener)
		{
			int index = listeners.IndexOf(listener);
			if (index >= 0)
			{
				listeners.RemoveAt(index);
			}
		}
	}
}
