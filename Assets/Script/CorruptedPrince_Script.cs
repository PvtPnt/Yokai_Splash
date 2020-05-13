using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CorruptedPrince_Script : MonoBehaviour
{
    [HideInInspector] public bool isDead;
    public int AttackCount;
    public int ActionIndex;
    public int ActionMin;
    public int ActionMax;

    public float WalkStepLength;
    public float ShockWave_PushStrength;
    public float CooldownTimer;
    public float Weakened_Time;

    public bool isWeakened;
    public bool isAttacking;
    bool isRoaring;


    public Vector3 Tsuchinoko_SpawnPosition_L;
    public Vector3 Tsuchinoko_SpawnPosition_R;
    public Vector3 Spike_Position;
    public Transform AttackStartPosition;
    public Transform LaserPosition;
    public LayerMask playerLayer;

    int Tsuchinoko_SummonCount;
    bool isPerformingAction;
    SpriteRenderer PrinceRenderer;
    Animator PrinceAnim;
    GameObject Player;
    Enemy_hp HP_Script;
    public GameObject Barrier;
    public GameObject Laser;
    public GameObject Tsuchinoko;
    public GameObject Spike_PatternA;
    public GameObject Spike_PatternB;
    public GameObject Spike_PatternC;
    public GameObject bossDialogue;
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

        if (isDead)
        {
            StopAllCoroutines();
            bossDialogue.SetActive(true);
        }

        if (isWeakened) { HP_Script.Defense = 0; }
        else { HP_Script.Defense = 100; }

        if (isRoaring) { Player.GetComponent<Rigidbody2D>().AddForce(Vector3.left * ShockWave_PushStrength, ForceMode2D.Force); }
    }

    void FixedUpdate()
    {if (isPerformingAction == false) { StartCoroutine("ActionManager"); }}

    public void AttackOn()
    { isAttacking = true; }

    public void AttackOff()
    { isAttacking = false; }

    IEnumerator Become_Weakened()
    {
        Barrier.SetActive(false);
        isWeakened = true;
        yield return new WaitForSeconds(Weakened_Time);
        Barrier.SetActive(true);
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
            if (ActionIndex == 1)       { PrinceAnim.SetTrigger("SummonTsuchinoko"); PrinceAnim.SetBool("Snapping", true);  }
            else if (ActionIndex == 2)  { PrinceAnim.SetTrigger("SummonSpike"); }
            else if (ActionIndex == 3)  { PrinceAnim.SetTrigger("ShootLaser");}
            else if (ActionIndex == 4)  { PrinceAnim.SetTrigger("Roar"); }
        }
    }

    IEnumerator ActionCooldown()
    {
        Tsuchinoko_SummonCount = 0;
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        isPerformingAction = false;
    }

    public void SummonTsuchinoko()
    {
        Tsuchinoko_SummonCount += 1;
        if (Tsuchinoko_SummonCount <= 4)
        {
            if ( Tsuchinoko_SummonCount % 2 == 0) 
            { 
                GameObject Summoned = Instantiate(Tsuchinoko, Tsuchinoko_SpawnPosition_L, Quaternion.identity);
                Summoned.GetComponent<SummonedTsuchinoko>().GoRight = true;
            }
            else
            {
                GameObject Summoned = Instantiate(Tsuchinoko, Tsuchinoko_SpawnPosition_R, Quaternion.identity);
                Summoned.GetComponent<SummonedTsuchinoko>().GoLeft = true;
            }
        }

        else 
        {
            PrinceAnim.SetBool("Snapping", false);
            AttackCount += 1;
            StartCoroutine("ActionCooldown");
        }
    }

    public void SummonSpike()
    {
        int SpikePattern = Random.Range(1, 4);
        if (SpikePattern == 1) { Instantiate(Spike_PatternA, Spike_Position, Quaternion.identity); }
        else if (SpikePattern == 2) { Instantiate(Spike_PatternB, Spike_Position, Quaternion.identity); }
        else if (SpikePattern == 3) { Instantiate(Spike_PatternC, Spike_Position, Quaternion.identity); }
        AttackCount += 1;
        StartCoroutine("ActionCooldown");
    }

    public void ReadyToShootLaser()
    { PrinceAnim.SetBool("ShootingLaser", true); }
    public void ShootLaser()
    {
        GameObject NewBeam = Instantiate(Laser, LaserPosition.position, Quaternion.identity);
    }


    public void RoarShockWave()
    {isRoaring = true;}

    public void StopRoar()
    {
        isRoaring = false;
        AttackCount += 1;
        StartCoroutine("ActionCooldown"); 
    }
}
