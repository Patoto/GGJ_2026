using System;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class EventSubscriber : MyMonoBehaviour
    {
        private bool subscribedToEvents;

        public void Setup()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            onInitialized += OnMyMonoBehaviourInitialized;
            onEnabled += OnMyMonoBehaviourEnabled;
            onDisabled += OnMyMonoBehaviourDisabled;
            onAboutToBeDestroyed += OnMyMonoBehaviourAboutToBeDestroyed;
            subscribedToEvents = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TryToUnsubscribeFromEvents();
        }

        private void TryToUnsubscribeFromEvents()
        {
            if (subscribedToEvents)
            {
                UnsubscribeFromEvents();
            }
        }

        private void UnsubscribeFromEvents()
        {
            onInitialized -= OnMyMonoBehaviourInitialized;
            onEnabled -= OnMyMonoBehaviourEnabled;
            onDisabled -= OnMyMonoBehaviourDisabled;
            onAboutToBeDestroyed -= OnMyMonoBehaviourAboutToBeDestroyed;
        }

        private void OnMyMonoBehaviourInitialized(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour is IEventSubscriberDeclarator eventSubscriberDeclarator)
            {
                eventSubscriberDeclarator.SubscribeToInitializeEvents();
            }
        }

        private void OnMyMonoBehaviourEnabled(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour is IEventSubscriberDeclarator eventSubscriberDeclarator)
            {
                eventSubscriberDeclarator.SubscribeToEnableEvents();
            }
        }

        private void OnMyMonoBehaviourDisabled(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour is IEventSubscriberDeclarator eventSubscriberDeclarator)
            {
                eventSubscriberDeclarator.UnsubscribeFromEnableEvents();
            }
        }

        private void OnMyMonoBehaviourAboutToBeDestroyed(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour is IEventSubscriberDeclarator eventSubscriberDeclarator)
            {
                eventSubscriberDeclarator.UnsubscribeFromInitializeEvents();
            }
        }
    }
}