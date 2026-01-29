using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class TagHolder : MyMonoBehaviour
    {
        [SerializeField] public new Tag tag;
        [SerializeField] public List<GameObject> referencedGameObjectsList = new();

        public T GetReferencedComponent<T>() where T : Component
        {
            return referencedGameObjectsList.FirstOrDefault(iReferencedGameObject => iReferencedGameObject.HasComponent<T>())?.GetComponent<T>();
        }

        public GameObject GetReferencedGameObject()
        {
            return referencedGameObjectsList.FirstOrDefault();
        }
    }
}