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
    [SerializeField] private int ActionIndex;

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

    }

    // Update is called once per frame
    void Update()
    { 
        if (HP <= 0) { Destroy(gameObject); }
    }

    void FixedUpdate()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;

        if(isPerformingAction == false) { StartCoroutine("ActionManager"); }
        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
        for (int i = 0; i < DamagePlayer.Length; i++)
        { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }

    }

    IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(5f);
    }

    IEnumerator ActionManager()
    {
        ActionIndex = Random.Range(1, 8); //Set ActionIndex value to random number between (a,b)} 
        //if (ActionIndex <= 4) { Walking(); }
        //else if (ActionIndex == 5) { RushAttack(); }
        //else if (ActionIndex == 6) { Claw(); }
        //else if (ActionIndex == 7) { SlipRush(); }
        //else if (ActionIndex == 8) { Throw(); }

        switch (ActionIndex)
        {
            case 1: 
                Walking();
                break;

            case 2:
                Walking();
                break;

            case 3:
                Walking();
                break;

            case 4:
                Walking();
                break;

            case 5:
                RushAttack();
                break;

            case 6:
                Claw();
                break;

            case 7:
                SlipRush();
                break;

            case 8:
                Throw();
                break;
        }
        yield return StartCoroutine("ActionCooldown");
    }

    void Walking()
    {
        if (transform.position.x < PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveRight", 3.0f); }
        else if (transform.position.x > PlayerPosition.x)
        { StartCoroutine("Onikuma_MoveLeft", 3.0f); }
    }

    void RushAttack()
    {
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        Debug.Log("Execute Claw");

        if (transform.position.x < PlayerPosition.x) //Player is to the right
        {
            InvokeRepeating("RoomEndAttack_Right", 0.5f, 0.25f);
        }
        else if (transform.position.x > PlayerPosition.x) //Player is to the left
        {
            InvokeRepeating("RoomEndAttack_Left", 0.5f, 0.25f); 
        }
    }

    IEnumerator Claw()
    { 
        Debug.Log("Execute Claw");
        yield return StartCoroutine("ActionCooldown");
    }

    IEnumerator SlipRush()
    { 
        Debug.Log("Excute Slip Rush");
        yield return StartCoroutine("ActionCooldown");
    }

    IEnumerator Throw()
    { 
        Debug.Log("Execute Throw");
        yield return StartCoroutine("ActionCooldown");
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
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        yield return StartCoroutine("ActionCooldown");
    }

    IEnumerator Onikuma_MoveRight(float WaitTime)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
        yield return StartCoroutine("ActionCooldown");
    }

    void RoomEndAttack_Left()
    {   if (transform.position.x > RoomEnd_L.position.x + 1)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force); }
        else if (transform.position.x < RoomEnd_R.position.x - 1)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force); }

        if (transform.position.x >= RoomEnd_R.position.x - 1)
        {
            CancelInvoke("RoomEndAttack_Left");
            Debug.Log("CancelInvoke RoomEndAttack_Left");
            return;
        }
    }

    void RoomEndAttack_Right()
    {
        if (transform.position.x < RoomEnd_R.position.x - 1)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force); }
        else if (transform.position.x > RoomEnd_L.position.x + 1)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force); }

        if (transform.position.x <= RoomEnd_L.position.x + 1)
        {
            CancelInvoke("RoomEndAttack_Right");
            Debug.Log("CancelInvoke RoomEndAttack_Right");
            return;
        }
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
