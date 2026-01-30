using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
#endif
namespace Kamgam.InvertedMask
{
    public class Installer
    {
        public const string AssetName = "Inverted Mask";
        public const string Version = "1.3.0";
        public const string Define = "KAMGAM_UGUI_INVERTED_MASK";
        public const string ManualUrl = "https://kamgam.com/unity/InvertedMaskManual.pdf";
        public const string AssetLink = "https://assetstore.unity.com/packages/slug/229717";

        public static string AssetRootPath = "Assets/Kamgam/InvertedMask/";
        public static string ExamplePath = AssetRootPath + "Examples/CodeDemo.unity";

        public static Version GetVersion() => new Version(Version);

#if UNITY_EDITOR
        [MenuItem("Tools/" + AssetName + "/Manual", priority = 101)]
        public static void OpenManual()
        {
            Application.OpenURL(ManualUrl);
        }

        [MenuItem("Tools/" + AssetName + "/Open Example Scene", priority = 103)]
        public static void OpenExample()
        {
            EditorApplication.delayCall += () => 
            {
                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ExamplePath);
                EditorGUIUtility.PingObject(scene);
                EditorSceneManager.OpenScene(ExamplePath);
            };
        }

        [MenuItem("Tools/" + AssetName + "/Please leave a review :-)", priority = 510)]
        public static void LeaveReview()
        {
            Application.OpenURL(AssetLink);
        }

        [MenuItem("Tools/" + AssetName + "/More Asset by KAMGAM", priority = 511)]
        public static void MoreAssets()
        {
            Application.OpenURL("https://kamgam.com/unity?ref=asset");
        }

        [MenuItem("Tools/" + AssetName + "/Version " + Version, priority = 512)]
        public static void LogVersion()
        {
            Debug.Log(AssetName + " v" + Version);
        }
#endif
    }
}