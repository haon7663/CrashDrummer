using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Movement m_Movement;
    public GameObject m_MainPanel;

    public Transform m_Canvas;

    public Text m_ScoreText;
    public Text m_BonusText;
    public int m_Score;

    public Image m_RankImage;
    public Sprite[] m_RankSprites;
    public GameObject m_RankFire;
    public int m_Rank;
    public int[] m_BonusScore;

    public GameObject m_WaveCountText;
    public Text m_LastScore;
    public int m_WaveCount;

    [Serializable]
    public struct WaveStruct
    {
        public GameObject[] m_Summon;
    }

    public WaveStruct[] m_WaveStruct;
    public List<GameObject> m_Enemy;

    private void Awake()
    {
        instance = this;
        m_Movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    private void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        Screen.SetResolution(setWidth, setHeight, true);
    }

    private void Update()
    {
        if(m_LastScore.enabled) m_LastScore.text = (Mathf.RoundToInt(m_Score * (1 + (float)m_WaveCount / 10))).ToString();
        m_ScoreText.text = m_Score.ToString();
        m_BonusText.text = "X" + (1 + (float)m_WaveCount / 10).ToString();
        m_RankImage.sprite = m_RankSprites[m_Rank > 4 ? 4 : m_Rank];
        m_RankFire.SetActive(m_Rank >= 4);

        if (m_Enemy.Count <= 0 && !m_MainPanel)
        {
            if (++m_Movement.curhp > m_Movement.maxhp) m_Movement.curhp = m_Movement.maxhp;
            m_Movement.HealthSet();
            Text text = Instantiate(m_WaveCountText).GetComponent<Text>();
            text.text = "Wave " + (m_WaveCount + 1).ToString();
            text.transform.SetParent(m_Canvas);
            text.transform.localScale = Vector3.one;
            for (int i = 0; i < m_WaveStruct[m_WaveCount].m_Summon.Length; i++)
            {
                GameObject summon = Instantiate(m_WaveStruct[m_WaveCount].m_Summon[i], new Vector3(Random.Range(-29, 29), m_WaveStruct[m_WaveCount].m_Summon[i].transform.position.y), Quaternion.identity);
                m_Enemy.Add(summon);
            }
            m_WaveCount++;
        }
    }

    public void Restart()
    {
        Fade.instance.FadeIn();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
