using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
//using static UnityEditor.PlayerSettings;
//using static UnityEditor.Rendering.InspectorCurveEditor;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Preset Fields")] 
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject splashFx;
    [SerializeField] private HealthBar healthBar;
    
    [Header("Settings")]
    [SerializeField] private float attackRange;
    public float MaxHealth = 100.0f;
    [SerializeField] private float CurHealth;
    [SerializeField] private Vector3 CurPos;
    [SerializeField] private float moveSpeed = 3.0f; // 이동속도
    [SerializeField] private float rotateSpeed = 100.0f; // 회전속도

    private bool chooseDir = false;
    public int randomDir;

    [Header("Audio")]
    public AudioSource enemyAudioSource;
    public AudioClip attackSound;
    public AudioClip deathSound;


    public enum State 
    {
        None,
        Idle,
        Attack,
        Wander
    }
    
    [Header("Debug")]
    public State state = State.None;
    public State nextState = State.None;

    private bool attackDone;

    private void Start()
    {
        state = State.None;
        nextState = State.Idle;

        CurHealth = MaxHealth;
        healthBar.UpdateHealthBar(MaxHealth, CurHealth);

        CurPos = transform.position;

        enemyAudioSource = gameObject.AddComponent<AudioSource>();
        enemyAudioSource.playOnAwake = false;
        enemyAudioSource.spatialBlend = 1.0f; // 3D sound
    }

    private void Update()
    {
        //1. 스테이트 전환 상황 판단
        if (nextState == State.None) 
        {
            switch (state) 
            {
                case State.Idle:
                    //1 << 6인 이유는 Player의 Layer가 6이기 때문
                    if (IsPlayerInRange())
                    {
                        nextState = State.Attack;
                    }
                    else if (!IsPlayerInRange())
                    {
                        nextState = State.Wander;
                    }
                    break;

                case State.Attack:
                    if (attackDone)
                    {
                        nextState = State.Idle;
                        attackDone = false;
                    }
                    break;

                case State.Wander:
                    {
                        nextState = State.Idle;
                    }
                    break;
                
            }
        }
        
        //2. 스테이트 초기화
        if (nextState != State.None) 
        {
            state = nextState;
            nextState = State.None;
            switch (state) 
            {
                case State.Idle:
                    Idle();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Wander:
                    Wander();
                    break;
                
            }
        }

       
        //IdleMove();
    }
    
    private void Idle()
    {
        animator.SetTrigger("idle");
    }

    private void Attack() //현재 공격은 애니메이션만 작동
    {
        animator.SetTrigger("attack");

        if (IsPlayerInRange())
        {
            // 플레이어가 범위 내에 있으면 공격 사운드 재생
            enemyAudioSource.PlayOneShot(attackSound);
        }

    }

    private void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        animator.SetTrigger("walk");
        
        transform.Rotate((randomDir * transform.up).normalized * Time.deltaTime * rotateSpeed);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        randomDir = Random.Range(-360, 360);
        chooseDir = false;
    }

    private bool IsPlayerInRange()
    {
        return Physics.CheckSphere(transform.position, attackRange, 1 << 6, QueryTriggerInteraction.Ignore);
    }

    
    public void TakeDamage (float amount) 
    {
        CurHealth -= amount;

       healthBar.UpdateHealthBar(MaxHealth, CurHealth);

        if (CurHealth <= 0.0f) 
        {
            Die();
            GameManager.instance.killEnemy++;
        }
    }

    private void Die()
    {
        // 사망 시 사운드 재생
        enemyAudioSource.PlayOneShot(deathSound);


        Destroy(gameObject);
    }
    
    //private void IdleMove()
    //{
    //    Vector3 vec = CurPos;
    //    vec.x += delta * Mathf.Sin(Time.time * speed);

    //    transform.position = vec;
    //    transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    //}


    public void InstantiateFx() //Unity Animation Event 에서 실행
    {
        Instantiate(splashFx, transform.position, Quaternion.identity);
    }
    
    public void WhenAnimationDone() //Unity Animation Event 에서 실행
    {
        attackDone = true;
    }


    private void OnDrawGizmosSelected()
    {
        //Gizmos를 사용하여 공격 범위를 Scene View에서 확인할 수 있게 (인게임에서는 볼 수 없음)
        //해당 함수는 없어도 기능 상의 문제는 없지만, 기능 체크 및 디버깅을 용이
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
