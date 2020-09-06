using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class AudioController : MonoBehaviour {
        private AudioSource source;

        private float origVolume;
        private bool play = false;

        private void Start() {
            source = GetComponent<AudioSource>();

            origVolume = source.volume;
        }
        
        public void Play(AudioClip clip, bool loop) {
            if (!clip) return;

            play = true;
            
            // Stop the current audio
            // StartCoroutine(FadeToNewClip(clip, loop));
            
            source.Stop();
            
            // Prepare the new audio clip
            source.clip = clip;
            source.volume = origVolume;
            source.loop = loop;
            
            // Play the new audio clip
            source.Play();
        }

        public void Stop() {
            play = false;
            
            source.Stop();
        }

        private IEnumerator FadeToNewClip(AudioClip clip, bool loop) {
            float timer = 0;
            float duration = 0.1f;

            while (timer < duration) {
                timer += Time.deltaTime;

                source.volume = origVolume * ((duration - timer) / duration);

                yield return null;
            }

            source.volume = 0;
            // source.Stop();
            
            // Prepare the new audio clip
            source.clip = clip;
            source.volume = origVolume;
            source.loop = loop;
            
            // Play the new audio clip
            if (play)
                source.Play();
        }
    }
}
