using PixelDust.Audiophile;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Nato.Sound
{
    public class MasterSound : MonoBehaviour
    {
        public static MasterSound Instance;
        public enum AUDIO_OUTPUT
        {
            MUSIC,
            EFFECT
        };

        public static readonly string MUSIC_KEY = "music_configuration";
        public static readonly string EFFECT_KEY = "effect_configuration";


        private const string MUSIC_VOLUME_PARAMETER = "MUSIC_VOLUME";
        private const string EFFECT_VOLUME_PARAMETER = "EFFECT_VOLUME";
        private static SoundEvent previousSound;

        [SerializeField] private AudioMixer audioMixer;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            SettingAudioVolume();
        }

        private void SettingAudioVolume()
        {
            float musicVolume = GetSavedVolume(AUDIO_OUTPUT.MUSIC);
            SetVolume(musicVolume, AUDIO_OUTPUT.MUSIC);

            float effectVolume = GetSavedVolume(AUDIO_OUTPUT.EFFECT);
            SetVolume(effectVolume, AUDIO_OUTPUT.EFFECT);
        }

        public static void SetVolume(float volume, AUDIO_OUTPUT output)
        {
            switch (output)
            {
                case AUDIO_OUTPUT.MUSIC:
                    Instance.audioMixer.SetFloat(MUSIC_VOLUME_PARAMETER, volume);
                    break;
                case AUDIO_OUTPUT.EFFECT:
                    Instance.audioMixer.SetFloat(EFFECT_VOLUME_PARAMETER, volume);
                    break;
            }
        }

        public static void Play(SoundEvent sound)
        {
            sound.Play();
        }

        public static void PlayAt(SoundEvent sound, Transform transform, float delay = 0, string overrideID = null, bool follow = false)
        {
            sound.PlayAt(transform, delay, overrideID, follow);
        }

        public static void PlayMusic(SoundEvent sound)
        {
            Instance.StopAllCoroutines();
            SetVolume(GetSavedVolume(AUDIO_OUTPUT.MUSIC), AUDIO_OUTPUT.MUSIC);
            if (previousSound != null)
                previousSound.Stop();
            sound.Stop();
            sound.Play();
            previousSound = sound;
        }



        public static void CrossfadeTo(SoundEvent newClip, float durationFade, float delay)
        {
            Instance.StartCoroutine(Instance.FadeOutIn(newClip, durationFade, delay));
        }

        public static void StopMusicFade(float durationFade)
        {
            Instance.StartCoroutine(Instance.FadeVolume(-80, durationFade));
        }

        private IEnumerator FadeOutIn(SoundEvent newClip, float durationFade, float delay)
        {
            yield return StartCoroutine(FadeVolume(-80f, durationFade));
            yield return new WaitForSeconds(delay);

            if (previousSound != null)
                previousSound.Stop();
            newClip.Stop();
            newClip.Play();
            previousSound = newClip;

            float targetVolume = GetSavedVolume(AUDIO_OUTPUT.MUSIC);
            yield return StartCoroutine(FadeVolume(targetVolume, durationFade));
        }

        private IEnumerator FadeVolume(float targetVolume, float duration)
        {
            float currentTime = 0;
            Instance.audioMixer.GetFloat(MUSIC_VOLUME_PARAMETER, out float currentVolume);
            float startVolume = currentVolume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVolume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                SetVolume(newVolume, AUDIO_OUTPUT.MUSIC);
                yield return null;
            }
            SetVolume(targetVolume, AUDIO_OUTPUT.MUSIC);
        }

        public static float GetSavedVolume(AUDIO_OUTPUT output)
        {
            switch (output)
            {
                case AUDIO_OUTPUT.MUSIC:
                    return PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAMETER, defaultValue: 0);
                case AUDIO_OUTPUT.EFFECT:
                    return PlayerPrefs.GetFloat(EFFECT_VOLUME_PARAMETER, defaultValue: 0);
            }
            return 0;
        }

        public static void SaveVolume(float volume, AUDIO_OUTPUT output)
        {
            switch (output)
            {
                case AUDIO_OUTPUT.MUSIC:
                    PlayerPrefs.SetFloat(MUSIC_KEY, volume);
                    break;
                case AUDIO_OUTPUT.EFFECT:
                    PlayerPrefs.SetFloat(EFFECT_KEY, volume);
                    break;
            }
        }
    }
}