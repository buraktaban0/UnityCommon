using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityCommon.Variables.Editor
{

	[CustomPropertyDrawer(typeof(Reference), true)]
	public class ReferenceDrawer : PropertyDrawer
	{

		private Tuple<Type, string>[] fields;

		private string[] fieldNames;
		private string[] fieldTypeNames;

		public static Type FindType(string typeName)
		{
			Type t = Type.GetType(typeName);

			if (t != null)
			{
				return t;
			}
			else
			{
				foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					t = asm.GetType(typeName);
					if (t != null)
						return t;
				}
				return null;
			}
		}


		private void CollectFields(SerializedProperty prop, GameObject go)
		{

			if (go == null)
				return;

			var targetTypeName = "UnityCommon.Variables." + prop.type;

			var targetType = FindType(targetTypeName);

			var method = targetType.GetMethod("GetWrappedType", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

			var wrappedType = (System.Type)method.Invoke(null, null);


			var types = go.GetComponents<MonoBehaviour>().Select(beh => beh.GetType());


			fields = types.SelectMany(type => type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
				.Where(field => field.FieldType == wrappedType)
				.Select(f => Tuple.Create(f.DeclaringType, f.Name)).ToArray();

			fieldNames = fields.Select(f => f.Item2).ToArray();
			fieldTypeNames = fields.Select(f => f.Item1.Name + "." + f.Item2).ToArray();

		}





		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var type = (Reference.Type)property.FindPropertyRelative("m_type").enumValueIndex;

			if (type == Reference.Type.MemberVariable)
				return 42;

			return 20;

		}


		private void SelectField(SerializedProperty prop, int i)
		{
			var go = (GameObject)prop.FindPropertyRelative("gameObject").objectReferenceValue;

			var tup = fields[i];

			prop.FindPropertyRelative("behaviour").objectReferenceValue = go.GetComponent(tup.Item1);
			prop.FindPropertyRelative("fieldName").stringValue = tup.Item2;


		}


		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var ddRect = new Rect(position.x, position.y, 16, position.height);


			var enumIndex = property.FindPropertyRelative("m_type").enumValueIndex;
			Reference.Type type = (Reference.Type)EditorGUI.EnumPopup(ddRect, (Reference.Type)enumIndex);
			//index = EditorGUI.Popup(ddRect, index, options);

			property.FindPropertyRelative("m_type").enumValueIndex = (int)type;

			var labelWidth = EditorGUIUtility.labelWidth;
			if (type == Reference.Type.Constant)
			{
				EditorGUIUtility.labelWidth = 16;
				var valueRect = new Rect(position.x + 20, position.y, position.width - 20, position.height);
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("constantValue"), new GUIContent(" "));
			}
			else if (type == Reference.Type.GlobalVariable)
			{
				var valueRect = new Rect(position.x + 20, position.y, position.width - 20, position.height);
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("variable"), GUIContent.none);
			}
			else // if (type == Reference.Type.MemberVariable)
			{
				var valueRect = new Rect(position.x + 20, position.y, position.width - 20, (position.height - 2) / 2f);

				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("gameObject"), GUIContent.none);

				var go = (GameObject)property.FindPropertyRelative("gameObject").objectReferenceValue;

				if (EditorGUI.EndChangeCheck())
				{
					if (go == null)
					{
						fieldNames = null;
					}

					CollectFields(property, (GameObject)property.FindPropertyRelative("gameObject").objectReferenceValue);

					if (fieldNames != null && fieldNames.Length > 0)
					{
						SelectField(property, 0);
					}

				}

				if (go != null && fieldNames == null)
				{
					CollectFields(property, go);
				}

				valueRect = new Rect(position.x, position.y + position.height / 2f + 2, position.width, (position.height - 2) / 2f);
				if (fieldNames != null && fieldNames.Length > 0)
				{

					int index = fieldNames.ToList().IndexOf(property.FindPropertyRelative("fieldName").stringValue);

					EditorGUI.BeginChangeCheck();
					index = EditorGUI.Popup(valueRect, index, fieldTypeNames);

					if (EditorGUI.EndChangeCheck())
					{
						SelectField(property, index);
					}

				}
				else
				{
					var targetTypeName = "UnityCommon.Variables." + property.type;

					var targetType = FindType(targetTypeName);

					var method = targetType.GetMethod("GetWrappedType", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

					var wrappedType = (System.Type)method.Invoke(null, null);

					EditorGUI.LabelField(valueRect, $"No component has a field of type {wrappedType.Name}");
				}

			}

			EditorGUIUtility.labelWidth = labelWidth;

			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

	}

}
