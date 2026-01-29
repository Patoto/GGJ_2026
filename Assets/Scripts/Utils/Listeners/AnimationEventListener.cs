using System;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class AnimationEventListener : MyMonoBehaviour
    {
        public static Action<AnimationEventListener> onCardboardBoxReachedSpawnPieceFrame;

        public void OnCardboardBoxReachedSpawnPieceFrame() 
        {
            onCardboardBoxReachedSpawnPieceFrame?.Invoke(this);
        }
    }
}