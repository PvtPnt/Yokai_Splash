using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedPrince_Script : MonoBehaviour
{
    public int ActionIndex;
    public int ActionMin;
    public int ActionMax;

    public float WalkStepLength;
    public float ShockWave_PushStrength;
    public float CooldownTimer;
    public float Weakened_Time;

    public bool isWeakened;
    public bool isAttacking;


    public Vector3 Tsuchinoko_SpawnPosition_L;
    public Vector3 Tsuchinoko_SpawnPosition_R;
    public Vector3 Spike_Position;
    public Transform AttackStartPosition;
    public LayerMask playerLayer;
    public Enemy_hp HP_Script;

    int Tsuchinoko_SummonCount;
    int AttackCount;
    bool isPerformingAction;
    SpriteRenderer PrinceRenderer;
    Animator PrinceAnim;
    GameObject Player;
    public GameObject Laser;
    public GameObject Tsuchinoko;
    public GameObject Spike_PatternA;
    public GameObject Spike_PatternB;
    public GameObject Spike_PatternC;
    // Start is called before the first frame update
    void Start()
    {
        HP_Script = GetComponent<Enemy_hp>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PrinceAnim = GetComponent<Animator>();
        PrinceRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP_Script.HP >= 30)
        {   if(AttackCount == 1) { StartCoroutine("Become_Weakened");}  }
        else if (HP_Script.HP < 30)
        { if (AttackCount == 2) { StartCoroutine("Become_Weakened"); } }

        if (isWeakened) { HP_Script.Defense = 0; }
        else { HP_Script.Defense = 100; }
    }

    void FixedUpdate()
    {if (isPerformingAction == false) { StartCoroutine("ActionManager"); }}

    public void AttackOn()
    { isAttacking = true; }

    public void AttackOff()
    { isAttacking = false; }

    IEnumerator Become_Weakened()
    {
        //isPerformingAction = true;
        isWeakened = true;
        yield return new WaitForSeconds(Weakened_Time);
        //isPerformingAction = false;
        isWeakened = false;
        AttackCount = 0;
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
        if (transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 1)       { PrinceAnim.SetTrigger("SummonTsuchinoko");  }
            else if (ActionIndex == 2)  { PrinceAnim.SetTrigger("SummonSpike"); }
            else if (ActionIndex == 3)  { ShootLaser(); }
            else if (ActionIndex == 4)  { PrinceAnim.SetTrigger("Shockwave"); }
        }
    }

    IEnumerator ActionCooldown()
    {
        //PrinceuAnim.SetTrigger("Idle");
        //PrinceSprite.flipX = false;
        //WalkStepCount = 0;
        //StartCoroutine("ReturnCollider");
        Tsuchinoko_SummonCount = 0;
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
    }

    void Summon()
    { InvokeRepeating("SummonTsuchinoko", 0.5f, 1.5f); }

    void SummonTsuchinoko()
    {
        Tsuchinoko_SummonCount += 1;
        if (Tsuchinoko_SummonCount <= 8)
        {
            if ( Tsuchinoko_SummonCount % 2 == 0) { Instantiate(Spike_PatternC, Tsuchinoko_SpawnPosition_L, Quaternion.identity); }
            else { Instantiate(Spike_PatternC, Tsuchinoko_SpawnPosition_R, Quaternion.identity); }
        }

        else 
        { 
            CancelInvoke("SummonTsuchinoko");
            StartCoroutine("ActionCooldown");
        }
    }

    public void SummonSpike()
    {
        int SpikePattern = Random.Range(0, 4);
        if (SpikePattern == 1) { Instantiate(Spike_PatternA, Spike_Position, Quaternion.identity); }
        else if (SpikePattern == 2) { Instantiate(Spike_PatternB, Spike_Position, Quaternion.identity); }
        else if (SpikePattern == 3) { Instantiate(Spike_PatternC, Spike_Position, Quaternion.identity); }
        StartCoroutine("ActionCooldown");
    }

    void ShootLaser()
    { 
        if (Player.transform.position.x < transform.position.x)
        {
            //Shoot laser left
            //PrinceRenderer.flipX = true;
            //PrinceAnim.SetTrigger("ShootLaser");
        }

        else if (Player.transform.position.x > transform.position.x)
        {
            //Shoot Laser right
            //PrinceRenderer.flipX = false;
            //PrinceAnim.SetTrigger("ShootLaser");
        }

    }

    public void LaserBeam()
    {

    }

    public void ShockWave()
    {
        if (Player.transform.position.x < transform.position.x)
        {
            Player.GetComponent<Rigidbody2D>().AddForce
            (Vector3.right * ShockWave_PushStrength, ForceMode2D.Impulse);
        }

        else if (Player.transform.position.x > transform.position.x)
        {
            Player.GetComponent<Rigidbody2D>().AddForce
            (Vector3.left * ShockWave_PushStrength, ForceMode2D.Impulse);
        }

        StartCoroutine("ActionCooldown");
    }
}
