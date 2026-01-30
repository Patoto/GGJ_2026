using System.Collections;
using UnityEngine;

namespace GGJ_2026
{
    public class Transition : MyMonoBehaviour
    {
        public enum Type
        {
            Fade,
            InvertedMask
        }

        [Header("Settings")]
        [SerializeField] public Type type;
        [Header("References")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Animator animator;

        private const string OUT_ANIMATION_NAME = "Out";
        private const string IN_ANIMATION_NAME = "In";

        private float halfTransitionTime;

        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

        private void Setup()
        {
            rectTransform.AnchorToCorners();
        }

        public IEnumerator PlayTransitionOutCoroutine(float seconds)
        {
            halfTransitionTime = seconds;
            SetupAnimatorSpeed();
            animator.Play(OUT_ANIMATION_NAME);
            yield return new WaitForSeconds(seconds);
        }

        public IEnumerator PlayTransitionInCoroutine()
        {
            animator.Play(IN_ANIMATION_NAME);
            yield return new WaitForSeconds(halfTransitionTime);
            Destroy(gameObject);
        }

        private void SetupAnimatorSpeed()
        {
            float animatorSpeed = 1f / halfTransitionTime;
            animator.speed = animatorSpeed;
        }
    }
}