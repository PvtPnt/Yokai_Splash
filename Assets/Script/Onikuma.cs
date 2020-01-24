using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : MonoBehaviour
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
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    public SpriteRenderer OnikumaSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { if (HP <= 0) { Destroy(gameObject); } }

    void FixedUpdate()
    {
        Collider2D DetectPlayer = Physics2D.OverlapCircle(EnemyHitbox.position, AttackRange, playerLayer);
        Walking();
    }
    void Walking()
    {
        CheckWall();
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRange, groundLayer);
        if (isGrounded)
        {
            if (IsWalkingLeft == true)
            { StartCoroutine("Onikuma_MoveLeft", 0.45f); }
            else if (IsWalkingLeft == false)
            { StartCoroutine("Onikuma_MoveRight", 0.45f); }
        }
    }

    void CheckWall()
    {
        if (IsWalkingLeft == true)
        {
            Current_WallChecker = WallChecker_Left;
            OnikumaSprite.flipX = false;
        }
        else if (IsWalkingLeft == false)
        {
            Current_WallChecker = WallChecker_Right;
            OnikumaSprite.flipX = true;
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

    IEnumerator Onikuma_MoveLeft(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        yield return new WaitForSecondsRealtime(WaitTime);
    }

    IEnumerator Onikuma_MoveRight(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        yield return new WaitForSecondsRealtime(WaitTime);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);
        if (IsWalkingLeft == false)
        { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}

        if (IsWalkingLeft == true)
        {Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}
    }
}
