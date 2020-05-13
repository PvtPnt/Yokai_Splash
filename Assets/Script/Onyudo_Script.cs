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
    public bool isDead;

    public float AttackRange;
    public float WalkStepLength;
    public float CooldownTimer;

    public Vector3 Impaler_Pos;
    public GameObject Rubble;
    public GameObject Rock;
    public GameObject Fireball;
    public GameObject Impaler;
    public GameObject TongueShot_HitBox;
    public Transform AttackStartPosition;
    public Transform AttackEndPosition;
    public LayerMask playerLayer;

    bool isPerformingAction;
    int ActionIndex;
    Animator OnAnim;

    public GameObject bossDialogue;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        OnAnim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");

        bossDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            GetComponent<BoxCollider2D>().offset = new Vector2(0.2296753f, 0.285043f);
            GetComponent<BoxCollider2D>().size = new Vector2(8.233887f, 2.416478f);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            bossDialogue.SetActive(true);
            return;
        }

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
            if (ActionIndex == 1)       { OnAnim.SetTrigger("TongueZonePrep"); }
            else if (ActionIndex == 2)  { OnAnim.SetTrigger("Fireball"); }
            else if (ActionIndex == 3)  { OnAnim.SetTrigger("ClapPrep"); }
            else if (ActionIndex == 4)  { OnAnim.SetTrigger("TonguePrep"); }
        }
    }

    void SummonImpaler()
    {GameObject NewImpaler = Instantiate(Impaler, Impaler_Pos, Quaternion.identity);}

    void Cooldown()
    { StartCoroutine("ActionCooldown"); }

    void SpawnFireball()
    {
        GameObject FlamingPalm = Instantiate(Fireball, transform.position + new Vector3(-3f,1.5f,0f), Quaternion.identity);
    }

    void Clap()
    {
        ClapCount += 1;
        if (ClapCount >= ClapCountMax)
        {
            //CancelInvoke("Clap");
            OnAnim.SetTrigger("Cooldown");
            StartCoroutine("ActionCooldown");
        }

        else
        {OnAnim.SetTrigger("ClapPrep");}
    }

    void Falling_Rubble()
    {
        RockDropPos_X = Player.transform.position.x;
        GameObject RubbleFall  = Instantiate(Rubble, new Vector3 (RockDropPos_X, RockDropPos_Y, transform.position.z), Quaternion.identity);
    }

    void Falling_Rock()
    {
        GameObject RockFall = Instantiate(Rock, new Vector3(RockDropPos_X, RockDropPos_Y * 1.5f, transform.position.z), Quaternion.identity);
    }

    void TongueShot_CollisionOn()
    { TongueShot_HitBox.SetActive(true);}

    void TongueShot_CollisionOff()
    { 
        TongueShot_HitBox.SetActive(false);
        StartCoroutine("ActionCooldown");
    }

    IEnumerator ActionCooldown()
    {
        CancelInvoke();
        OnAnim.SetTrigger("idle");
        //StartCoroutine("ReturnCollider");
        Debug.Log("Action in cooldown");
        yield return new WaitForSeconds(CooldownTimer);
        ClapCount = 0;
        isPerformingAction = false;
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        Gizmos.color = Color.blue;
        { Gizmos.DrawWireSphere(transform.position, AttackRange); }
    }
    //private void OnDestroy()
    //{
    //    print("Something");
    //    print("Something");
    //    bossDialogue.SetActive(true);
    //}
}
