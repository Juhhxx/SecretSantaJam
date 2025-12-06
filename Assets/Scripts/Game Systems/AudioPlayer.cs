    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Audio;

    // Create an asset, add the audios and randomness and play it somewhere by passing a source
    [CreateAssetMenu(menuName = "Game/Audio Clip Player")]
    public class AudioPlayer : ScriptableObject
    {
        [SerializeField] private bool loop = false;
        [SerializeField, MinMaxSlider(0.2f, 2)] private Vector2 pitchRange;
        [SerializeField, MinMaxSlider(0.4f, 1.5f)] private Vector2 volumeRange;

        [SerializeField, Label("Set a mixer to override the source output, empty if the source one")] 
        private AudioMixerGroup _overrideMixer;
        [Header("Clips - Picked at random")]
        [SerializeField] private AudioClip[] _clips;

        public void Play(AudioSource source)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }

            AudioClip clip = _clips[Random.Range(0, _clips.Length)];
            source.clip = clip;
            source.pitch = Random.Range(pitchRange.x, pitchRange.y);
            source.volume = Random.Range(volumeRange.x, volumeRange.y);
            source.loop = loop;

            if (_overrideMixer)
                source.outputAudioMixerGroup = _overrideMixer;
            source.Play();
        }
    }
