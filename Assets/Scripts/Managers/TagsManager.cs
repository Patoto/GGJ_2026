using UnityEngine;

namespace PortalRollerCoaster
{
    public class TagsManager : Manager
    {
        [SerializeField] public Tag surfaceDraggerTriggerTag;
        [SerializeField] public Tag draggableSurfaceColliderTag;
        [SerializeField] public Tag pathColliderTag;
        [SerializeField] public Tag portalableSurfaceTag;
        [SerializeField] public Tag portalTransporterTriggerTag;
        [SerializeField] public Tag platformColliderTag;
    }
}