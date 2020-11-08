using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Events;
using UnityCommon.Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace UnityCommon.Variables
{
	public abstract class Variable : ScriptableObject
	{
		private static Dictionary<string, Variable> variables;
		private static List<Variable> prefsVariables;

		[FormerlySerializedAs("BindToPlayerPrefs")]
		[HideInInspector]
		[SerializeField]
		public bool bindToPlayerPrefs = false;


		[HideInInspector]
		[SerializeField]
		public string PrefsKey;


		public abstract object GetValueAsObject();


		public abstract void SetValueAsObject(object obj);

		public abstract void InvokeModified();

		public abstract void ResetToEditorValue();

		public virtual string Serialize()
		{
			return GetValueAsObject().ToString();
		}

		public virtual void Deserialize(string s)
		{
			var val = Convert.ChangeType(s, GetType());
			SetValueAsObject(val);
		}

		public virtual bool CanBeBoundToPlayerPrefs() => true;


		public virtual void OnInspectorChanged()
		{
		}

		public abstract void RaiseModifiedEvent();

		public static void LoadAll()
		{
			// if (prefsVariables != null)
			// {
			// 	foreach (var var in prefsVariables)
			// 	{
			// 		var.ResetToEditorValue();
			// 	}
			// }

			variables = new Dictionary<string, Variable>(16);
			prefsVariables = new List<Variable>(8);

			var loadedVariables = Resources.LoadAll<Variable>("Variables");

			for (int i = 0; i < loadedVariables.Length; i++)
			{
				var variable = loadedVariables[i];

				variable.PrefsKey = $"Variable_{variable.name}";

				if (variables.ContainsKey(variable.name))
				{
					Debug.Log("Variables already contain name " + variable.name);
					if (variables[variable.name] == null)
					{
						variables[variable.name] = variable;
					}
				}
				else
				{
					variables.Add(variable.name, variable);
				}

				variable.hideFlags = HideFlags.DontUnloadUnusedAsset;
				DontDestroyOnLoad(variable);

				if (variable.bindToPlayerPrefs)
				{
					try
					{
						prefsVariables.Add(variable);

						var key = variable.PrefsKey;
						if (PlayerPrefs.HasKey(key))
						{
							var strVal = PlayerPrefs.GetString(key);
							variable.Deserialize(strVal);
						}
						else
						{
							variable.ResetToEditorValue();
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(ex);
					}
				}
				else
				{
					variable.ResetToEditorValue();
				}

				variable.RaiseModifiedEvent();
			} // For end
		}


		public static void SavePrefsVariables()
		{
			foreach (var v in prefsVariables)
			{
				var key = v.PrefsKey;
				var value = v.Serialize();
				PlayerPrefs.SetString(key, value);
			}

			PlayerPrefs.Save();
		}


		public static T Get<T>(string name, bool allowNull = false) where T : Variable
		{
			if (variables == null || variables.Count < 1)
			{
				LoadAll();
			}

			if (variables.ContainsKey(name) == false)
			{
				if (!allowNull)
				{
					Debug.LogError($"Variable with name {name} is not loaded.");
					//throw new KeyNotFoundException($"Variable with name {name} is not loaded.");
				}

				Debug.LogWarning($"Variable with name {name} is not loaded, returning null");
				return null;
			}

			return (T) variables[name];
		}


		public static T Create<T>() where T : Variable
		{
			return ScriptableObject.CreateInstance<T>();
		}
	}

	public abstract class Variable<T> : Variable
	{
		[SerializeField]
		private T editorValue;

		[SerializeField]
		[HideInInspector]
		protected T value;

		public T Value
		{
			get { return value; }
			set
			{
				// TODO: Temporarily disabled because of garbage allocation, fix this
				if (object.Equals(this.value, value))
				{
					//Debug.Log($"Setting value of variable {name} without any changes. Value: {value}");
					return;
				}

				this.value = value;

				if (bindToPlayerPrefs)
				{
					PlayerPrefs.SetString(PrefsKey, this.Serialize());
				}

				OnModified.Invoke(this, value);
			}
		}


		public GameEvent<T> OnModified { get; private set; }

		public override void RaiseModifiedEvent()
		{
			OnModified.Invoke(this, Value);
		}

		private void OnEnable()
		{
			OnModified = new GameEvent<T>();

			if (!bindToPlayerPrefs)
				value = editorValue;
		}

		/*
		private void OnDisable()
		{
			OnModified = new GameEvent<T>();

			if (!BindToPlayerPrefs)
				value = editorValue;
		}
		*/

		public override object GetValueAsObject()
		{
			return value;
		}

		public override void SetValueAsObject(object obj)
		{
			value = (T) obj;
		}

		public void SetValueWithoutNotify(T value)
		{
			this.value = value;
		}


		public override void InvokeModified()
		{
			editorValue = value;

			OnModified?.Invoke(this, value);
		}


		public override void ResetToEditorValue()
		{
			Value = editorValue;
		}

		public static implicit operator T(Variable<T> v)
		{
			return v.Value;
		}
	}
}
