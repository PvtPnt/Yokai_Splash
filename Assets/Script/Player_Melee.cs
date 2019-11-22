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
    public int atk_direction_x;
    public int atk_direction_y;
    public AudioSource myAudio;
    public AudioClip attackingSound;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 Direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 ATK_Direction_X = new Vector2(atk_direction_x, 0);
        Vector2 ATK_Direction_Y = new Vector2(0, atk_direction_y);
        if (BurstMode == false) { Damage = NormalMelee; };

        //Direction Control
        if (Direction.x < 0)
        {
            IsWalkingLeft = true;
            atk_direction_x = -1;
        }
        else if (Direction.x > 0)
        {
            IsWalkingLeft = false;
            atk_direction_x = 1;
        }

        //Burst Trigger
        if (Input.GetKeyDown(KeyCode.Q) && BurstMode == false)
        {Burst();}

        if (GetComponent<Player_cube_control>().isGrounded == false)
        {
            ATK_Direction_X = Vector2.zero;
            atk_direction_y = 0;

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * 500, ForceMode2D.Force);
            }
        }
        else { atk_direction_y = 1; }


        if (timeBTWattack <= 0)
        // you can attack
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.J))
            {
                myAudio.clip = attackingSound;
      
                myAudio.Play();
                GetComponent<Rigidbody2D>().AddForce(ATK_Direction_X * GetComponent<Player_cube_control>().WalkSpeed, ForceMode2D.Force);
                GetComponent<Rigidbody2D>().AddForce(ATK_Direction_Y * 100, ForceMode2D.Force);
                AttackDirection(Damage);
                timeBTWattack = Start_timeBTWattack;
            }

        }
        else {timeBTWattack -= Time.deltaTime;}
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

    private void AttackDirection(int Damage)
    {
        if (IsWalkingLeft == false)
        {
            Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosFront.position, AttackRange, EnemyLayer);
            for (int i = 0; i < DamageEnemy.Length; i++)
            { DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage); }
        }
        else if (IsWalkingLeft == true)
        {
            Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosBack.position, AttackRange, EnemyLayer);
            for (int i = 0; i < DamageEnemy.Length; i++)
            { DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage); }
        }
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
