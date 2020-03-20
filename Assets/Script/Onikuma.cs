using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : MonoBehaviour
{
    public float WalkSpeed;
    public float RushSpeed;
    public float CooldownTimer;

    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;
    public float Col_sizeX, Col_sizeY = 1f;
    public float Col_OffsetX, Col_OffsetY;
    public float HighThrow_offset;
    public float LowThrow_offset;

    public BoxCollider2D BCollider2D;

    public int Damage;
    public int ActionIndex;

    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;
    public bool isPerformingAction;
    public bool isVulnerable;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    public Transform groundChecker;

    SpriteRenderer OnikumaSprite;
    public Sprite Standing;
    public Sprite Prowling;
    public GameObject Throwables;

    public Vector2 PlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        BCollider2D = GetComponent<BoxCollider2D>();
        OnikumaSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    { 
        BCollider2D.size = new Vector3(Col_sizeX, Col_sizeY, 0);
        BCollider2D.offset = new Vector3(Col_OffsetX, Col_OffsetY, 0);

            if (isVulnerable == false)
        {
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }
    }

    void FixedUpdate()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;
        if (isPerformingAction == false) { StartCoroutine("ActionManager"); }
    }

    IEnumerator ActionCooldown()
    {
        StartCoroutine("ReturnCollider");
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
        Col_OffsetX = -0.017f;
        Col_OffsetY = 0.0597f;
        Col_sizeX = 4.82f;
        Col_sizeY = 7.58f;
    }

    IEnumerator ReturnCollider()
    {
        yield return new WaitForSeconds(1f);
        AttackRange = 3.5f;
        yield return null;
    }

    IEnumerator ActionManager()
    {
        OnikumaSprite.sprite = Standing;
        isPerformingAction = true;
        ActionIndex = Random.Range(2, 5); //Set ActionIndex value to random number between (a,b)} 
        Debug.Log("Action index is " + ActionIndex);
        if      (ActionIndex <= 2)  { Walking(); }
        else if (ActionIndex == 3)  { RushAttack(); }
        else if (ActionIndex == 4)  { Claw(); }
        else if (ActionIndex == 5)  { SlipRush(); }
        else if (ActionIndex == 6)  { ThrowHigh(); }
        else if (ActionIndex == 7)  { ThrowLow(); }

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
        OnikumaSprite.sprite = Prowling;
        Col_OffsetX = -0.016f;
        Col_OffsetY = 0.369f;
        Col_sizeX = 4.82f;
        Col_sizeY = 2.46f;
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;

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

    void SlipRush()
    {
        OnikumaSprite.sprite = Prowling;
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        Col_OffsetX = -0.016f;
        Col_OffsetY = 0.369f;
        Col_sizeX = 4.82f;
        Col_sizeY = 2.46f;

        if (transform.position.x < PlayerPosition.x) //Player is to the right
        {
            Debug.Log("Execute SlipRush Right");
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * RushSpeed, ForceMode2D.Impulse);
            StartCoroutine("Slip", -1);
        }
        else if (transform.position.x > PlayerPosition.x) //Player is to the left
        {
            Debug.Log("Execute SlipRush Left");
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * RushSpeed, ForceMode2D.Impulse);
            StartCoroutine("Slip", 1);
        }
    }

    void Claw()
    {
        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        AttackRange = 3f;
        Col_OffsetX = -0.017f;
        Col_OffsetY = 0.0597f;
        Col_sizeX = 4.82f;
        Col_sizeY = 7.58f;
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

    void ThrowHigh()
    {
        Debug.Log("Execute High Throw");
        GameObject Throwabless =
        Instantiate(Throwables, transform.position + new Vector3(0, HighThrow_offset,0) , Quaternion.identity);

        Vector2 PlayerPosition = GameObject.Find("Player").transform.position;
        
        if (transform.position.x < PlayerPosition.x)
        { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = false; }
        else { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = true; }
        StartCoroutine("ActionCooldown");
    }

    void ThrowLow()
    {
        Debug.Log("Execute Low Throw");
        GameObject Throwabless =
        Instantiate(Throwables, transform.position + new Vector3(0, LowThrow_offset, 0), Quaternion.identity);
        
        if (transform.position.x < PlayerPosition.x)
        { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = false; }
        else { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = true; }
        StartCoroutine("ActionCooldown");
    }

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

    IEnumerator Slip(int direction)
    {
        Debug.Log("Slipping");
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * RushSpeed *0.65f, ForceMode2D.Impulse);
        isVulnerable = true;    //Disable attack collider
        yield return new WaitForSeconds(3.5f); //Vulnerable time
        isVulnerable = false;
        StartCoroutine("ActionCooldown");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRange);

        Gizmos.color = Color.blue;
        if (IsWalkingLeft == false)
        { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange); }

        if (IsWalkingLeft == true)
        { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange); }
    }
}
