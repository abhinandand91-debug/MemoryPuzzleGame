using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] private AudioClip backgroundMusic;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip cardFlipClip;
        [SerializeField] private AudioClip matchFoundClip;
        [SerializeField] private AudioClip mismatchClip;
        [SerializeField] private AudioClip gameOverClip;
        [SerializeField] private AudioClip buttonClickClip;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Auto start background music
            if (backgroundMusic != null)
                PlayMusic();
        }

        #region Music
        public void PlayMusic(bool loop = true)
        {
            if (backgroundMusic == null) return;
            musicSource.clip = backgroundMusic;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
        #endregion

        #region SFX - Explicit Methods
        public void PlayCardFlip()
        {
            PlaySFX(cardFlipClip);
        }

        public void PlayMatchFound()
        {
            PlaySFX(matchFoundClip);
        }

        public void PlayMismatch()
        {
            PlaySFX(mismatchClip);
        }

        public void PlayGameOver()
        {
            PlaySFX(gameOverClip);
        }

        public void PlayButtonClick()
        {
            PlaySFX(buttonClickClip);
        }
        #endregion

        #region Internal
        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
        #endregion
    }
}
