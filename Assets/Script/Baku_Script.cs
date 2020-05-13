using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baku_Script : MonoBehaviour
{
    public GameObject Prince;
    public int ActionMin;
    public int ActionMax;

    public int Damage;

    public bool isAttacking;
    public bool isDead;

    public float AttackRange;
    public float WalkStepLength;
    public float CooldownTimer;
    public float ChargeStepLength;
    public float SuckTimer;
    public float SuckTime;

    public Transform AttackStartPosition;
    public Transform AttackEndPosition;
    public Transform Beam_Position;
    public CircleCollider2D StompHitbox;
    public LayerMask playerLayer;
    public GameObject DreamBeam;
    public GameObject bossDialogue;
    public GameObject bossDialogue2;

    float DeathTimer = 3f;
    bool isSucking;
    bool isPerformingAction;
    int ActionIndex;
    SpriteRenderer BakuSprite;
    Animator BakuAnim;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        BakuSprite = GetComponent<SpriteRenderer>();
        BakuAnim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        bossDialogue.SetActive(false);
        bossDialogue2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            DeathTimer -= Time.deltaTime;
            StopAllCoroutines();
            WalkStepLength = 0;
            ChargeStepLength = 0;
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.2328949f, 0.3661842f);
            GetComponent<BoxCollider2D>().size = new Vector2(6.80365f, 2.760612f);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.layer = 11;
            Prince.SetActive(true);
            if (DeathTimer <= 0) { Destroy(this.gameObject); }
            bossDialogue.SetActive(true);
            bossDialogue2.SetActive(true);
            //return;
        }

        if (isAttacking == true)
        {
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(transform.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }

        if (isSucking)
        {
            SuckTimer += Time.deltaTime;

            if (SuckTimer >= SuckTime)
            {
                BakuAnim.SetBool("Sucking", false);
                StartCoroutine("ActionCooldown");
            }

            else
            {
                Player.GetComponent<Rigidbody2D>().AddForce
                (Vector3.right * Player.GetComponent<Player_cube_control>().WalkSpeed * 0.4f, ForceMode2D.Force);
            }
        }
    }

    void FixedUpdate()
    {
        if (isPerformingAction == false) { StartCoroutine("ActionManager"); }
    }

    public void ChargeHitbox_On()
    { isAttacking = true; }

    public void ChargeHitbox_Off()
    { isAttacking = false; }

    IEnumerator ActionManager()
    {
        isPerformingAction = true;
        ActionIndex = Random.Range(ActionMin, ActionMax); //Set ActionIndex value to random number between (a,b)} 
        Debug.Log("Action index is " + ActionIndex);
        InvokeRepeating("GoToAttackStart", 0.5f, 1f);
        yield return null;
    }

    IEnumerator ActionCooldown()
    {
        isSucking = false;
        SuckTimer = 0;
        BakuAnim.SetTrigger("Idle");
        BakuSprite.flipX = false;
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
    }

    void GoToAttackStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), WalkStepLength);
        if (transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 1)      { BakuAnim.SetTrigger("Suck_prep"); BakuAnim.SetBool("Sucking", true); }
            else if (ActionIndex == 2) { BakuAnim.SetTrigger("Charge_prep"); BakuAnim.SetBool("Charging", true); }
            else if (ActionIndex == 3) { BakuAnim.SetTrigger("Stomp"); }
            else if (ActionIndex == 4) { BakuAnim.SetTrigger("Beam_Prep");  }
            else if (ActionIndex == 5) { BakuAnim.SetTrigger("Delayed_Beam_Prep"); }
        }
    }

    public void Suck()
    { isSucking = true; }


    void Charge()
    {InvokeRepeating("ChargeControl", 0.5f, 0.25f);}

    void ChargeControl()
    {
        if (transform.position.x != AttackEndPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackEndPosition.position.x, transform.position.y, 0), ChargeStepLength); }
        else
        {
            CancelInvoke("ChargeControl");
            InvokeRepeating("ChargeBack", 1f, 0.25f);
        }
    }

    void ChargeBack()
    {
        BakuSprite.flipX = true;
        if (transform.position.x != AttackStartPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), ChargeStepLength); }
        else
        {
            CancelInvoke("ChargeBack");
            BakuAnim.SetBool("Charging", false);
            StartCoroutine("ActionCooldown");
        }
    }
    
    public void Stomp_Hitbox_On()
    {StompHitbox.enabled = true;}

    public void Stomp_Hitbox_Off()
    {StompHitbox.enabled = false;
     StartCoroutine("ActionCooldown");
    }

    public void Beam()
    {GameObject NewBeam = Instantiate(DreamBeam, Beam_Position.position, Quaternion.identity);}


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    //public void DeathDialogue()
    //{
    //    bossDialogue.SetActive(true);
    //}
}
