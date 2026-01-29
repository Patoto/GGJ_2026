using UnityEngine;
using System;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PortalRollerCoaster
{
	[Serializable]
	public class SceneField
	{
		[SerializeField] private Object sceneAsset;

		[SerializeField][HideInInspector] private string scenePath;

		public static implicit operator string(SceneField sceneField)
		{
			return sceneField.scenePath;
		}
	}

	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(SceneField))]
	public class SceneFieldPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty serializedProperty, GUIContent guiContent)
		{
			EditorGUI.BeginProperty(rect, guiContent, serializedProperty);
            SerializedProperty sceneAsset = serializedProperty.FindPropertyRelative("sceneAsset");
            SerializedProperty scenePath = serializedProperty.FindPropertyRelative("scenePath");
			rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), guiContent);
			EditorGUI.BeginChangeCheck();
            Object value = EditorGUI.ObjectField(rect, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
			if (EditorGUI.EndChangeCheck())
			{
				sceneAsset.objectReferenceValue = value;
				if (sceneAsset.objectReferenceValue != null)
				{
					scenePath.stringValue = AssetDatabase.GetAssetPath(value);
				}
			}
			EditorGUI.EndProperty();
		}
	}
	#endif
}