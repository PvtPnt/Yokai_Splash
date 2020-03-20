﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    //public int HP;

    public float WalkSpeed;
    public float Tsuchinoko_Jumpforce;
    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;

    public int Damage;
    public float PushForce;
    public bool waveDirectionLeft;

    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;
    public bool onPush = false;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    public SpriteRenderer TsuchinokoSprite;

    public bool isPlayerinRange = false;
    public float stateDelay;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWalkingLeft) { EnemyHitbox = WallChecker_Left; }
        else { EnemyHitbox = WallChecker_Right; }
    }

    void FixedUpdate()
    {
        if (!isPlayerinRange)
        {
            Walking();
        }

        else
        {
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }
        
    }

    //public void ReceiveDamage(int Damage)
    //{
    //    HP -= Damage;
    //}

    public void PushedBack(bool WaveDirectionLeft)
    {
        
        Vector3 pushedDir = new Vector3(1, 0, 0);
        if (WaveDirectionLeft == true)
        {
            Debug.Log("Pushback hit Left");
            GetComponent<Rigidbody2D>().AddForce(-pushedDir * PushForce);
        }

        else
        {
            Debug.Log("Pushback hit Right");
            GetComponent<Rigidbody2D>().AddForce(pushedDir * PushForce);
        }

        onPush = true;
        StartCoroutine(endPush());
    }

    public void PushedBackWithEnemy()
    {
        onPush = true;
        StartCoroutine(endPush());
    }

    IEnumerator endPush()
    {
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

        isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer);

        if (isWalled == true)
        {
            if (IsWalkingLeft == true)
            { IsWalkingLeft = false; }
            else if (IsWalkingLeft == false)
            { IsWalkingLeft = true; }
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
        {Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}

        if (IsWalkingLeft == true)
        {Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerinRange = true;
        }
        if (other.gameObject.tag == "Enemy")
        {

            other.gameObject.SendMessage("PushedBackWithEnemy");
        }

    }
    void OnCollisionExit2D(Collision2D other)
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
