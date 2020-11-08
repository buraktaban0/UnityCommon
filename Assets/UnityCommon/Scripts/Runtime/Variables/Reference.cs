using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Variables
{

	[Serializable]
	public abstract class Reference
	{
		public enum Type : int
		{
			Constant = 0,
			GlobalVariable = 1,
			MemberVariable = 2
		}

		[SerializeField] protected Type m_type = Type.Constant;
		public Type type { get => m_type; set => m_type = value; }

		public abstract object GetValue();

	}


	[Serializable]
	public abstract class Reference<T, T1> : Reference where T1 : Variable<T>
	{

		protected static System.Type GetWrappedType()
		{
			return typeof(T1);
		}

		// For constant
		[SerializeField] protected T constantValue;

		// For global and member
		[SerializeField] protected T1 variable;

		[SerializeField] protected GameObject gameObject;
		[SerializeField] protected MonoBehaviour behaviour;
		[SerializeField] protected string fieldName;


		public T Value
		{
			get
			{
				if (m_type == Type.Constant)
				{
					return constantValue;
				}
				else if (m_type == Type.GlobalVariable)
				{
					if (variable != null)
						return variable.Value;
					else
					{
						Debug.Log($"Value reference of type {typeof(T)} is set to use a variable which is null. Returning constant.");
						return constantValue;
					}
				}
				else // (m_type == Type.MemberVariable)
				{
					if (variable == null)
					{
						CollectMemberVariable();
					}

					return variable.Value;

				}
			}
			set
			{

				if (m_type == Type.Constant)
				{
					constantValue = value;
				}
				else
				{
					variable.Value = value;
				}

			}
		}


		private void CollectMemberVariable()
		{

			var field = behaviour.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			this.variable = (T1)field.GetValue(behaviour);

			if (this.variable == null)
			{
				throw new MemberAccessException("You have to make sure that the target 'Member Variable' is initialized before access time. Consider initializing it in the Awake method and accessing it in and after Start method.");
			}

		}


		public override object GetValue()
		{
			return Value;
		}

		public T1 GetVariable()
		{
			if (variable == null && m_type == Type.MemberVariable)
			{
				CollectMemberVariable();
			}

			return variable;
		}

		public void SetVariable(T1 var)
		{
			this.variable = var;
		}

		/*
		public static implicit operator T(Reference<T, T1> reference)
		{
			return reference.Value;
		}
		*/


	}
}
