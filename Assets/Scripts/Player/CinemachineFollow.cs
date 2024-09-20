using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CinemachineFollow : MonoBehaviour
{
    public static CinemachineFollow instance;
    public CinemachineVirtualCamera m_Cinevirtual;
    private CinemachineTransposer m_CinemachineTransposer;

    [Space]
    [Header("Light")]
    public UnityEngine.Rendering.Universal.Light2D m_GlobalLight;
    public UnityEngine.Rendering.Universal.Light2D m_SpotLight;

    public Transform m_Player;
    public Transform m_Enemy;
    public Image m_DelayBar;

    public bool isProducing;

    public float m_MinSize;
    public float m_MaxSize;

    public float m_RealSize = 7;
    float setSize = 7;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        m_RealSize = Mathf.Lerp(m_RealSize, setSize, Time.deltaTime * 9);
        m_Cinevirtual.m_Lens.OrthographicSize = m_RealSize;
        m_DelayBar.color = Color.Lerp(m_DelayBar.color, new Color(1, 1, 1, isProducing ? 1 : 0), Time.deltaTime * 6);

        if (!m_Player) return;
        if (isProducing)
        {
            setSize = m_MinSize;
            transform.position = (m_Enemy.position + m_Player.position) / 2 + new Vector3(0, 1.5f);

            m_GlobalLight.intensity = Mathf.Lerp(m_GlobalLight.intensity, 0.1f, Time.deltaTime * 9);
            m_SpotLight.transform.position = Vector3.Lerp(m_SpotLight.transform.position, transform.position + new Vector3(0, 7), Time.deltaTime * 12);
            m_SpotLight.intensity = Mathf.Lerp(m_SpotLight.intensity, 2.2f, Time.deltaTime * 6);
            m_SpotLight.pointLightInnerAngle = Mathf.Lerp(m_SpotLight.pointLightInnerAngle, 55, Time.deltaTime * 6);
        }
        else
        {
            m_GlobalLight.intensity = Mathf.Lerp(m_GlobalLight.intensity, 0.9f, Time.deltaTime * 4);
            setSize = m_MaxSize;
            transform.position = new Vector3(m_Player.position.x, 2);

            m_SpotLight.transform.position = Vector3.Lerp(m_SpotLight.transform.position, transform.position + new Vector3(0, 13), Time.deltaTime * 5);
            m_SpotLight.intensity = Mathf.Lerp(m_SpotLight.intensity, 0, Time.deltaTime * 6);
            m_SpotLight.pointLightInnerAngle = Mathf.Lerp(m_SpotLight.pointLightInnerAngle, 0, Time.deltaTime * 6);
        }
    }
}
