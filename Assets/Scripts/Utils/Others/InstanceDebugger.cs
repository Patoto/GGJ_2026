using UnityEngine;

namespace PortalRollerCoaster
{
    public class InstanceDebugger : MyMonoBehaviour
    {
        [SerializeField] private string debugMessage = "DEBUG MESSAGE";
        [SerializeField] private bool usesBreakpoint = true;

        public void Debug()
        {
            UnityEngine.Debug.Log(debugMessage);
            if (usesBreakpoint)
            {
                UnityEngine.Debug.Break();
            }
        }
    }
}