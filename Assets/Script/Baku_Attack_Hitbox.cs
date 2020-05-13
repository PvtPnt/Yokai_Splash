using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baku_Attack_Hitbox : MonoBehaviour
{
    int Damage;
    public bool isPrinceLaser;
    public bool isStomp;
    public int StompDamage;

    public bool isBeam;
    public int BeamDamage;
    public int BeamDOT;

    float Col_OffsetX;
    float Col_OffsetY;

    float Col_Size_x;
    float Col_Size_y;

    CorruptedPrince_Script Prince_Controller;
    Animator Prince_Animator;
    Animator Baku_Animator;
    Baku_Script Baku_Controller;
    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (isStomp) { Damage = StompDamage; }
        else if (isBeam) { Damage = BeamDamage; }

        if(isPrinceLaser)
        {
            Prince_Animator = GameObject.Find("Prince").GetComponent<Animator>();
            Prince_Controller = GameObject.Find("Prince").GetComponent<CorruptedPrince_Script>();
        }

        else
        {
            Baku_Animator = GameObject.Find("Baku").GetComponent<Animator>();
            Baku_Controller = GameObject.Find("Baku").GetComponent<Baku_Script>();
        }
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider.offset = new Vector2(Col_OffsetX, Col_OffsetY);
        boxCollider.size = new Vector2(Col_Size_x, Col_Size_y);
    }

    public void Beam_1()
    {
        Col_OffsetX = 4.43641f;
        Col_OffsetY = 0.4116654f;
        Col_Size_x = 4.815845f;
        Col_Size_y = 1.819431f;
    }

    public void Beam_2()
    {
        Col_OffsetX = 2.662477f;
        Col_OffsetY = -0.3557591f;
        Col_Size_x = 8.363701f;
        Col_Size_y = 3.35428f;
    }

    public void Beam_3()
    {
        Col_OffsetX = -0.3266494f;
        Col_OffsetY = -0.3079047f;
        Col_Size_x = 14.24673f;
        Col_Size_y = 3.738518f;
    }

    public void Beam_out()
    {
        Baku_Animator.SetBool("Shooting_Beam", false);
        Baku_Controller.StartCoroutine("ActionCooldown");
        Destroy(this.gameObject);
    }

    public void Prince_Laser_out()
    {
        Prince_Animator.SetBool("ShootingLaser", false);
        Prince_Controller.AttackCount += 1;
        Prince_Controller.StartCoroutine("ActionCooldown");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            other.GetComponent<Player_cube_control>().P_ReceiveDamage(Damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player" && isBeam)
        {
            other.GetComponent<Player_cube_control>().P_ReceiveDamage(BeamDOT);
        }
    }
}
