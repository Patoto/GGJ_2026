using System.Collections.Generic;
using UnityEngine;

namespace PortalRollerCoaster
{
    public abstract class ListHolder<T> : MyMonoBehaviour
    {
        [SerializeField] public List<T> list = new();
    }
}