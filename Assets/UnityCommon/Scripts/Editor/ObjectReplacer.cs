using System.Runtime.Remoting.Contexts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityCommon.Scripts.Editor
{
	public class ObjectReplacer : EditorWindow
	{
		public static ObjectReplacer instance;

		[MenuItem("Tools/Replace GameObject")]
		private static void ShowWindow()
		{
			var window = GetWindow<ObjectReplacer>();
			window.titleContent = new GUIContent("GameObject Replacer");
			window.Show();

			instance = window;
		}


		[MenuItem("GameObject/Replace...", false, 0)]
		public static void ReplaceExplicit()
		{
			ShowWindow();

			instance.objectToBeReplaced = Selection.activeGameObject;
		}

		[MenuItem("GameObject/Replace...", true)]
		public static bool ReplaceExplicitValidation()
		{
			if (Selection.gameObjects.Length > 1)
				return false;

			var obj = Selection.activeGameObject;
			return obj != null && AssetDatabase.IsMainAsset(obj) == false;
		}

		private GameObject objectToBeReplaced;
		private GameObject replacement;


		private void OnGUI()
		{
			replacement =
				EditorGUILayout.ObjectField("Replacement", replacement, typeof(GameObject), true) as GameObject;

			objectToBeReplaced =
				EditorGUILayout.ObjectField("Object to be replaced", objectToBeReplaced, typeof(GameObject), true) as
					GameObject;

			GUILayout.Space(30);

			if (GUILayout.Button("Replace"))
			{
				bool sure = EditorUtility.DisplayDialog("GameObject Replacer", "Are you sure?", "Yes");
				if (sure)
				{
					var t = objectToBeReplaced.transform;

					var name = objectToBeReplaced.name;
					var layer = objectToBeReplaced.layer;
					var tag = objectToBeReplaced.tag;
					var parent = objectToBeReplaced.transform.parent;
					int sibling = objectToBeReplaced.transform.GetSiblingIndex();

					var newObj = PrefabUtility.InstantiatePrefab(replacement) as GameObject;

					Undo.SetCurrentGroupName("Object Replacement Op");

					Undo.RegisterCreatedObjectUndo(newObj, "Replacement");

					var newT = newObj.transform;
					newT.position = t.position;
					newT.rotation = t.rotation;
					newObj.name = name;
					newObj.layer = layer;
					newObj.tag = tag;
					newObj.transform.parent = parent;
					newT.localScale = t.localScale;

					newT.SetSiblingIndex(sibling);

					Selection.activeGameObject = newObj;

					Undo.DestroyObjectImmediate(objectToBeReplaced);

					this.Close();
				}
			}
		}
	}
}
