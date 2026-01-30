using NaughtyAttributes;
using UnityEngine;

namespace GGJ_2026
{
    public class AnimationFrameSetter : MyMonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string stateName;
        [SerializeField] private int frame;
        [SerializeField] private int layer = -1;

        [Button]
        private void SetAnimationAtFrame()
        {
            AnimationClip animationClip = animator.GetAnimationClip(stateName);
            float totalFrames = animationClip.length * animationClip.frameRate;
            float normalizedTime = frame / totalFrames;
            animator.Play(stateName, layer, normalizedTime);
            animator.Update(0);
        }
    }
}