﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player_cube_control : MonoBehaviour
{
    public int JumpCount = 0;

    private float timeBTWdash;
    public float Start_timeBTWdash;
    private float timeBTWshoot;
    public float Start_timeBTWshoot;
    public float WalkSpeed;
    public float DashSpeed_multiplier;
    public float JumpForce;
    public float groundCheckRange = 1f;

    public float HP;
    public float MaxHP;
    public float HP_Regen_Amount;

    public float Water_regen_rate;
    public float MaxWater;
    public float Water;
    public float Water_cost;

    public float InvincibleTime = 1f;
    public float DashFrame = 0.3f;
    public float XOffset = 3f;
    public float YOffset = 1f;
    [HideInInspector] public float Walking;

    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isLoss;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isGrounded;
    public bool IFrameActivation = false;
    public bool IsWalkingLeft = false;

    public LayerMask groundLayer;

    public AudioSource myAudio;
    public AudioSource myAudioHit;
    public AudioSource myAudioAttack;
    public GameObject DeadPrompt;
    public GameObject Bullet;
    public GameObject Trap;
    public GameObject Wave;
    public AudioClip dashingSound;
    public AudioSource myMusic;
    public AudioClip hitSound;
    public AudioClip shootingSound;
    public AudioClip jumpingSound;

    public Transform GroundChecker;
    public SpriteRenderer PlayerSprite;
    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP<=0)
        {
            GetComponent<Player_Melee>().enabled = false;
            GetComponent<Animator>().SetBool("alive", false);
            GetComponent<Animator>().SetBool("dash", false);
            GetComponent<Animator>().SetBool("Run", false);
            JumpForce = 0;
            WalkSpeed = 0;
            myMusic.enabled = false;
            DeadPrompt.SetActive(true);
            GetComponent<Player_OnDead>().enabled = true;
            GetComponent<Player_cube_control>().enabled = false;
            return;
        }

        GetComponent<Animator>().SetBool("shoot", false);
        GetComponent<Animator>().SetBool("dash", false);
        //Get Direction as of X-axis as 1 or -1
        Vector2 Direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        GetComponent<Rigidbody2D>().AddForce(Direction * WalkSpeed, ForceMode2D.Force);

        Vector2 velo = GetComponent<Rigidbody2D>().velocity;
        velo.x = Direction.x * WalkSpeed * Time.deltaTime;

        GetComponent<Rigidbody2D>().velocity = velo;

       
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        //TRAP
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && IsWalkingLeft == true
            || Input.GetKeyDown(KeyCode.O) && IsWalkingLeft == true)
        {
            GameObject NewTrap =
                Instantiate(Trap, transform.position, Quaternion.identity);
            NewTrap.GetComponent<WaveController>().DirectionIsLeft = false;
        }

        else if (Input.GetKeyDown(KeyCode.JoystickButton3) && IsWalkingLeft == false
            || Input.GetKeyDown(KeyCode.O) && IsWalkingLeft == false)
        {
            GameObject NewTrap =
                  Instantiate(Trap, transform.position, Quaternion.identity);
            NewTrap.GetComponent<WaveController>().DirectionIsLeft = true;
        }


        //Health Regeneration
        HP += HP_Regen_Amount * Time.deltaTime;
        if (HP > MaxHP) { HP = MaxHP; };

        Vector2 Direction = new Vector2(Input.GetAxis("Horizontal"), 0);


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

        if (Input.GetKey(KeyCode.L) && Direction.x != 0 || Input.GetKey(KeyCode.JoystickButton1) && Direction.x != 0)
        {
            if (timeBTWdash <= 0)
            {
                Dash();
                StartCoroutine("Dashframe", DashFrame);
            }
        }
        else { timeBTWdash -= Time.deltaTime; }

        if (isDashing == true)
        {
            GetComponent<Animator>().SetBool("dash", true);
        }

        //ATTACK--Shoot
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.U))
        {
            if (timeBTWshoot <= 0 && Water > Water_cost)
            {
                Water = Water - Water_cost;
                myAudioAttack.clip = shootingSound;
                myAudioAttack.Play();
                GetComponent<Animator>().SetBool("shoot", true);
                Shoot();
            }
        } else { timeBTWshoot -= Time.deltaTime; }

        //Water Regeneration
        Water += Water_regen_rate * Time.deltaTime;
        if (Water > MaxWater) { Water = MaxWater; };

        //Movement and animation
        Walking = Input.GetAxis("Horizontal");
        if (Walking != 0 && isGrounded == true)
        {
            GetComponent<Animator>().SetBool("Run", true);
        }
        else if (Walking == 0 & isGrounded) { GetComponent<Animator>().SetBool("Run", false); }

        isGrounded = Physics2D.OverlapCircle(GroundChecker.position, groundCheckRange, groundLayer);

        //jump controlling
        if (isGrounded)
        {
            if (GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                JumpCount = 0;
                GetComponent<Animator>().SetBool("jump", false);
                GetComponent<Animator>().SetBool("jump2", false);
             }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && isGrounded
           || Input.GetKeyDown(KeyCode.K) && isGrounded)
        {
            myAudio.clip = jumpingSound;
            myAudio.Play();
            GetComponent<Animator>().SetBool("jump", true);
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce);
            JumpCount += 1;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && isGrounded == false && JumpCount < 2
            || Input.GetKeyDown(KeyCode.K) && isGrounded == false && JumpCount < 2)
        {
            myAudio.clip = jumpingSound;
            myAudio.Play();
            GetComponent<Animator>().SetBool("jump2", true);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce * 1.1f);
            JumpCount = 2;
        }

    }

    void HealthRegeneration()
    {
        HP += HP_Regen_Amount;
    }

    void Shoot()
    {
        GameObject Player_Bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        Player_Bullet.GetComponent<BulletController>().isMovingLeft = IsWalkingLeft;
        timeBTWshoot = Start_timeBTWshoot;
    }

    void Dash()
    {
        isDashing = true;
        collider.isTrigger = false;
        myAudio.clip = dashingSound;
        myAudio.Play();
            if (IsWalkingLeft == true)
            {
                InvokeRepeating("SpawnTrailPart", 0f, 0.03f);
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * DashSpeed_multiplier, ForceMode2D.Force);
                // Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds.
            }
            else if (IsWalkingLeft == false)
            {
                InvokeRepeating("SpawnTrailPart", 0f, 0.03f);
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * DashSpeed_multiplier, ForceMode2D.Force);
                // Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds.
            };
        timeBTWdash = Start_timeBTWdash;
    }


    public void P_ReceiveDamage(int Damage)
    {
            HP -= Damage;
            StartCoroutine("Iframe", InvincibleTime);
            myAudioHit.clip = hitSound;
            myAudioHit.Play();
    }

    IEnumerator Iframe(float InvincibleTime)
    {
        gameObject.layer = 12; //Player_Invin layer
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(InvincibleTime);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        gameObject.layer = 11; //Player layer
    }

    IEnumerator Dashframe()
    {
        gameObject.layer = 12; //Player_Invin layer
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(DashFrame);
        gameObject.layer = 11; //Player layer
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject();
        trailPart.layer = 12; //Make trail layer == Invincible layer
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sortingLayerName = "Player";
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale;

        Vector2 Trail_Scale = transform.localScale;
        Trail_Scale.x *= -1;

        if (PlayerSprite.flipX == true)
        { trailPart.transform.localScale = Trail_Scale; }
        else if (PlayerSprite.flipX == false)
        { trailPart.transform.localScale = transform.localScale; }

        StartCoroutine(FadeTrailPart(trailPartRenderer));
        Destroy(trailPart, 0.3f); // replace 0.xf with needed lifeTime
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.5f; // replace 0.xf with needed alpha decrement
        trailPartRenderer.color = color;

        yield return new WaitForSeconds(0.3f);
        if(isGrounded) { CancelInvoke("SpawnTrailPart"); }
        collider.isTrigger = false;
        isDashing = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy_Bullet")
        {
            P_ReceiveDamage(5);
            Destroy(collision.gameObject);
        }
    }
}
