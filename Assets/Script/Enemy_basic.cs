using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public int HP;

    public float WalkSpeed;
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
    public Transform WallChecker;

    public Vector2 RayOffset = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }
        CheckWall(Vector2.right);
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
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRange, groundLayer);
        if (IsWalkingLeft == true)
        {
            Debug.DrawRay(transform.position, -Vector2.up * groundCheckRange, Color.red);
            if (isGrounded)
            {
                //Walk Left, Check hole on left
                //EnemySprite.flipX = false;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * -WalkSpeed, ForceMode2D.Force);
            }
            else
            {
                IsWalkingLeft = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, -Vector2.up * groundCheckRange, Color.red);
            if (isGrounded)
            {
                //Walk Right, Check hole on right
                //EnemySprite.flipX = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed , ForceMode2D.Force);
            }
            else
            {
                IsWalkingLeft = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    void CheckWall(Vector2 direction)
    {
        if (IsWalkingLeft == true)
        {
            isWalled = Physics2D.OverlapCircle(WallChecker.position, wallCheckRange, wallLayer);
        } 
        else if (IsWalkingLeft == false)
        {
            isWalled = Physics2D.OverlapCircle(WallChecker.position, wallCheckRange, wallLayer);
        }

        if (isWalled)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StartCoroutine("Wait_Before_Turning", 2f);
        }
    }

    IEnumerator Wait_Before_Turning()
    {
        yield return new WaitForSeconds(2f);
        if (IsWalkingLeft)
        {
            IsWalkingLeft = false;
        }
        else
        {
            IsWalkingLeft = true;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        if (IsWalkingLeft == false)
        { 
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
            Gizmos.DrawWireSphere(WallChecker.position, wallCheckRange);
        }

        if (IsWalkingLeft == true)
        { 
            Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);
            Gizmos.DrawWireSphere(WallChecker.position, wallCheckRange);
        }
    }
}
