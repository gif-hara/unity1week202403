using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource bgmAudioSource;

        [SerializeField]
        private AudioSource seAudioSource;

        public void PlayOneShot(AudioClip clip)
        {
            seAudioSource.PlayOneShot(clip);
        }

        public void PlayBgm(AudioClip clip)
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }

        public void StopBgm()
        {
            bgmAudioSource.Stop();
        }

        public bool IsPlayingBgm()
        {
            return bgmAudioSource.isPlaying;
        }
    }
}
