using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class MeshCombiner : MyMonoBehaviour
    {
        [SerializeField] private MeshFilter targetMeshFilter;

        [Button]
        private void CombineMeshes()
        {
            List<MeshFilter> sourceMeshFiltersList = gameObject.GetAllChildrenWithComponent<MeshFilter>(false, true);
            CombineInstance[] combineInstance = CreateCombineInstance(sourceMeshFiltersList);
            Mesh mesh = GetCombinedMesh(combineInstance);
            targetMeshFilter.mesh = mesh;
        }

        private static Mesh GetCombinedMesh(CombineInstance[] combineInstance)
        {
            Mesh mesh = new();
            mesh.CombineMeshes(combineInstance);
            return mesh;
        }

        private CombineInstance[] CreateCombineInstance(List<MeshFilter> sourceMeshFiltersList)
        {
            CombineInstance[] combineInstance = new CombineInstance[sourceMeshFiltersList.Count];
            for (int i = 0; i < sourceMeshFiltersList.Count; i++)
            {
                combineInstance[i].mesh = sourceMeshFiltersList[i].sharedMesh;
                combineInstance[i].transform = sourceMeshFiltersList[i].transform.localToWorldMatrix;
            }
            return combineInstance;
        }
    }
}