using DigitalRubyShared;
using System;
using UnityEngine;

namespace GGJ_2026
{
    public class InputsManager : Manager, IEventSubscriberDeclarator
    {
        [SerializeField] private SwipeGestureRecognizerComponentScript swipeDownGestureRecognizer;

        public static Action<GestureRecognizer> onSwipeDownGestureEnded;

        public void SubscribeToEnableEvents()
		{
            swipeDownGestureRecognizer.Gesture.StateUpdated += OnSwipeDownGestureRecognizerStateUpdated;
		}

        public void UnsubscribeFromEnableEvents()
		{
            swipeDownGestureRecognizer.Gesture.StateUpdated -= OnSwipeDownGestureRecognizerStateUpdated;
		}

        private void OnSwipeDownGestureRecognizerStateUpdated(GestureRecognizer gestureRecognizer)
        {
            if (gestureRecognizer.State == GestureRecognizerState.Ended)
            {
                onSwipeDownGestureEnded?.Invoke(gestureRecognizer);
            }
        }
    }
}