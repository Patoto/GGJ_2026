using System;
using UnityEngine;

namespace GGJ_2026
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