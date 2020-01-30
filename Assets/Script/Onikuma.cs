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
    public Transform groundChecker;
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
        //Walking();
        Vector3 PlayerPosition = GameObject.Find("Player").transform.position;

        if (transform.position.x < PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveRight", 3.0f); }
        else if (transform.position.x > PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveLeft", 3.0f); }
        
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, groundCheckRange, groundLayer);

        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
        for (int i = 0; i < DamagePlayer.Length; i++)
        { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
    }
    void Walking()
    {
        CheckWall();
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, groundCheckRange, groundLayer);
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
        yield return new WaitForSecondsRealtime(WaitTime);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
    }

    IEnumerator Onikuma_MoveRight(float WaitTime)
    {
        yield return new WaitForSecondsRealtime(WaitTime);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRange);

        Gizmos.color = Color.blue;
        if (IsWalkingLeft == false)
        { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}

        if (IsWalkingLeft == true)
        {Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}
    }
}
