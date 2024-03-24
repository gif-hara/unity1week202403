using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        public void PlayOneShot(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void PlayLoop(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}
