using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onyudo_Script : MonoBehaviour
{
    public int ActionMin;
    public int ActionMax;
    public int ClapCount;
    public int ClapCountMax;
    public float ClapInterval;
    public float RockDropPos_X;
    public float RockDropPos_Y;
    public int Damage;

    public bool isAttacking;

    public float AttackRange;
    public float WalkStepLength;
    public float CooldownTimer;

    public GameObject Rubble;
    public GameObject Rock;
    public GameObject Palm;
    public Transform AttackStartPosition;
    public Transform AttackEndPosition;
    public LayerMask playerLayer;

    bool isPerformingAction;
    int ActionIndex;
    Animator OnAnim;

    // Start is called before the first frame update
    void Start()
    {
        OnAnim = GetComponent<Animator>();
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
        //PlayerPosition = GameObject.Find("Player").transform.position;
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

    void GoToAttackStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(AttackStartPosition.position.x, transform.position.y, 0), WalkStepLength);
        if (transform.position.x == AttackStartPosition.position.x)
        {
            CancelInvoke("GoToAttackStart");
            if (ActionIndex == 1)       { TongueAura(); }
            else if (ActionIndex == 2)  { FirePalm(); }
            else if (ActionIndex == 3)  { InvokeRepeating("Clap", 0.5f, ClapInterval); }
            else if (ActionIndex == 4)  { TongueWhip(); }
        }
    }

    void TongueAura()
    { }

    void FirePalm()
    { 
        //OnAnim.SetTrigger("FirePalm");
    }

    public void SpawnFirePalm()
    {
        GameObject FlamingPalm = Instantiate(Palm, new Vector3(RockDropPos_X, RockDropPos_Y, transform.position.z), Quaternion.identity);
    }

        void Clap()
    {
        ClapCount += 1;
        if (ClapCount >= ClapCountMax)
        { StartCoroutine("ActionCooldown"); }

        else
        { 
            //OnAnim.SetTrigger("Clap");
        }
    }

    void Falling_Rubble()
    {
        RockDropPos_X = Random.Range(AttackStartPosition.position.x, AttackEndPosition.position.x);
        GameObject RubbleFall  = Instantiate(Rubble, new Vector3 (RockDropPos_X, RockDropPos_Y, transform.position.z), Quaternion.identity);
    }

    void Falling_Rock()
    {
        GameObject RockFall = Instantiate(Rock, new Vector3(RockDropPos_X, RockDropPos_Y, transform.position.z), Quaternion.identity);
    }

    void TongueWhip()
    { }

    IEnumerator ActionCooldown()
    {
        //OnAnim.SetTrigger("Idle");
        //OnikumaSprite.flipX = false;
        //WalkStepCount = 0;
        //Col_OffsetX = -0.017f;
        //Col_OffsetY = 0.0597f;
        //Col_sizeX = 4.82f;
        //Col_sizeY = 7.58f;
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

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        Gizmos.color = Color.blue;
        { Gizmos.DrawWireSphere(transform.position, AttackRange); }
    }
}
