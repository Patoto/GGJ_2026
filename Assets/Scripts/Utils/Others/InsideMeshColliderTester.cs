using NaughtyAttributes;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class InsideMeshColliderTester : MyMonoBehaviour
    {
        [SerializeField] private MeshCollider meshCollider;

        [Button]
        private void TestInsideMeshCollider()
        {
            bool positionIsInsideMeshCollider = Utils.PositionIsInsideMeshCollider(transform.position, meshCollider);
            Debug.Log("Position is inside mesh collider: " + positionIsInsideMeshCollider);
        }
    }
}