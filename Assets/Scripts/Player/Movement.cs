using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;
    private CapsuleCollider2D m_CapsuleCollider2D;
    private SpriteRenderer m_SpriteRenderer;

    public Image[] m_Hearts;
    public Sprite[] m_HeartSprite;
    public GameObject m_Death;

    public int maxhp;
    public int curhp;

    public float m_Speed;
    public Material[] m_Material;

    public bool isCantMove;

    float x, y, hitTime;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isCantMove) x = CinemachineFollow.instance.isProducing ? 0 : Input.GetAxisRaw("Horizontal");
        else if (isCantMove) x = 0;
        if (x != 0) transform.localScale = new Vector3(x, 1);

        m_Animator.SetBool("isRun", x != 0);

        m_SpriteRenderer.material = m_Material[hitTime > 0 ? 1 : 0];
        if (hitTime > 0)
            hitTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(x * m_Speed * Time.deltaTime, m_Rigidbody2D.velocity.y);
    }

    public void HealthSet()
    {
        for (int i = 0; i < 5; i++)
        {
            m_Hearts[i].sprite = m_HeartSprite[i >= curhp ? 1 : 0];
        }
    }
    public void OnDamage(Transform pos)
    {
        x = pos.position.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(x, 1);
        curhp--;
        
        for(int i = 0; i < 5; i++)
        {
            m_Hearts[i].sprite = m_HeartSprite[i >= curhp ? 1 : 0];
        }

        hitTime = 0.125f;
        GameManager.instance.m_Rank = 0;
        CinemachineShake.Instance.ShakeCamera(10, 0.2f);
        CinemachineFollow.instance.isProducing = curhp > 0;
        CinemachineFollow.instance.m_RealSize = 3;

        if (curhp <= 0)
        {
            m_Animator.SetBool("isDeath", true);
            m_Animator.SetTrigger("death");
            m_Death.SetActive(true);
            m_SpriteRenderer.material = m_Material[0];
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            m_CapsuleCollider2D.enabled = false;
            enabled = false;
        }
    }
}
