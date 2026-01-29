using System;
using Unity.Cinemachine;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class CinemachineCameraHandler : MyMonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;

        public static Action<CinemachineCameraHandler> onSetup;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            onSetup?.Invoke(this);
        }

        public void Toggle(bool on)
        {
            cinemachineCamera.enabled = on;
        }

        public bool IsOn()
        {
            return cinemachineCamera.enabled;
        }
    }
}