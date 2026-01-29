using UnityEngine;

namespace PortalRollerCoaster
{
    public class MeshGizmoDrawer : MyMonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private Color color;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Transform meshFilterTransform = meshFilter.transform;
            Gizmos.DrawMesh(meshFilter.sharedMesh, meshFilterTransform.position, meshFilterTransform.rotation, meshFilterTransform.lossyScale);
        }
    }
}