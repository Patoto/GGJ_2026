using System;
using UnityEngine;

namespace PortalRollerCoaster
{
	public class DeveloperMenu : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private CanvasGroup canvasGroup;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

		public void SubscribeToInitializeEvents()
        {
            ToggleDeveloperMenuButton.onClicked += OnToggleDeveloperMenuButtonClicked;
            FinishLevelButton.onPointerClick += OnFinishLevelButtonPointerClick;
        }

		public void UnsubscribeFromInitializeEvents()
        {
            ToggleDeveloperMenuButton.onClicked -= OnToggleDeveloperMenuButtonClicked;
            FinishLevelButton.onPointerClick -= OnFinishLevelButtonPointerClick;
        }

        private void OnToggleDeveloperMenuButtonClicked(ToggleDeveloperMenuButton toggleDeveloperMenuButton)
        {
            Toggle(!IsOn());
        }

        private bool IsOn()
        {
            return gameObject.activeInHierarchy && canvasGroup.interactable;
        }

        private void Toggle(bool on)
        {
            canvasGroup.DoToggleFadeAndInteractableTween(on, seconds: 0.1f);
        }

        private void OnFinishLevelButtonPointerClick(FinishLevelButton finishLevelButton)
        {
            Toggle(false);
        }
    }
}