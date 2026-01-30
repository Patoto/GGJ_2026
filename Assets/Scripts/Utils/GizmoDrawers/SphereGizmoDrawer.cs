#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

namespace GGJ_2026
{
    public class SphereGizmoDrawer : MyMonoBehaviour
    {
        [Header("General")]
        [SerializeField] public bool drawGizmo = true;
        [Header("Sphere")]
        [ShowIf(nameof(drawGizmo))][SerializeField] public Color sphereColor;
        [ShowIf(nameof(drawGizmo))][SerializeField] private float sphereSize;
        [ShowIf(nameof(drawGizmo))][SerializeField] private Vector3 sphereOffset;
        [Header("Text")]
        [ShowIf(nameof(drawGizmo))][SerializeField] public string textString;
        [ShowIf(nameof(drawGizmo))][SerializeField] private Vector3 textOffset;
        [ShowIf(nameof(drawGizmo))][SerializeField] private GUIStyle textGuiStyle;

        private void OnDrawGizmos()
        {
            if (drawGizmo)
            {
                DrawSphere();
                DrawText();
            }
        }

        private void DrawSphere()
        {
            Vector3 spherePosition = transform.position + sphereOffset;
            Handles.color = sphereColor;
            Handles.SphereHandleCap(0, spherePosition, Quaternion.identity, sphereSize, EventType.Repaint);
        }

        private void DrawText()
        {
            Vector3 textPosition = transform.position + textOffset;
            Handles.Label(textPosition, textString, textGuiStyle);
        }
    }
}
#endif