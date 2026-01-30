using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class DeletePersistentDataButton : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Button button;

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
                GameManager.instance.persistentDataManager.DeletePersistentData();
            }
        }
    }
}