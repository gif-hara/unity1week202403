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

        public void PlayLoop(AudioClip clip)
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }

        public void StopLoop()
        {
            bgmAudioSource.Stop();
        }
    }
}
