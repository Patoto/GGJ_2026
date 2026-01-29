using UnityEngine;

namespace PortalRollerCoaster
{
    public interface IEventSubscriberDeclarator
    {
        public abstract void SubscribeToEnableEvents();
        public abstract void UnsubscribeFromEnableEvents();

        public virtual void SubscribeToInitializeEvents() { }
        public virtual void UnsubscribeFromInitializeEvents() { }
    }
}