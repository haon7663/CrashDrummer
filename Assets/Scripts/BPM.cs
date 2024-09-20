using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPM : MonoBehaviour
{
    public static BPM instance;
    private AudioSource m_AudioSource;

    public float m_MusicBPM;
    public float m_StandardBPM;
    public float m_MusicTemp;
    public float m_StandardTemp;

    public float tikTime = 0;
    public float nextTime = 0;

    public bool onHit;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        tikTime = (m_StandardBPM / m_MusicBPM) * (m_StandardTemp / m_MusicTemp);
        nextTime += Time.deltaTime;
        if (nextTime > tikTime)
        {
            m_AudioSource.Play();
            transform.Rotate(new Vector3(0, 0, 60));
            nextTime = 0;
        }

        onHit = nextTime > tikTime - 0.125f || nextTime < 0.125f;
    }
}
