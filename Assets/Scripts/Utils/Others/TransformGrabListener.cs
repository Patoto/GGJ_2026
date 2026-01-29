#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace PortalRollerCoaster
{
	[InitializeOnLoad]
	public static class TransformGrabListener
	{
		private static Transform grabbedTransform;

		public static Action<Transform> onGrabbedTransform;
		public static Action<Transform> onReleasedTransform;

		static TransformGrabListener()
		{
			SceneView.duringSceneGui += OnSceneViewDuringSceneGUI;
		}

		private static void OnSceneViewDuringSceneGUI(SceneView sceneView)
		{
			switch (Event.current.type)
			{
				case EventType.MouseDown:
					if (Selection.activeTransform != null)
					{
						OnGrabbedTransform();
					}
					break;
				case EventType.MouseUp:
					if (grabbedTransform != null)
					{
						OnReleasedTransform();
					}
					break;
			}
		}

		private static void OnGrabbedTransform()
		{
			grabbedTransform = Selection.activeTransform;
			onGrabbedTransform?.Invoke(grabbedTransform);
		}

		private static void OnReleasedTransform()
		{
			onReleasedTransform?.Invoke(grabbedTransform);
			grabbedTransform = null;
		}
	}
}
#endif