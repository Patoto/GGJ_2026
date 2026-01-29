using System;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class AudioManager : Manager, IEventSubscriberDeclarator
    {
        [SerializeField] private Sound selectSoundPrefab;
        [SerializeField] private Sound confirmSoundPrefab;
        [SerializeField] private Sound cancelSoundPrefab;
        [SerializeField] private Sound popupSoundPrefab;

        [NonSerialized] public Sound currentMusicSound;

        private const bool PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE = true;
        private const float PLAY_SOUND_FADE_IN_SECONDS_DEFAULT_VALUE = 0f;

        public override void Setup()
        {
            UpdateAudioOn();
        }

        public void SubscribeToEnableEvents()
        {
            PersistentData.onAudioOnChanged += OnPersistentDataAudioOnChanged;
        }

        public void UnsubscribeFromEnableEvents()
        {
            PersistentData.onAudioOnChanged -= OnPersistentDataAudioOnChanged;
        }

        public Sound PlaySound(Sound soundPrefab, Transform parent = null, bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE, float fadeInSeconds = PLAY_SOUND_FADE_IN_SECONDS_DEFAULT_VALUE)
        {
            Sound soundInstance = Instantiate(soundPrefab, parent);
            bool hasParent = parent != null;
            soundInstance.ToggleSpatialBlend(hasParent);
            if (!destroyOnLoad)
            {
                DontDestroyOnLoad(soundInstance);
            }
            if (fadeInSeconds > 0f)
            {
                soundInstance.FadeIn(fadeInSeconds);
            }
            return soundInstance;
        }

        public Sound PlaySound(Sound soundPrefab, Vector3 position, bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE, float fadeInSeconds = PLAY_SOUND_FADE_IN_SECONDS_DEFAULT_VALUE)
        {
            Sound sound = PlaySound(soundPrefab, parent: null, destroyOnLoad, fadeInSeconds);
            sound.transform.position = position;
            sound.ToggleSpatialBlend(true);
            return sound;
        }

        public void PlayMusic(Sound musicSoundPrefab)
        {
            if (currentMusicSound != null)
            {
                Destroy(currentMusicSound.gameObject);
            }
            currentMusicSound = PlaySound(musicSoundPrefab, destroyOnLoad: false);
        }

        private void UpdateAudioOn()
        {
            ToggleAudio(GameManager.instance.persistentDataManager.persistentData.audioOn);
        }

        private void ToggleAudio(bool on)
        {
            if (on)
            {
                AudioListener.volume = 1f;
            }
            else
            {
                AudioListener.volume = 0f;
            }
        }

        private void OnPersistentDataAudioOnChanged()
        {
            UpdateAudioOn();
        }

        public void PlaySelectSound(bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE)
        {
            PlaySound(selectSoundPrefab, destroyOnLoad: destroyOnLoad);
        }

        public void PlayConfirmSound(bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE)
        {
            PlaySound(confirmSoundPrefab, destroyOnLoad: destroyOnLoad);
        }

        public void PlayCancelSound(bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE)
        {
            PlaySound(cancelSoundPrefab, destroyOnLoad: destroyOnLoad);
        }

        public void PlayPopupSound(bool destroyOnLoad = PLAY_SOUND_DESTROY_ON_LOAD_DEFAULT_VALUE)
        {
            PlaySound(popupSoundPrefab, destroyOnLoad: destroyOnLoad);
        }
    }
}