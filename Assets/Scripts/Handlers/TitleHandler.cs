using System;
using TapticPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class TitleHandler : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Sound musicSound;

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
			if (GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				GameManager.instance.audioManager.PlayMusic(musicSound);
			}
			else
			{
				Deactivate();
			}
        }

        private void OnLevelPointerListenerPointerUp(PointerEventData pointerEventData)
        {
			TapticManager.Impact(ImpactFeedback.Medium);
            Deactivate();
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
			onDeactivated?.Invoke();
        }
    }
}