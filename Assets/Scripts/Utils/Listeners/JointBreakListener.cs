using System;
using UnityEngine;

namespace GGJ_2026
{
    [RequireComponent(typeof(Joint))]
    public class JointBreakListener : MyMonoBehaviour
    {
        public static Action<JointBreakListener, Joint, float> onJointBreak;

        private void OnJointBreak(float breakForce)
        {
            onJointBreak?.Invoke(this, GetComponent<Joint>(), breakForce);
        }
    }
}