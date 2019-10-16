using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Melee : MonoBehaviour
{
    private float timeBTWattack;
    public float Start_timeBTWattack;
    public float AttackRange;
    public float BurstTime = 10f;

    public Transform AttackPosFront;
    public Transform AttackPosBack;
    public LayerMask EnemyLayer;

    public bool IsWalkingLeft = false;
    public bool BurstMode = false;

    public int Damage;
    public int NormalMelee;
    public int BurstMelee;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (BurstMode == false) { Damage = NormalMelee; };

        //Direction Control
        if (Direction.x < 0)
        {IsWalkingLeft = true;}
        else if (Direction.x > 0)
        {IsWalkingLeft = false;}

        //Burst Trigger
        if (Input.GetKeyDown(KeyCode.Q) && BurstMode == false)
        {Burst();}

        //Melee Attack -- Refer to Player_Melee.cs
        if (timeBTWattack<=0)
        {   //you can attack

            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.J))
            {
                if(IsWalkingLeft == false)
                {
                    Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosFront.position, AttackRange, EnemyLayer);
                    for (int i = 0; i < DamageEnemy.Length; i++)
                    {DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage); }
                }

                if (IsWalkingLeft == true)
                {
                    Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosBack.position, AttackRange, EnemyLayer);
                    for (int i = 0; i < DamageEnemy.Length; i++)
                    {DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage);}
                }

                timeBTWattack = Start_timeBTWattack;
            }
        }
        else { timeBTWattack -= Time.deltaTime; }
    }


    IEnumerator Expire()
    {
        yield return new WaitForSeconds(BurstTime);
        BurstMode = false;
        BurstOffDMG();
    }

    void Burst()
    {
            BurstMode = true;
            GetComponent<Player_cube_control>().BurstHeal();
            BurstOnDMG();
            StartCoroutine("Expire", BurstTime);
    }

    public void BurstOnDMG()
    {Damage = BurstMelee;}

    public void BurstOffDMG()
    {Damage = NormalMelee;}

    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        if(IsWalkingLeft == false)
        { Gizmos.DrawWireSphere(AttackPosFront.position, AttackRange); }

        if(IsWalkingLeft == true)
        { Gizmos.DrawWireSphere(AttackPosBack.position, AttackRange); }
    }
}
