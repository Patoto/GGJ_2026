using System;
using UnityEngine;

namespace PortalRollerCoaster
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