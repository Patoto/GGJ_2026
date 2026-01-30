using System;
using UnityEngine;

namespace GGJ_2026
{
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(SerializableVector3 serializableVector3)
        {
            return new Vector3(serializableVector3.x, serializableVector3.y, serializableVector3.z);
        }

        public static implicit operator SerializableVector3(Vector3 vector3)
        {
            return new SerializableVector3(vector3.x, vector3.y, vector3.z);
        }
    }
}