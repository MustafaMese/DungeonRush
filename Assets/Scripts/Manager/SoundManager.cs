using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Managers
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance = null;
        // Game Instance Singleton
        public static SoundManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private List<AudioSource> effectsSources = new List<AudioSource>();

        [SerializeField] AudioSource musicSource;
        public AudioClip hurt;

        [SerializeField] float lowPitchRange = 0.95f;
        [SerializeField] float highPitchRange = 1.05f;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            musicSource.Play();
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level > 1)
                musicSource.Stop();
            else
            {
                if (!musicSource.isPlaying)
                    musicSource.Play();
            }
        }

        public void SelectAudioSource(AudioClip clip)
        {
            AudioSource a;
            for (int i = 0; i < effectsSources.Count; i++)
            {
                a = effectsSources[i];
                if (!a.isPlaying)
                {
                    PlayEffect(a, clip);
                    return;
                }
            }

            a = AddAudioSource();
            PlayEffect(a, clip);
        }

        private AudioSource AddAudioSource() 
        {
            AudioSource audio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            effectsSources.Add(audio);
            return audio;
        }

        private void PlayEffect(AudioSource effectsSource, AudioClip clip)
        {
            effectsSource.clip = clip;
            effectsSource.Play();
        }

        public void PlayMusic()
        {
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

    }
}