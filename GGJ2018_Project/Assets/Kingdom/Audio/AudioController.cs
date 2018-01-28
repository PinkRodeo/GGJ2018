using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Kingdom
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer m_AudioMixer;

        private AudioMixerSnapshot mainSnapshot;
        private AudioMixerSnapshot eventSnapshot;

        public void Start()
        {
            mainSnapshot = m_AudioMixer.FindSnapshot("Overworld");
            eventSnapshot = m_AudioMixer.FindSnapshot("InEvent");
            eventSnapshot.TransitionTo(3f);
        }

    }

}
