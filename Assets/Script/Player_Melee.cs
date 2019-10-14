using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Melee : MonoBehaviour
{
    private float timeBTWattack;
    public float Start_timeBTWattack;

    public Transform AttackPosFront;
    public Transform AttackPosBack;
    public LayerMask EnemyLayer;
    public float AttackRange;
    public int Damage;
    public bool IsWalkingLeft = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        if (Direction.x < 0)
        {
            IsWalkingLeft = true;
        }
        else if (Direction.x > 0)
        {
            IsWalkingLeft = false;
        }

        if (timeBTWattack<=0)
        {   //you can attack

            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.J))
            //if (Input.GetKeyDown(KeyCode.J))
            {
                if(IsWalkingLeft == false)
                {
                    Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosFront.position, AttackRange, EnemyLayer);
                    for (int i = 0; i < DamageEnemy.Length; i++)
                    {
                        DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage);
                    }
                }

                if (IsWalkingLeft == true)
                {
                    Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPosBack.position, AttackRange, EnemyLayer);
                    for (int i = 0; i < DamageEnemy.Length; i++)
                    {
                        DamageEnemy[i].GetComponent<Enemy_basic>().ReceiveDamage(Damage);
                    }
                }

                timeBTWattack = Start_timeBTWattack;
            }


        }

        else { timeBTWattack -= Time.deltaTime; }
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
