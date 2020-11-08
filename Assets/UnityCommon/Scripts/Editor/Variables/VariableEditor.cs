using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityCommon.Variables;
using UnityEditor;
using UnityEngine;


namespace UnityCommon.Variables.Editor
{
	[CustomEditor(typeof(Variable), true)]
	public class VariableEditor : UnityEditor.Editor
	{
		private Variable variable;

		private SerializedObject obj;

		private SerializedProperty prop, bindProp;

		public void OnEnable()
		{
			variable = (Variable) target;
			obj = new SerializedObject(target);
			prop = obj.FindProperty("value");
			bindProp = obj.FindProperty("bindToPlayerPrefs");
		}


		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			GUILayout.Space(10);

			base.OnInspectorGUI();


			if (Application.isPlaying == false)
			{
				if (GetValue(obj.targetObject, "value") != GetValue(obj.targetObject, "editorValue"))
				{
					SetValue(obj.targetObject, "value", GetValue(obj.targetObject, "editorValue"));

					OnEnable();
				}
			}

			GUILayout.Space(15);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(prop, new GUIContent("Value"), true);


			GUILayout.Space(40);

			GUI.enabled = variable.CanBeBoundToPlayerPrefs();

			var bind = bindProp.boolValue;

			bind &= GUI.enabled;

			bindProp.boolValue = GUILayout.Toggle(bind, "Bind to PlayerPrefs");

			if (variable.CanBeBoundToPlayerPrefs() == false)
				GUILayout.Label("This type of variable cannot be bound to PlayerPrefs");

			GUI.enabled = true;


			if (EditorGUI.EndChangeCheck())
			{
				obj.ApplyModifiedProperties();
				variable.InvokeModified();
			}


			if (variable.bindToPlayerPrefs)
			{
				GUILayout.Space(15);

				if (PlayerPrefs.HasKey(variable.PrefsKey))
				{
					var prefsVal = PlayerPrefs.GetString(variable.PrefsKey);

					var ser = variable.Serialize();
					variable.Deserialize(prefsVal);

					OnEnable();

					GUI.enabled = false;
					EditorGUILayout.PropertyField(prop, new GUIContent("Value in PlayerPrefs"));
					GUI.enabled = true;

					variable.Deserialize(ser);

					OnEnable();
				}
				else
				{
					GUILayout.Label("No entry");
				}

				GUILayout.Space(30);

				if (GUILayout.Button("Update in PlayerPrefs"))
				{
					PlayerPrefs.SetString(variable.PrefsKey, variable.Serialize());
				}

				GUILayout.Space(20);

				if (GUILayout.Button("Remove from PlayerPrefs"))
				{
					PlayerPrefs.DeleteKey(variable.PrefsKey);
				}
			}


			if (EditorGUI.EndChangeCheck())
			{
				variable.OnInspectorChanged();
			}


			EditorUtility.SetDirty(this);
			Repaint();
		}


		public object GetValue(object obj, string varName)
		{
			FieldInfo field = obj.GetType().BaseType.GetField(varName, BindingFlags.Instance | BindingFlags.NonPublic);
			var value = field.GetValue(obj);

			return value;
		}


		public void SetValue(object obj, string varName, object val)
		{
			FieldInfo field = obj.GetType().BaseType.GetField(varName, BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(obj, val);
		}
	}
}
