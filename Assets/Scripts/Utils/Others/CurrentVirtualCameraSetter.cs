using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Linq;

namespace GGJ_2026
{
    public class CurrentVirtualCameraSetter : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private CinemachineBrain cinemachineBrain;

        private readonly List<CinemachineCameraHandler> cinemachineCameraHandlersList = new();

        public void SubscribeToEnableEvents()
        {
            CinemachineCameraHandler.onSetup += OnCinemachineCameraHandlerSetup;
            MyMonoBehaviour.onAboutToBeDestroyed += OnMyMonoBehaviourAboutToBeDestroyed;
        }

        public void UnsubscribeFromEnableEvents()
        {
            CinemachineCameraHandler.onSetup -= OnCinemachineCameraHandlerSetup;
            MyMonoBehaviour.onAboutToBeDestroyed -= OnMyMonoBehaviourAboutToBeDestroyed;
        }

        private void OnCinemachineCameraHandlerSetup(CinemachineCameraHandler cinemachineCameraHandler)
        {
            cinemachineCameraHandlersList.AddIfNotInList(cinemachineCameraHandler);
        }

        private void OnMyMonoBehaviourAboutToBeDestroyed(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour is CinemachineCameraHandler cinemachineCameraHandler)
            {
                cinemachineCameraHandlersList.RemoveIfIsInList(cinemachineCameraHandler);
            }
        }

        public void SetCurrentCinemachineCameraHandler(CinemachineCameraHandler cinemachineCameraHandler, float transitionSeconds = 1f)
        {
            ToggleAllCinemachineCameraHandlers(false);
            cinemachineBrain.SetDefaultBlendSeconds(transitionSeconds);
            cinemachineCameraHandler.Toggle(true);
        }

        private void ToggleAllCinemachineCameraHandlers(bool on)
        {
            cinemachineCameraHandlersList.ForEach(iCinemachineCameraHandler => iCinemachineCameraHandler.Toggle(on));
        }

        public CinemachineCameraHandler GetCurrentCinemachineCameraHandler()
        {
            return cinemachineCameraHandlersList.FirstOrDefault(iCinemachineCameraHandler => iCinemachineCameraHandler.IsOn());
        }
    }
}