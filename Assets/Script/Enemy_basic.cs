using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public int HP;

    public float WalkSpeed;
    public float Tsuchinoko_Jumpforce;
    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;

    public int Damage;

    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }
        Walking();

        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
        for (int i = 0; i < DamagePlayer.Length; i++)
        { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
    }

    public void ReceiveDamage(int Damage)
    {
        HP -= Damage;
        Debug.Log("Damage taken");
    }

    void Walking()
    {
        CheckWall();
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRange, groundLayer);
        if (isGrounded)
        {
            if (IsWalkingLeft == true)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed, ForceMode2D.Force);
                StartCoroutine("Tsuchinoko_Jump", 2f);
            }
            else if (IsWalkingLeft == false)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed, ForceMode2D.Force);
                StartCoroutine("Tsuchinoko_Jump", 2f);
            }
        }


    }

    void CheckWall()
    {
        if (IsWalkingLeft == true)
        {Current_WallChecker = WallChecker_Left;} 
        else if (IsWalkingLeft == false)
        {Current_WallChecker = WallChecker_Right;}

        isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer);

        if (isWalled == true)
        {
            if (IsWalkingLeft == true)
            { IsWalkingLeft = false; }
            else if (IsWalkingLeft == false)
            { IsWalkingLeft = true; }
        }
    }

    IEnumerator Tsuchinoko_Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * Tsuchinoko_Jumpforce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);
        if (IsWalkingLeft == false)
        { 
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
        }

        if (IsWalkingLeft == true)
        { 
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
        }
    }
}
