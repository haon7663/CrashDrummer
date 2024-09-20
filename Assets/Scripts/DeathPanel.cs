using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public Text m_Text;
    private void OnEnable()
    {
        float score = float.Parse(m_Text.text) * (1 + (float)GameManager.instance.m_WaveCount / 10);
        if(PlayerPrefs.HasKey("FullScore"))
        {
            if(PlayerPrefs.GetInt("FullScore") < Mathf.RoundToInt(score))
            {
                PlayerPrefs.SetInt("FullScore", Mathf.RoundToInt(score));
            }
        }
    }
}
