using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : MonoBehaviour
{
    //public int HP;

    public float WalkSpeed;
    public float RushSpeed;
    public float CooldownTimer;

    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;
    public float Col_sizeX, Col_sizeY = 1f;
    public float Col_OffsetX, Col_OffsetY;

    public BoxCollider2D BCollider2D;

    public int Damage;
    public int ActionIndex;

    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;
    public bool isPerformingAction;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    public Transform groundChecker;
    public Transform Current_WallChecker;
    public Transform WallChecker_Right;
    public Transform WallChecker_Left;

    public Transform RoomEnd_L;
    public Transform RoomEnd_R;

    public SpriteRenderer OnikumaSprite;

    public Vector2 PlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        BCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    { 
        //if (HP <= 0) { Destroy(gameObject); }
        BCollider2D.size = new Vector3(Col_sizeX, Col_sizeY, 0);
        BCollider2D.offset = new Vector3(Col_OffsetX, Col_OffsetY, 0);
    }

    void FixedUpdate()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;
        if (isPerformingAction == false) { StartCoroutine("ActionManager"); }
        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
        for (int i = 0; i < DamagePlayer.Length; i++)
        { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }

    }

    //public void ReceiveDamage(int Damage)
    //{HP -= Damage;}

    IEnumerator ActionCooldown()
    {
        StartCoroutine("ReturnCollider");
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
    }

    IEnumerator ReturnCollider()
    {
        yield return new WaitForSeconds(1f);
        AttackRange = 1f;
        Col_sizeX = 1f;
        Col_sizeY = 1f;
        yield return null;
    }

    IEnumerator ActionManager()
    {
        isPerformingAction = true;
        ActionIndex = Random.Range(1, 4); //Set ActionIndex value to random number between (a,b)} 
        Debug.Log("Action index is " + ActionIndex);
        if      (ActionIndex <= 2)  { Walking(); }
        else if (ActionIndex == 3)  { RushAttack(); }
        else if (ActionIndex == 4)  { Claw(); }
        else if (ActionIndex == 5)  { SlipRush(); }
        else if (ActionIndex == 6)  { Throw(); }

        //yield return StartCoroutine("ActionCooldown");
        yield return null;
    }

    void Walking()
    {
        Debug.Log("Walking");
        if (transform.position.x < PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveRight", 3.0f); }
        else if (transform.position.x > PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveLeft", 3.0f); }
    }

    void RushAttack()
    {
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        Col_sizeX = 2f;
        Col_sizeY = 0.5f;

        if (transform.position.x < PlayerPosition.x) //Player is to the right
        {
            Debug.Log("Execute RushAttack Right");
            RoomEndAttack_Right();
        }
        else if (transform.position.x > PlayerPosition.x) //Player is to the left
        {
            Debug.Log("Execute RushAttack Left");
            RoomEndAttack_Left();
        }
    }

    void Claw()
    {
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        AttackRange = 2f;
        Col_sizeX = 0.5f;
        Col_sizeY = 2f;
        Debug.Log("Execute Claw");

        if (transform.position.x < PlayerPosition.x) //Player is to the right
        {
            Debug.Log("Execute ClawAttack Right");
            RoomEndAttack_Right();
        }
        else if (transform.position.x > PlayerPosition.x) //Player is to the left
        {
            Debug.Log("Execute ClawAttack Left");
            RoomEndAttack_Left();
        }
    }

    void SlipRush()
    {
        Debug.Log("Execute SlipRush");
        StartCoroutine("ActionCooldown");
    }

    void Throw()
    {
        Debug.Log("Execute Throw");
        StartCoroutine("ActionCooldown");
    }


    //void CheckWall()
    //{
    //    if (IsWalkingLeft == true)
    //    {
    //        Current_WallChecker = WallChecker_Left;
    //        OnikumaSprite.flipX = false;
    //    }
    //    else if (IsWalkingLeft == false)
    //    {
    //        Current_WallChecker = WallChecker_Right;
    //        OnikumaSprite.flipX = true;
    //    }

    //    isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer);

    //    if (isWalled == true)
    //    {
    //        if (IsWalkingLeft == true)
    //        { IsWalkingLeft = false; }
    //        else if (IsWalkingLeft == false)
    //        { IsWalkingLeft = true; }
    //    }
    //}

    IEnumerator Onikuma_MoveLeft(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed, ForceMode2D.Force);
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine("ActionCooldown");
    }

    IEnumerator Onikuma_MoveRight(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed, ForceMode2D.Force);
        yield return new WaitForSeconds(WaitTime);
        yield return StartCoroutine("ActionCooldown");
    }

    void RoomEndAttack_Left()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * RushSpeed, ForceMode2D.Impulse);
        StartCoroutine("RushBack", 1);
    }

    void RoomEndAttack_Right()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * RushSpeed, ForceMode2D.Impulse);
        StartCoroutine("RushBack", -1);
    }

    IEnumerator RushBack(int direction)
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right *direction * RushSpeed, ForceMode2D.Impulse);
        StartCoroutine("ActionCooldown");
    }



    //private void OnDrawGizmosSelected()
    //{
    //    Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(groundChecker.position, groundCheckRange);

    //    Gizmos.color = Color.blue;
    //    if (IsWalkingLeft == false)
    //    { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}

    //    if (IsWalkingLeft == true)
    //    {Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange);}
    //}
}
