using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.DataBinding
{
	[System.Serializable]
	public class FloatEvent : UnityEvent<float>
	{
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int>
	{
	}

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>
	{
	}

	[System.Serializable]
	public class StringEvent : UnityEvent<string>
	{
	}

	[System.Serializable]
	public class SpriteEvent : UnityEvent<Sprite>
	{
	}


	[DefaultExecutionOrder(-5)]
	public class DataObserver<TRef, TVar, TEvent, T> : MonoBehaviour where TRef : Reference<T, TVar>, new()
	                                                                 where TVar : Variable<T>
	                                                                 where TEvent : UnityEvent<T>
	{
		[SerializeField]
		protected bool onStart = true;

		[SerializeField]
		protected TRef data;

		[SerializeField]
		protected TEvent onModified;


		private bool isInitialized = false;


		private void Reset()
		{
			data = new TRef();
			data.type = Reference.Type.GlobalVariable;
		}

		protected virtual void OnValidate()
		{
			if (data.type == Reference.Type.Constant)
			{
				Debug.Log("Observed data can not be 'Constant', switching to 'Global'");
				data.type = Reference.Type.GlobalVariable;
			}
		}


		protected virtual void Start()
		{
			isInitialized = true;

			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				variable.OnModified.AddListener(OnDataModified);
			}

			if (onStart)
			{
				OnDataModified(data.Value);
			}
		}

		protected virtual void OnEnable()
		{
			if (isInitialized == false)
				return;


			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				if (variable == null || variable.OnModified == null)
					return;

				variable.OnModified.AddListener(OnDataModified);
			}

			OnDataModified(data.Value);
		}

		protected virtual void OnDisable()
		{
			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				if (variable == null || variable.OnModified == null)
					return;

				variable.OnModified.RemoveListener(OnDataModified);
			}
		}


		protected virtual void OnDataModified(T val)
		{
			onModified.Invoke(val);
		}
	}
}
