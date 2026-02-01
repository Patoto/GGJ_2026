using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class TitleHandler : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        public static Action onDeactivated;

        public void SubscribeToEnableEvents()
		{
			LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
        }

        private void Start()
        {
            GameManager.instance.persistentDataManager.startedALevelThisSessionAmount++;
			if (!GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				Deactivate();
			}
        }

        private void OnLevelPointerListenerPointerUp(PointerEventData pointerEventData)
        {
            Deactivate();
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
			onDeactivated?.Invoke();
        }
    }
}