using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBeat : MonoBehaviour
{
    private Camera m_Camera;
    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_Animator;
    private Movement m_Movement;

    public AudioClip[] m_AudioClips;

    public Transform m_Canvas;
    public GameObject m_BeatKey;
    public GameObject m_Particle;
    public GameObject m_LandParticle;

    private Image m_DelayBar;

    public LayerMask m_EnemyLayer;
    public LayerMask m_PlayerLayer;

    public float speed;
    public int maxhp;
    public int curhp;
    public float multifulExp = 1;
    public int[] m_Beat;

    public Color[] m_ConditionColor;
    public Vector3[] m_ConditionSize;

    public bool isSelect;

    public Material[] m_Material;

    Key[] beatKey;
    float hitTime, delayTime;
    float stopTime = 0.5f;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        m_Canvas = GameObject.Find("Canvas").transform;
        m_DelayBar = GameObject.Find("AttackDelay_Bar").GetComponent<Image>();

        Instantiate(m_LandParticle, new Vector3(transform.position.x, -2.75f), Quaternion.identity);

        m_Camera = Camera.main;
        m_Beat = new int[maxhp];
        for(int i = 0; i < maxhp; i++)
        {
            m_Beat[i] = Random.Range(0, 3);
        }
        ShowBeat();
    }

    public void ShowBeat()
    {
        beatKey = new Key[maxhp];
        for (int i = maxhp - 1; i >= 0; i--)
        {
            beatKey[i] = Instantiate(m_BeatKey, transform.position + new Vector3(0, 3), Quaternion.identity).GetComponent<Key>();
            beatKey[i].m_Image.sprite = beatKey[i].m_Sprites[m_Beat[i]];
            beatKey[i].transform.SetParent(m_Canvas);
            beatKey[i].transform.localScale = Vector3.one;
        }
    }

    private void LateUpdate()
    {
        if (!m_Movement.enabled || stopTime > 0)
        {
            stopTime -= Time.deltaTime;
            return;
        }
        if (isSelect)
        {
            m_SpriteRenderer.sortingLayerName = "Spot";
            for (int i = 0; i < m_Beat.Length; i++)
            {
                int setCondition = i - (maxhp - curhp) < 0 ? 4 : i - (maxhp - curhp);
                if (setCondition > m_ConditionColor.Length - 1) setCondition = m_ConditionColor.Length - 1;
                beatKey[i].transform.position = Vector3.Lerp(beatKey[i].transform.position, transform.position + new Vector3((i - (maxhp - curhp)) * 1, 3f), Time.deltaTime * 5);
                beatKey[i].m_Image.color = Color.Lerp(beatKey[i].m_Image.color, m_ConditionColor[setCondition], Time.deltaTime * 13);
                beatKey[i].transform.localScale = Vector3.Lerp(beatKey[i].transform.localScale, m_ConditionSize[setCondition], Time.deltaTime * 4);
            }
            m_DelayBar.fillAmount = delayTime;
            if (delayTime > 0)
                delayTime -= Time.deltaTime;
            else
            {
                OnAttack();
            }
        }
        else
        {
            m_SpriteRenderer.sortingLayerName = "Enemy";
            for (int i = 0; i < m_Beat.Length; i++)
            {
                int setCondition = i == (maxhp - curhp) ? 0 : 4;
                if (setCondition > m_ConditionColor.Length - 1) setCondition = m_ConditionColor.Length - 1;
                beatKey[i].transform.position = transform.position + new Vector3(0, 3f);
                if (CinemachineFollow.instance.isProducing && !isSelect)
                    beatKey[i].m_Image.color = Color.Lerp(beatKey[i].m_Image.color, new Color(0.1f, 0.1f, 0.1f, 1), Time.deltaTime * 8);
                else
                    beatKey[i].m_Image.color = Color.Lerp(beatKey[i].m_Image.color, m_ConditionColor[setCondition], Time.deltaTime * 8);
                beatKey[i].transform.localScale = Vector3.Lerp(beatKey[i].transform.localScale, m_ConditionSize[setCondition], Time.deltaTime * 8);
            }
        }

        m_SpriteRenderer.material = m_Material[hitTime > 0 ? 1 : 0];
        if(hitTime > 0)
            hitTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!m_Movement.enabled || stopTime > 0)
        {
            stopTime -= Time.deltaTime;
            return;
        }
        m_Rigidbody2D.velocity = new Vector3(0, 0);
        if (!CinemachineFollow.instance.isProducing && !isSelect)
        {
            transform.localScale = new Vector3(transform.position.x > m_Movement.transform.position.x ? 1 : -1, 1, 1);
            if (Physics2D.Raycast(transform.position, new Vector3(transform.localScale.x * -2.25f, 0), 1.5f, m_PlayerLayer))
            {
                m_Animator.SetBool("isLongAttack", true);
                m_Rigidbody2D.velocity = new Vector3(0, 0);
            }
            else if(!m_Animator.GetBool("isLongAttack"))
            {
                var hit = Physics2D.OverlapCircle(transform.position + new Vector3(transform.localScale.x * -1.9f, 0), 0.2f, m_EnemyLayer);
                if (!hit) m_Rigidbody2D.velocity = new Vector3(transform.localScale.x * -speed * Time.deltaTime, 0);
            }
        }
    }

    public void OnHitScan()
    {
        m_Animator.SetBool("isLongAttack", false);
        if (Physics2D.Raycast(transform.position, new Vector3(transform.localScale.x * -2.25f, 0), 1.5f, m_PlayerLayer) && !CinemachineFollow.instance.isProducing && !isSelect) OnAttack();
    }

    public void OnAttack()
    {
        delayTime = 1;
        m_Animator.SetTrigger("attack");
        m_Movement.OnDamage(transform);
        CinemachineFollow.instance.isProducing = curhp > 0;
        CinemachineFollow.instance.m_RealSize = 4;
        CinemachineFollow.instance.m_Enemy = transform;
        isSelect = curhp > 0;
    }

    public void OnDamage()
    {        
        curhp--;
        hitTime = 0.06f;
        CinemachineShake.Instance.ShakeCamera(10, 0.15f);
        CinemachineFollow.instance.isProducing = curhp > 0;
        CinemachineFollow.instance.m_RealSize = 4;
        CinemachineFollow.instance.m_Enemy = transform;
        isSelect = curhp > 0;
        delayTime = 1;

        AudioSource audio = Instantiate(m_Particle, transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f)), Quaternion.Euler(0, 0, Random.Range(0, 360f))).GetComponent<AudioSource>();
        audio.clip = m_AudioClips[Random.Range(0, m_AudioClips.Length)];
        audio.Play();

        if (curhp <= 0)
        {
            for(int i = 0; i < beatKey.Length; i++)
            {
                Destroy(beatKey[i].gameObject);
            }
            GameManager.instance.m_Enemy.Remove(gameObject);
            GameManager.instance.m_Score += (5 + GameManager.instance.m_BonusScore[GameManager.instance.m_Rank]) * (int)multifulExp;
            if (GameManager.instance.m_Rank < 4)
            {
                GameManager.instance.m_Rank += 1;
                BackGroundMusic.instance.Switch(GameManager.instance.m_Rank);
            }
            Destroy(gameObject);
        }
    }
}
