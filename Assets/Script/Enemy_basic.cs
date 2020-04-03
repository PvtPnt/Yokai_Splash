using System.Collections;
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

    public Transform EnemyHitbox;
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    public Vector3 RayOffset = new Vector3(1f, 0f, 0f);

    public SpriteRenderer TsuchinokoSprite;
    public Animator TsuchinokoAnimator;

    public bool isPlayerinRange = false;
    public float stateDelay;

    // Start is called before the first frame update
    void Start()
    {
        TsuchinokoSprite = GetComponent<SpriteRenderer>();
        TsuchinokoAnimator = GetComponent<Animator>();
        if (isBigChungus)
        {groundCheckRange += 2f;}
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWalkingLeft) { EnemyHitbox = WallChecker_Left; }
        else { EnemyHitbox = WallChecker_Right; }
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

    public void AttackCall(string Call)
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
        
        Vector3 pushedDir = new Vector3(1, 0, 0);
        if (WaveDirectionLeft == true)
        {
            Debug.Log("Pushback hit Left");
            GetComponent<Rigidbody2D>().AddForce(-pushedDir * 100);
        }

        else
        {
            Debug.Log("Pushback hit Right");
            GetComponent<Rigidbody2D>().AddForce(pushedDir * 100);
        }

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
            TsuchinokoAnimator.SetBool("moving", true);
            if (IsWalkingLeft == true)
            {StartCoroutine("Tsuchinoko_MoveLeft", 0.45f);}
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
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if (IsWalkingLeft) { IsWalkingLeft = false; }
            else { IsWalkingLeft = true; };
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

        if (isBigChungus == false) { isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer); }
        else { isWalled = Physics2D.OverlapCircle(Current_WallChecker.position - new Vector3(0f,1f,0f), wallCheckRange, wallLayer); }

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerinRange = true;
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
