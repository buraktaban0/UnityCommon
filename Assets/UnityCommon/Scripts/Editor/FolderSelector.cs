//
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEditor;
// using UnityEngine;
//
// namespace UnityCommon.Editor
// {
// 	public static class FolderSelector
// 	{
//
// 		[MenuItem("Common/Go to \"Resources\"")]
// 		public static void OpenResourcesFolder()
// 		{
// 			int id = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/Resources").GetInstanceID();
// 			ShowFolderContents(id);
// 		}
//
// 		[MenuItem("Common/Go to \"Assets\"")]
// 		public static void OpenAssetsFolder()
// 		{
// 			int id = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets").GetInstanceID();
// 			ShowFolderContents(id);
// 		}
//
// 		[MenuItem("Common/Go to \"Scripts\"")]
// 		public static void OpenScriptsFolder()
// 		{
// 			int id = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/Scripts").GetInstanceID();
// 			ShowFolderContents(id);
// 		}
//
// 		[MenuItem("Common/Go to \"Scenes\"")]
// 		public static void OpenScenesFolder()
// 		{
// 		}
//
// 		public static void OpenFolder(string path)
// 		{
// 			int id = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path).GetInstanceID();
// 			ShowFolderContents(id);
// 		}
//
// 		/// <summary>
// 		/// Selects a folder in the project window and shows its content.
// 		/// Opens a new project window, if none is open yet.
// 		/// </summary>
// 		/// <param name="folderInstanceID">The instance of the folder asset to open.</param>
// 		private static void ShowFolderContents(int folderInstanceID)
// 		{
// 			// Find the internal ProjectBrowser class in the editor assembly.
// 			Assembly editorAssembly = typeof(UnityEditor.Editor).Assembly;
// 			System.Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");
//
// 			// This is the internal method, which performs the desired action.
// 			// Should only be called if the project window is in two column mode.
// 			MethodInfo showFolderContents = projectBrowserType.GetMethod(
// 				"ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);
//
// 			// Find any open project browser windows.
// 			UnityEngine.Object[] projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);
//
// 			if (projectBrowserInstances.Length > 0)
// 			{
// 				for (int i = 0; i < projectBrowserInstances.Length; i++)
// 					ShowFolderContentsInternal(projectBrowserInstances[i], showFolderContents, folderInstanceID);
// 			}
// 			else
// 			{
// 				EditorWindow projectBrowser = OpenNewProjectBrowser(projectBrowserType);
// 				ShowFolderContentsInternal(projectBrowser, showFolderContents, folderInstanceID);
// 			}
// 		}
//
// 		private static void ShowFolderContentsInternal(Object projectBrowser, MethodInfo showFolderContents, int folderInstanceID)
// 		{
// 			// Sadly, there is no method to check for the view mode.
// 			// We can use the serialized object to find the private property.
// 			SerializedObject serializedObject = new SerializedObject(projectBrowser);
// 			bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;
//
// 			if (!inTwoColumnMode)
// 			{
// 				// If the browser is not in two column mode, we must set it to show the folder contents.
// 				MethodInfo setTwoColumns = projectBrowser.GetType().GetMethod(
// 					"SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
// 				setTwoColumns.Invoke(projectBrowser, null);
// 			}
//
// 			bool revealAndFrameInFolderTree = true;
// 			showFolderContents.Invoke(projectBrowser, new object[] { folderInstanceID, revealAndFrameInFolderTree });
// 		}
//
// 		private static EditorWindow OpenNewProjectBrowser(System.Type projectBrowserType)
// 		{
// 			EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
// 			projectBrowser.Show();
//
// 			// Unity does some special initialization logic, which we must call,
// 			// before we can use the ShowFolderContents method (else we get a NullReferenceException).
// 			MethodInfo init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
// 			init.Invoke(projectBrowser, null);
//
// 			return projectBrowser;
// 		}
//
//
// 	}
// }
