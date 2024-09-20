using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatAttack : MonoBehaviour
{
    private Animator m_Animator;
    private Movement m_Movement;

    public LayerMask m_EnemyLayer;


    float leftTime, rightTime, moveTime;
    bool onleft, onright;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (!m_Movement.enabled) return;
        m_Animator.SetBool("isProducting", CinemachineFollow.instance.isProducing);
        if (Input.GetKeyDown(KeyCode.J))
        {
            leftTime = 0.015f;
            onleft = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            rightTime = 0.015f;
            onright = true;
        }

        bool cantAttack = m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_LeftAttack_Ready") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_RightAttack_Ready") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_AllAttack_Ready");
        if (leftTime > 0 && rightTime > 0)
        {
            rightTime = 0;
            leftTime = 0;
            onright = false;
            onleft = false;
            if(!cantAttack)
            {
                m_Animator.SetTrigger("allAttack");
                moveTime = 0.6f;
            }
        }
        else if(Input.GetKey(KeyCode.J) && leftTime <= 0 && onleft)
        {
            onleft = false;
            if (!cantAttack)
            {
                m_Animator.SetTrigger("leftAttack");
                moveTime = 0.6f;
            }
        }
        else if(Input.GetKey(KeyCode.K) && rightTime <= 0 && onright)
        {
            onright = false;
            if (!cantAttack)
            {
                m_Animator.SetTrigger("rightAttack");
                moveTime = 0.6f;
            }
        }
        m_Movement.isCantMove = moveTime > 0;
        leftTime -= Time.deltaTime;
        rightTime -= Time.deltaTime;
        moveTime -= Time.deltaTime;
    }

    public void ToDamage(int index)
    {
        Attack(index);
    }

    private void Attack(int index)
    {
        var collider = Physics2D.OverlapBox(transform.position + new Vector3(transform.localScale.x * 1.5f, 0), new Vector2(2f, 4), 0, m_EnemyLayer);
        if (collider)
        {
            var setBeat = collider.GetComponent<SetBeat>();
            if (setBeat && setBeat.m_Beat[setBeat.maxhp - setBeat.curhp] == index)
            {
                setBeat.OnDamage();
            }
            else
            {
                setBeat.OnAttack();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(transform.localScale.x * 1.5f, 0), new Vector2(2f, 4));
    }
}
