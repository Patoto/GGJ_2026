using System.Collections.Generic;
using UnityEngine;

namespace GGJ_2026
{
    public abstract class ListHolder<T> : MyMonoBehaviour
    {
        [SerializeField] public List<T> list = new();
    }
}