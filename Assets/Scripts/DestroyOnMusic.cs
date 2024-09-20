using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnMusic : MonoBehaviour
{
    public AudioSource m_AudioSource;
    private void Update()
    {
        if (!m_AudioSource.isPlaying) Destroy(gameObject);
    }
}
