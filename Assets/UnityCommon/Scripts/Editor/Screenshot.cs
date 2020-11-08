// using System;
// using System.Collections;
// using System.IO;
// using UnityEditor;
// using UnityEngine;
//
// #if UNITY_EDITOR
//
//
// /*
//  * Add 1242x2688 and 1242x2208 fixed resolutions to game view resolution presets
//  * Shortcut :  Alt+Shift+S 
//  * Saves in {Project Path}/Screenshots
//  *
//  */
//
//
// namespace UnityCommon.Runtime.Rendering
// {
//
//     public static class Screenshot
//     {
//
//
//         [MenuItem("Screenshot/Take #&s")]
//         public static void TakeAndSave()
//         {
//
//             GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(TakeAndSave_());
//         }
//
//         private static IEnumerator TakeAndSave_()
//         {
//
//             GameViewSizeGroupType type;
//
//             if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
//                 type = GameViewSizeGroupType.Android;
//             else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
//                 type = GameViewSizeGroupType.iOS;
//             else
//                 throw new Exception($"Target platform must be Android or iOS, it is {EditorUserBuildSettings.activeBuildTarget}");
//
//
//             var dir = Application.dataPath.Replace("Assets", "Screenshots");
//             if (Directory.Exists(dir) == false)
//             {
//                 Directory.CreateDirectory(dir);
//             }
//
//
//             var _w = Screen.width;
//             var _h = Screen.height;
//
//             int w = 1242;
//             int h = 2688;
//
//             int size = GameViewUtils.FindSize(type, w, h);
//             if (size < 0)
//             {
//                 GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution, type,
//                     w, h, $"SS_{w}x{h}");
//
//                 yield return null;
//
//                 size = GameViewUtils.FindSize(type, w, h);
//             }
//
//             if (size < 0)
//                 throw new Exception("Cannot find size (1)");
//
//             GameViewUtils.SetSize(size);
//
//             yield return null;
//             yield return null;
//
//             var path = Application.dataPath.Replace("Assets", "") + $"Screenshots/{w}x{h}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}" + ".png";
//             ScreenCapture.CaptureScreenshot(path, 1);
//
//             yield return null;
//
//             w = 1242;
//             h = 2208;
//
//             size = GameViewUtils.FindSize(type, w, h);
//
//             if (size < 0)
//             {
//                 GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution, type,
//                     w, h, $"SS_{w}x{h}");
//
//                 yield return null;
//
//                 size = GameViewUtils.FindSize(type, w, h);
//             }
//
//             if (size < 0)
//                 throw new Exception("Cannot find size (1)");
//
//             GameViewUtils.SetSize(size);
//
//             yield return null;
//             yield return null;
//
//
//             path = Application.dataPath.Replace("Assets", "") + $"Screenshots/{w}x{h}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}" + ".png";
//             ScreenCapture.CaptureScreenshot(path, 1);
//
//             /*
//             yield return null;
//
//             w = _w;
//             h = _h;
//             Debug.Log(size);
//             size = GameViewUtils.FindSize(type, w, h);
//             GameViewUtils.SetSize(size);
//             */
//
//         }
//
//
//
//
//     }
//
// }
//
//
// #endif
