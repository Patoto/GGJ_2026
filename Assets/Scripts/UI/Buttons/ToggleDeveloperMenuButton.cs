using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PortalRollerCoaster
{
    public class ToggleDeveloperMenuButton : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Button button;

        public static Action<ToggleDeveloperMenuButton> onClicked;

        public void SubscribeToEnableEvents()
        {
            Button.onPointerClick += OnButtonPointerClick;
        }

        public void UnsubscribeFromEnableEvents()
        {
            Button.onPointerClick -= OnButtonPointerClick;
        }

        private void OnButtonPointerClick(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (pointerListener == button)
            {
                onClicked?.Invoke(this);
            }
        }
    }
}