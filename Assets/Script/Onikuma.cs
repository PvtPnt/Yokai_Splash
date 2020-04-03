using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : MonoBehaviour
{
    public float WalkSpeed;
    public float RushSpeed;
    public float CooldownTimer;

    public float WalkStepLength;
    public float RushStepLength;
    public float ClawStepLength;
    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;
    public float AttackRange;
    public float Col_sizeX, Col_sizeY = 1f;
    public float Col_OffsetX, Col_OffsetY;
    public float HighThrow_offset;
    public float LowThrow_offset;

    [HideInInspector] public BoxCollider2D BCollider2D;

    public int Damage;
    public int ActionIndex;
    public int ActionMin;
    public int ActionMax;
    public int WalkStepCount;

    public bool IsWalkingLeft;
    public bool isGrounded;
    public bool isWalled;
    public bool isPerformingAction;
    [HideInInspector] public bool RushIsReady;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    public Transform groundChecker;
    public Transform AttackStartPosition;
    public Transform AttackEndPosition;

    public Animator OnikumaAnimator;
    SpriteRenderer OnikumaSprite;
    public Sprite Standing;
    public Sprite Prowling;
    public GameObject Throwables;

    public Vector3 PlayerPosition;
    private float PlayerPosition_X;
    // Start is called before the first frame update
    void Start()
    {
        BCollider2D = GetComponent<BoxCollider2D>();
        OnikumaSprite = GetComponent<SpriteRenderer>();
        OnikumaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        BCollider2D.size = new Vector3(Col_sizeX, Col_sizeY, 0);
        BCollider2D.offset = new Vector3(Col_OffsetX, Col_OffsetY, 0);

            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
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
        OnikumaAnimator.SetTrigger("Idle");
        OnikumaSprite.flipX = false;
        isPerformingAction = false;
        WalkStepCount = 0;
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
        isPerformingAction = true;
        ActionIndex = Random.Range(ActionMin, ActionMax); //Set ActionIndex value to random number between (a,b)} 
        Debug.Log("Action index is " + ActionIndex);
        if      (ActionIndex <= 2)  { InvokeRepeating("Walking",0.5f,1f); }
        else    { InvokeRepeating("GoToAttackStart", 0.5f, 1f); }
        yield return null;
    }

    void Walking()
    {
        WalkStepCount += 1;
        if (WalkStepCount <= 5)
        {
            Debug.Log("Walking");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerPosition.x, transform.position.y, 0), WalkStepLength);
        }
        else
        {
            CancelInvoke("Walking");
            StartCoroutine("ActionCooldown");
        }
    }

    void GoToAttackStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), WalkStepLength);
        if ( transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 3) { RushPrep(); }
            else if (ActionIndex == 4) { SpinPrep(); }
            else if (ActionIndex == 5) { ThrowHigh(); }
            else if (ActionIndex == 6) { ThrowLow(); }
        }
    }

    void RushPrep()
    {
        Debug.Log("Preparing Rush");
        OnikumaAnimator.SetTrigger("PrepCharge"); 
    }

    void RushAttack()
    {
        Debug.Log("Rush");
        Col_OffsetX = 0.175f;
        Col_OffsetY = -0.65f;
        Col_sizeX = 5.202f;
        Col_sizeY = 2.5f;
        OnikumaAnimator.SetTrigger("Charge");
        InvokeRepeating("RushControl", 0.5f, 0.25f); 
    }

    void RushControl()
    {
        if (transform.position.x != AttackEndPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackEndPosition.position.x, transform.position.y, 0), RushStepLength); }
        else
        { 
            CancelInvoke("RushControl");
            InvokeRepeating("RushBack", 1f, 0.25f);
        }
    }

    void RushBack()
    {
        OnikumaSprite.flipX = true;
        if (transform.position.x != AttackStartPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), RushStepLength); }
        else
        {
            CancelInvoke("RushBack");
            StartCoroutine("ActionCooldown");
        }
    }

    void SpinPrep()
    {
        Debug.Log("Preparing Spin");
        OnikumaAnimator.SetTrigger("PrepSpin");
    }

    void SpinAttack()
    {
        Debug.Log("Execute Spin");
        AttackRange = 3f;
        Col_OffsetX = -0.017f;
        Col_OffsetY = 0.0597f;
        Col_sizeX = 4.82f;
        Col_sizeY = 7.58f;
        OnikumaAnimator.SetBool("Spin", true);
        InvokeRepeating("SpinControl", 0.5f, 0.25f);
    }

    void SpinControl()
    {
        if (transform.position.x != AttackEndPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackEndPosition.position.x, transform.position.y, 0), ClawStepLength); }
        else
        {
            CancelInvoke("SpinControl");
            InvokeRepeating("SpinBack", 1f, 0.25f);
        }
    }

    void SpinBack()
    {
        if (transform.position.x != AttackStartPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), RushStepLength); }
        else
        {
            CancelInvoke("SpinBack");
            OnikumaAnimator.SetBool("Spin", false);
            StartCoroutine("ActionCooldown");
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
