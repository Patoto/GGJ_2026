using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ_2026
{
    [RequireComponent(typeof(AudioSource))]
    public class Sound : MyMonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClipsList;
        [SerializeField] private bool useRandomRangePitch;
        [SerializeField][ShowIf(nameof(useRandomRangePitch))] private float pitchRadius;

        [NonSerialized] public AudioSource audioSource;
        [NonSerialized] public float originalVolume;

        private const float DEFAULT_FADE_VOLUME_SECONDS = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

        private void Setup()
        {
            SetupReferences();
            SetRandomAudioClip();
            CheckForRandomizePitch();
        }

        private void SetupReferences()
        {
            audioSource = GetComponent<AudioSource>();
            originalVolume = audioSource.volume;
        }

        private void SetRandomAudioClip()
        {
            audioSource.clip = audioClipsList.GetRandom();
        }

        private void CheckForRandomizePitch()
        {
            if (useRandomRangePitch)
            {
                RandomizePitch();
            }
        }

        private void RandomizePitch()
        {
            audioSource.pitch += Random.Range(-pitchRadius, pitchRadius);
        }

        private void Start()
        {
            TryToPlaySound();
        }

        private void TryToPlaySound()
        {
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

        private void Update()
        {
            CheckForDestroy();
        }

        private void CheckForDestroy()
        {
            if (!audioSource.isPlaying)
            {
                DestroyMe();
            }
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }

        [Button]
        private void TestSound()
        {
            SetupReferences();
            SetRandomAudioClip();
            TryToPlaySound();
        }

        public Tween FadeOutAndDestroy(float seconds = DEFAULT_FADE_VOLUME_SECONDS)
        {
            return FadeToFixedVolume(0f, seconds).OnComplete(DestroyMe);
        }

        public Tween FadeIn(float seconds = DEFAULT_FADE_VOLUME_SECONDS)
        {
            audioSource.volume = 0f;
            return FadeToFixedVolume(originalVolume, seconds);
        }

        public Tween FadeToFixedVolume(float fixedVolume, float seconds = DEFAULT_FADE_VOLUME_SECONDS)
        {
            return audioSource.DOFade(fixedVolume, seconds);
        }

        public Tween FadeToVolumePercentage(float volumePercentageMultiplier, float seconds = DEFAULT_FADE_VOLUME_SECONDS)
        {
            return FadeToFixedVolume(originalVolume * volumePercentageMultiplier, seconds);
        }

        public void ToggleSpatialBlend(bool on)
        {
            audioSource.spatialBlend = 0f;
            if (on)
            {
                audioSource.spatialBlend = 1f;
            }
        }

        public void Stop()
        {
            DestroyMe();
        }
    }
}