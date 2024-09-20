using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinMove : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Length;

    private Camera m_Camera;
    Vector2 savePos;
    float sinTimer;

    Image m_Panel;
    Text m_Text, m_ScoreText, m_PressText;

    bool isPress;

    private void Start()
    {
        m_Panel = GetComponentInParent<Image>();
        m_Text = GetComponent<Text>();
        m_ScoreText = transform.parent.GetChild(1).GetComponent<Text>();
        m_PressText = transform.parent.GetChild(2).GetComponent<Text>();

        m_Camera = Camera.main;
        savePos = transform.position;

        if (PlayerPrefs.HasKey("FullScore")) m_ScoreText.text = "MAXSCORE: " + PlayerPrefs.GetInt("FullScore");
        else
        {
            PlayerPrefs.SetInt("FullScore", 0);
            m_ScoreText.text = "MAXSCORE: " + PlayerPrefs.GetInt("FullScore");
        }
    }
    private void Update()
    {
        sinTimer += m_Speed * Time.deltaTime;
        transform.position = new Vector3(savePos.x, savePos.y + Mathf.Sin(sinTimer) * m_Length);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPress = true;
        }

        if(isPress)
        {
            m_Panel.color -= new Color(0, 0, 0, Time.deltaTime*0.95f);
            m_Text.color -= new Color(0, 0, 0, Time.deltaTime);
            m_ScoreText.color -= new Color(0, 0, 0, Time.deltaTime);
            m_PressText.color -= new Color(0, 0, 0, Time.deltaTime);
            if (m_Text.color.a <= 0) Destroy(m_Panel.gameObject);
        }
    }
}
