using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baku_Script : MonoBehaviour
{
    public int ActionMin;
    public int ActionMax;

    public int Damage;

    public bool isAttacking;

    public float AttackRange;
    public float WalkStepLength;
    public float CooldownTimer;
    public float ChargeStepLength;

    public Transform AttackStartPosition;
    public Transform AttackEndPosition;
    public LayerMask playerLayer;

    bool isPerformingAction;
    int ActionIndex;
    Animator BakuAnim;
    GameObject Player;
    GameObject DreamBeam;

    // Start is called before the first frame update
    void Start()
    {
        BakuAnim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == true)
        {
            Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(transform.position, AttackRange, playerLayer);
            for (int i = 0; i < DamagePlayer.Length; i++)
            { DamagePlayer[i].GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); }
        }
    }

    void FixedUpdate()
    {
        if (isPerformingAction == false) { StartCoroutine("ActionManager"); }
    }

    public void AttackOn()
    { isAttacking = true; }

    public void AttackOff()
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
        //BakuAnim.SetTrigger("Idle");
        //BakuSprite.flipX = false;
        //WalkStepCount = 0;
        //StartCoroutine("ReturnCollider");
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
    }

    IEnumerator ReturnCollider()
    {
        yield return new WaitForSeconds(1f);
        //AttackRange = 5f;
        yield return null;
    }

    void GoToAttackStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), WalkStepLength);
        if (transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 1)      { Suck();  }
            else if (ActionIndex == 2) {  }
            else if (ActionIndex == 3) {  }
            else if (ActionIndex == 4) { BeamPrep("Normal");  }
            else if (ActionIndex == 5) { BeamPrep("Delayed"); }
        }
    }

    void Suck()
    {
        float Timer = 5f;
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        { 
            StartCoroutine("ActionCooldown");
            return;
        }

        else
        {
            if (Player.transform.position.x < transform.position.x)
            {
                Player.GetComponent<Rigidbody2D>().AddForce
                (Vector3.right * Player.GetComponent<Player_cube_control>().WalkSpeed * Time.deltaTime, ForceMode2D.Force);
            }

            else if (Player.transform.position.x > transform.position.x)
            {
                Player.GetComponent<Rigidbody2D>().AddForce
                (Vector3.left * Player.GetComponent<Player_cube_control>().WalkSpeed * Time.deltaTime, ForceMode2D.Force);
            }
        }
    }

    void ChargePrep()
    {
        Debug.Log("Preparing Rush");
        //BakuAnim.SetTrigger("PrepCharge");
    }

    void Charge()
    {
        //BakuAnim.SetBool("Charge", true);
        InvokeRepeating("ChargeControl", 0.5f, 0.25f);
    }

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
        //OnikumaSprite.flipX = true;
        if (transform.position.x != AttackStartPosition.position.x)
        { transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), ChargeStepLength); }
        else
        {
            CancelInvoke("RushBack");
            //BakuAnim.SetBool("Charge", false);
            StartCoroutine("ActionCooldown");
        }
    }
    
    void Stomp()
    {
        //BakuAnim.SetTrigger("Stomp");
    }

    void BeamPrep(string variation)
    {
        if (variation == "Normal")
        {
            //BakuAnim.SetTrigger("BeamPrepNormal"); 
        }

        else if (variation == "Delayed")
        {
            //BakuAnim.SetTrigger("BeamPrepDelayed");
        }
    }

    void Beam()
    {
        GameObject NewBeam = Instantiate(DreamBeam, transform.position, Quaternion.identity);
    }

    void Beam_Delayed()
    {
        GameObject NewBeam = Instantiate(DreamBeam, transform.position, Quaternion.identity);
    }
}
