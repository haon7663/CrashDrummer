using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackGroundMusic : MonoBehaviour
{
    public static BackGroundMusic instance;

    public AudioClip[] m_AudioClips;

    private AudioSource audioSources;
    public AudioSource m_CameraAudioSource;

    public float[] m_Volume;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSources = GetComponent<AudioSource>();
        Switch(0);
    }

    private void Update()
    {
        if(audioSources.isPlaying)
        {
            m_CameraAudioSource.volume = Mathf.Lerp(m_CameraAudioSource.volume, 0, Time.deltaTime * 14);
        }
        else m_CameraAudioSource.volume = Mathf.Lerp(m_CameraAudioSource.volume, 0.35f, Time.deltaTime * 14);
    }

    public void Switch(int index)
    {
        audioSources.volume = m_Volume[index];
        audioSources.clip = m_AudioClips[index];
        audioSources.Play();
    }
}
