﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public float WalkSpeed;
    public float Tsuchinoko_Jumpforce;
    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;

    public int Damage;
    public bool waveDirectionLeft;

    public bool isWalk;
    public bool isAttacking;
    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;
    public bool onPush = false;
    public bool NoGapAhead;
    public bool isBigChungus;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private BoxCollider2D DetectionBox;

    public Transform EnemyHitbox;
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    public Vector3 RayOffset = new Vector3(1f, 0f, 0f);

    public SpriteRenderer TsuchinokoSprite;
    Animator TsuchinokoAnimator;

    public bool isPlayerinRange = false;
    public float stateDelay;

    // Start is called before the first frame update
    void Start()
    {
        DetectionBox = GetComponent<BoxCollider2D>();
        TsuchinokoSprite = GetComponent<SpriteRenderer>();
        TsuchinokoAnimator = GetComponent<Animator>();
        if (isBigChungus)
        {groundCheckRange += 2f;}
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWalkingLeft)
        {
            EnemyHitbox = WallChecker_Left;
            DetectionBox.offset = new Vector2(-0.95f, 0.005f);
        }
        else
        {
            EnemyHitbox = WallChecker_Right;
            DetectionBox.offset = new Vector2(0.95f, 0.005f);
        }
        GapCheck();
    }

    void FixedUpdate()
    {
        if (!isPlayerinRange)
        {
            TsuchinokoAnimator.SetBool("attacking", false);
            Walking();
        }

        else
        {
            TsuchinokoAnimator.SetBool("moving", false);
            StartCoroutine("Attack");
        }

    }

    void PlayerCheck()
    {

    }

    IEnumerator Attack()
    {
        TsuchinokoAnimator.SetBool("attacking", true);
        if (isAttacking == true)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }
        yield return null;
    }


    public void PushedBack(bool WaveDirectionLeft)
    {
        onPush = true;

        Vector3 pushedDir = new Vector3(1, 0, 0);
        if (WaveDirectionLeft == true)
        {
            Debug.Log("Pushback hit Left");
            GetComponent<Rigidbody2D>().AddForce(-pushedDir * 200);
        }

        else
        {
            Debug.Log("Pushback hit Right");
            GetComponent<Rigidbody2D>().AddForce(pushedDir * 200);
        }

        StartCoroutine(endPush());
    }

    IEnumerator endPush()
    {
        onPush = true;

        yield return new WaitForSeconds(2.0f);
        onPush = false;
        Debug.Log("endpush");
    }

    void Walking()
    {
        CheckWall();
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRange, groundLayer);
        if (isGrounded)
        {
            if (IsWalkingLeft == true)
            {
                StartCoroutine("Tsuchinoko_MoveLeft", 0.45f);
            }
            else if (IsWalkingLeft == false)
            {StartCoroutine("Tsuchinoko_MoveRight", 0.45f);}
        }
    }

    void GapCheck()
    {
        if (IsWalkingLeft)  { NoGapAhead = Physics2D.OverlapCircle(-RayOffset + transform.position, groundCheckRange -0.5f, groundLayer); }
        else                { NoGapAhead = Physics2D.OverlapCircle(RayOffset + transform.position, groundCheckRange -0.5f, groundLayer); }
        if (NoGapAhead == false)
        {
            if (isGrounded == false)
            { this.enabled = false; }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                if (IsWalkingLeft) { IsWalkingLeft = false; }
                else { IsWalkingLeft = true; };
            }
        }
    }

    private void OnDisable()
    {
        TsuchinokoSprite.color = Color.red;
        Destroy(this.gameObject, 0.5f);
    }

    void CheckWall()
    {
        if (IsWalkingLeft == true)
        {
            Current_WallChecker = WallChecker_Left;
            TsuchinokoSprite.flipX = false;
        }
        else if (IsWalkingLeft == false)
        {
            Current_WallChecker = WallChecker_Right;
            TsuchinokoSprite.flipX = true;
        }

        if (isBigChungus == false) { isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer); }
        else { isWalled = Physics2D.OverlapCircle(Current_WallChecker.position - new Vector3(0f,1f,0f), wallCheckRange, wallLayer); }

        if (isWalled == true)
        {
            if (IsWalkingLeft == true)
            { 
                IsWalkingLeft = false;
                GetComponent<Rigidbody2D>().velocity =  new Vector2(0, 0);
            }
            else if (IsWalkingLeft == false)
            { 
                IsWalkingLeft = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }

    IEnumerator Tsuchinoko_MoveLeft(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * Tsuchinoko_Jumpforce, ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(WaitTime);
    }

    IEnumerator Tsuchinoko_MoveRight(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * Tsuchinoko_Jumpforce, ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(WaitTime);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);

        Gizmos.color = Color.red;
        if (IsWalkingLeft == false)
        {
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(RayOffset + transform.position, groundCheckRange -0.5f);
        }



        if (IsWalkingLeft == true)
        {
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(-RayOffset + transform.position, groundCheckRange -0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerinRange = true;
        }

        if(other.gameObject.tag == "Explosion")
        {
           int DamageToTake = other.gameObject.GetComponent<WaterSplash>().Damage;

            GetComponent<Enemy_hp>().HP = GetComponent<Enemy_hp>().HP - DamageToTake;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("DelayAttacktoIdle", stateDelay);
        }
    }

    void DelayAttacktoIdle()
    {
        isPlayerinRange = false;
    }
}
