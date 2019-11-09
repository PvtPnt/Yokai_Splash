using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_cube_control : MonoBehaviour
{
    public int JumpCount = 0;

    private float timeBTWdash;
    public float Start_timeBTWdash;
    public float WalkSpeed;
    public float DashSpeed_multiplier;
    public float maxVelocity;
    public float JumpForce;
    public float groundCheckRange = 1f;
    public float HP;
    public float InvincibleTime = 1f;
    public float XOffset = 3f;
    public float YOffset = 1f;
    public float BurstTime = 10f;

    public bool isAlive;
    public bool isLoss;
    public bool isGrounded;
    public bool IsWalkingLeft = false;
    public bool BurstMode = false;

    public LayerMask groundLayer;

    public GameObject Bullet;
    public GameObject Trap;
    public GameObject Wave;

    public Transform GroundChecker;
    public SpriteRenderer PlayerSprite;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get Direction as of X-axis as 1 or -1
        Vector2 Direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        GetComponent<Rigidbody2D>().AddForce(Direction * WalkSpeed, ForceMode2D.Force);

        //Velocity Limit
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxVelocity)
        { GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxVelocity; }

        //Burst Mode Trigger
        if (Input.GetKeyDown(KeyCode.Q) && BurstMode == false || Input.GetKeyDown(KeyCode.JoystickButton4) && BurstMode == false)
        { Burst(); }

        //Sprite Animation flipping
        if (Direction.x < 0)
        {
            PlayerSprite.flipX = true;
            IsWalkingLeft = true;
        }
        else if (Direction.x > 0)
        {
            PlayerSprite.flipX = false;
            IsWalkingLeft = false;  
        }

        if (Input.GetKey(KeyCode.L) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (timeBTWdash <= 0)
            { Dash(); }
        }
        else { timeBTWdash -= Time.deltaTime; }


        isGrounded = Physics2D.OverlapCircle(GroundChecker.position, groundCheckRange, groundLayer);

        if (isGrounded)
        {
            JumpCount = 0;
        }



        //ATTACK--Shoot
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.U))
        {
            Shoot();
        }

        //TRAP
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && IsWalkingLeft == true
            || Input.GetKeyDown(KeyCode.O) && IsWalkingLeft == true)
        {
            GameObject NewTrap =
                Instantiate(Trap, transform.position + Vector3.left * XOffset + Vector3.down * YOffset, Quaternion.identity);
            NewTrap.GetComponent<TrapController>().IsWalkingLeft = IsWalkingLeft;
        }

        else if (Input.GetKeyDown(KeyCode.JoystickButton3) && IsWalkingLeft == false
            || Input.GetKeyDown(KeyCode.O) && IsWalkingLeft)
        {
            GameObject NewTrap =
                  Instantiate(Trap, transform.position + Vector3.right * XOffset + Vector3.down * YOffset, Quaternion.identity);
            NewTrap.GetComponent<TrapController>().IsWalkingLeft = IsWalkingLeft;
        }
    }

    void Update()
    {
        //jump controlling
        if (isGrounded) {
            GetComponent<Animator>().SetBool("jump", false);
            GetComponent<Animator>().SetBool("jump2", false);
        }
        
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && isGrounded
           || Input.GetKeyDown(KeyCode.K) && isGrounded)
        {
            GetComponent<Animator>().SetBool("jump", true);
            Debug.Log("Player jumped");
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce);
            JumpCount += 1;
        
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && isGrounded == false && JumpCount < 2
            || Input.GetKeyDown(KeyCode.K) && isGrounded == false && JumpCount < 2)
        {
            GetComponent<Animator>().SetBool("jump2", true);
            Debug.Log("Player jumped twice");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce * 1.5f);
            JumpCount = 2;
        }
        //Movement and animation
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && isGrounded == true)
        {
            GetComponent<Animator>().SetBool("Run", true);
            Debug.Log("Player running");
        } else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) { GetComponent<Animator>().SetBool("Run", false); }
    }

    void Shoot()
    {
        //animator.SetTrigger("Attack");
        GameObject NewBullet =
            Instantiate(Bullet, transform.position, Quaternion.identity);
        NewBullet.GetComponent<BulletController>().isMovingLeft = IsWalkingLeft;
    }


    IEnumerator Expire()
    {
        yield return new WaitForSeconds(BurstTime);
        BurstMode = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        gameObject.layer = 11; //Player layer
    }

    void Burst()
    {
            BurstMode = true;
            StartCoroutine("Expire", BurstTime);
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        gameObject.layer = 12; //Player_Invin layer
    }


    void Dash()
    {       
        if (IsWalkingLeft == true)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * DashSpeed_multiplier, ForceMode2D.Force); }
        else if (IsWalkingLeft == false)
        { GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * DashSpeed_multiplier, ForceMode2D.Force); };
        timeBTWdash = Start_timeBTWdash;
    }
    
    public void BurstHeal()
    {
        HP += 30;
    }

    public void P_ReceiveDamage(int Damage)
    {
        HP -= Damage;
        Debug.Log("Player take damage");
        StartCoroutine("Hit_Iframe", InvincibleTime);
    }

    IEnumerator Hit_Iframe()
    {
        gameObject.layer = 12; //Player_Invin layer
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(InvincibleTime);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        gameObject.layer = 11; //Player layer

    }

    //public void ReceiveDamage(int Damage)
    // {
    //    HP -= Damage;

    //     if (HP <= 0)
    //     {
    //Destroy(this.gameObject);         // method 1
    //animator.SetTrigger("Death");     // method 2, if I have death anim

    //PlayerSprite.color = Color.red;
    //         isAlive = false;
    //         isLoss = true;
    //        gameObject.SetActive(false);
    // method 3
    //    }
    //      else
    //     {
    //         StartCoroutine("TakeDamage", 1);
    //     }
    //  }
}
