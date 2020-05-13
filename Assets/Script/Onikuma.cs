using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onikuma : MonoBehaviour
{
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

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isThrowing;
    [HideInInspector] public bool isThrowHigh;
    [HideInInspector] public bool isThrowLow;
    [HideInInspector] public bool IsWalkingLeft;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isWalled;
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
    public bool isDead;

    public GameObject bossDialogue;
    // Start is called before the first frame update
    void Start()
    {
        BCollider2D = GetComponent<BoxCollider2D>();
        OnikumaSprite = GetComponent<SpriteRenderer>();
        OnikumaAnimator = GetComponent<Animator>();

        bossDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BCollider2D.size = new Vector3(Col_sizeX, Col_sizeY, 0);
        BCollider2D.offset = new Vector3(Col_OffsetX, Col_OffsetY, 0);

        if (isDead)
        { 
            StopAllCoroutines();
            WalkStepLength = 0;
            RushStepLength = 0;
            Col_OffsetX = -0.1569824f;
            Col_OffsetY = 0.5503812f;
            Col_sizeX = 7.527222f;
            Col_sizeY = 3.73073f;
            bossDialogue.SetActive(true);
            return;
        }

        if (isAttacking == true)
        {
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(EnemyHitbox.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }

        if (isThrowing && isThrowHigh)
        {
            GameObject Throwabless = Instantiate(Throwables, transform.position + new Vector3(-1f, HighThrow_offset, 0), Quaternion.identity);
            Vector2 PlayerPosition = GameObject.Find("Player").transform.position;

            if (transform.position.x < PlayerPosition.x)
            { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = false; }
            else { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = true; }
            isThrowHigh = false;
        }

        if (isThrowing && isThrowLow)
        {
            GameObject Throwabless = Instantiate(Throwables, transform.position + new Vector3(-0.5f, LowThrow_offset, 0), Quaternion.identity);
            Vector2 PlayerPosition = GameObject.Find("Player").transform.position;

            if (transform.position.x < PlayerPosition.x)
            { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = false; }
            else { Throwables.GetComponent<OnikumaThrowables>().isMovingLeft = true; }
            isThrowLow = false;
        }
    }

    public void AttackOn()
    {isAttacking = true;}

    public void AttackOff()
    {isAttacking = false;}

    void FixedUpdate()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;
        if (isPerformingAction == false) { StartCoroutine("ActionManager"); }
    }

    IEnumerator ActionCooldown()
    {
        OnikumaAnimator.SetTrigger("Idle");
        OnikumaSprite.flipX = false;
        WalkStepCount = 0;
        Col_OffsetX = -0.017f;
        Col_OffsetY = 0.0597f;
        Col_sizeX = 4.82f;
        Col_sizeY = 7.58f;
        StartCoroutine("ReturnCollider");
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
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
        InvokeRepeating("GoToAttackStart", 0.5f, 1f);
        yield return null;
    }

    void GoToAttackStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), WalkStepLength);
        if ( transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 1) { RushPrep(); }
            else if (ActionIndex == 2) { SpinPrep(); }
            else if (ActionIndex == 3) { ThrowHigh(); }
            else if (ActionIndex == 4) { ThrowLow(); }
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
        OnikumaAnimator.SetBool("Charge",true);
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
            OnikumaAnimator.SetBool("Charge", false);
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

    void ThrowOn()
    { isThrowing = true; }

    void ThrowOff()
    { isThrowing = false; }

    void ThrowHigh()
    {
        OnikumaAnimator.SetTrigger("Throw");
        isThrowHigh = true;
        Debug.Log("Execute High Throw");
        StartCoroutine("ActionCooldown");
    }

    void ThrowLow()
    {
        OnikumaAnimator.SetTrigger("Throw");
        isThrowLow = true;
        Debug.Log("Execute Low Throw");
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

    //private void OnDestroy()
    //{
    //  print("Something");
    //   print("Something");
    //    bossDialogue.SetActive(true);
    //}
}
