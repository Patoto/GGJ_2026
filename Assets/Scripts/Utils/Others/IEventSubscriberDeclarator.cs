using UnityEngine;

namespace GGJ_2026
{
    public interface IEventSubscriberDeclarator
    {
        public abstract void SubscribeToEnableEvents();
        public abstract void UnsubscribeFromEnableEvents();

        public virtual void SubscribeToInitializeEvents() { }
        public virtual void UnsubscribeFromInitializeEvents() { }
    }
}