using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Melee : MonoBehaviour
{
    private float timeBTWattack;
    public float Start_timeBTWattack;
    public float AttackRange;
    public float MeleeWaterCost;

    public Transform AttackPosFront;
    public Transform AttackPosBack;
    public LayerMask EnemyLayer;
    private Player_cube_control PlayerMainScript;

    public bool IsWalkingLeft = false;

    public int Damage;
    public AudioClip swingingSound;
    public int atk_direction_x;
    public int atk_direction_y;


    // Start is called before the first frame update
    void Start()
    {
        PlayerMainScript = GetComponent<Player_cube_control>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 ATK_Direction_X = new Vector2(atk_direction_x, 0);
        Vector2 ATK_Direction_Y = new Vector2(0, atk_direction_y);

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

        if (PlayerMainScript.isGrounded == false)
        
        {
            ATK_Direction_X = Vector2.zero;
            atk_direction_y = 0;

            if (Input.GetKeyDown(KeyCode.S)) GetComponent<Rigidbody2D>().AddForce(Vector2.down * 500, ForceMode2D.Force);
        }
        else { atk_direction_y = 1; }


        if (timeBTWattack <= 0)
        // you can attack
        {
            if (PlayerMainScript.Water >= MeleeWaterCost)
            {
                GetComponent<Animator>().SetBool("Melee", false);
                if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.J))
                {
                    PlayerMainScript.Water -= MeleeWaterCost;
                    GetComponent<Player_cube_control>().myAudioAttack.clip = swingingSound;
                    GetComponent<Player_cube_control>().myAudioAttack.Play();
                    GetComponent<Animator>().SetBool("Melee", true);
                    GetComponent<Rigidbody2D>().AddForce(ATK_Direction_X * GetComponent<Player_cube_control>().WalkSpeed * 3.0f, ForceMode2D.Force);
                    GetComponent<Rigidbody2D>().AddForce(ATK_Direction_Y * 120, ForceMode2D.Force);
                    AttackDirection(Damage);
                    timeBTWattack = Start_timeBTWattack;
                }
            }


        }
        else {timeBTWattack -= Time.deltaTime;}
    }

    private void AttackDirection(int Damage)
    {
        if (IsWalkingLeft == false)
        {
            Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosFront.position, AttackRange, EnemyLayer);
            for (int i = 0; i < DamageEnemy.Length; i++)
            { DamageEnemy[i].GetComponent<Enemy_hp>().ReceiveDamage(Damage); }
        }
        else if (IsWalkingLeft == true)
        {
            Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosBack.position, AttackRange, EnemyLayer);
            for (int i = 0; i < DamageEnemy.Length; i++)
            { DamageEnemy[i].GetComponent<Enemy_hp>().ReceiveDamage(Damage); }
        }
    }


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
