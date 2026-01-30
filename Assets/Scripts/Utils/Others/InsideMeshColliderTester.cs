using NaughtyAttributes;
using UnityEngine;

namespace GGJ_2026
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