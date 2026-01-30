using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class FinishLevelButton : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Button button;

        public static Action<FinishLevelButton> onPointerClick;

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
                onPointerClick?.Invoke(this);
            }
        }
    }
}