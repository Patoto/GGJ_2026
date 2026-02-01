using System;
using UnityEngine;

namespace GGJ_2026
{
    public class Mom : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        public void SubscribeToEnableEvents()
		{
			LoseHandler.onAboutToShowJumpscareMom += OnLoseHandlerAboutToShowJumpscareMom;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LoseHandler.onAboutToShowJumpscareMom -= OnLoseHandlerAboutToShowJumpscareMom;
        }

        private void OnLoseHandlerAboutToShowJumpscareMom()
		{
			Destroy(gameObject);
		}
    }
}