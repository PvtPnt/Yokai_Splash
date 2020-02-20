using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{


    //public float WalkSpeed;
    //public float Tsuchinoko_Jumpforce;
    //public float groundCheckRange = 1f;
    //public float wallCheckRange = 1f;
    //public float EnemyCheckRange = 10f;
    //public float AttackRange;

    public int Damage;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform EnemyHitbox;
    //public Transform Current_WallChecker;
    //public Transform WallChecker_Right;
    //public Transform WallChecker_Left;

    public SpriteRenderer TsuchinokoSprite;

    public bool isPlayerinRange = false;
    public float stateDelay;
    private GameObject playerObj;

    //Projectile Behavior
    public Transform spawnPoint;
    public GameObject bullet;
    public float shotDelay;
    [SerializeField]
    private float currentShotDelay;
    bool isShoot = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    //void Update()
    ////{ if (HP <= 0) { Destroy(gameObject); } }

    void FixedUpdate()
    {
        if (!isPlayerinRange)
        {
            return;
        }
        else
        {
            currentShotDelay += Time.deltaTime;
           if(currentShotDelay > shotDelay)
            {
                EnemyShoot();
                currentShotDelay = 0f;
            }
        }

    }


    //public void ReceiveDamage(int Damage)
    //{
    //    HP -= Damage;
    //    Debug.Log("Damage taken");
    //}

    //void Walking()
    //{
    //    CheckWall();
    //    isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRange, groundLayer);
    //    if (isGrounded)
    //    {
    //        if (IsWalkingLeft == true)
    //        { StartCoroutine("Tsuchinoko_MoveLeft", 0.45f); }
    //        else if (IsWalkingLeft == false)
    //        { StartCoroutine("Tsuchinoko_MoveRight", 0.45f); }
    //    }
    //}

    //void CheckWall()
    //{
    //    if (IsWalkingLeft == true)
    //    {
    //        Current_WallChecker = WallChecker_Left;
    //        TsuchinokoSprite.flipX = false;
    //    }
    //    else if (IsWalkingLeft == false)
    //    {
    //        Current_WallChecker = WallChecker_Right;
    //        TsuchinokoSprite.flipX = true;
    //    }

    //    isWalled = Physics2D.OverlapCircle(Current_WallChecker.position, wallCheckRange, wallLayer);

    //    if (isWalled == true)
    //    {
    //        if (IsWalkingLeft == true)
    //        { IsWalkingLeft = false; }
    //        else if (IsWalkingLeft == false)
    //        { IsWalkingLeft = true; }
    //    }
    //}

    //IEnumerator Tsuchinoko_MoveLeft(float WaitTime)
    //{
    //    GetComponent<Rigidbody2D>().AddForce(Vector2.left * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
    //    GetComponent<Rigidbody2D>().AddForce(Vector2.up * Tsuchinoko_Jumpforce, ForceMode2D.Impulse);
    //    Vector3 temp = spawnPoint.localScale;
    //    temp.x *= -1;
    //    spawnPoint.localScale = temp;
    //    yield return new WaitForSecondsRealtime(WaitTime);
    //}

    //IEnumerator Tsuchinoko_MoveRight(float WaitTime)
    //{
    //    GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime, ForceMode2D.Force);
    //    GetComponent<Rigidbody2D>().AddForce(Vector2.up * Tsuchinoko_Jumpforce, ForceMode2D.Impulse);
    //    Vector3 temp = spawnPoint.localScale;
    //    temp.x = Mathf.Abs(temp.x);
    //    spawnPoint.localScale = temp;
    //    yield return new WaitForSecondsRealtime(WaitTime);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(Current_WallChecker.position, wallCheckRange);
    //    if (IsWalkingLeft == false)
    //    { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange); }

    //    if (IsWalkingLeft == true)
    //    { Gizmos.DrawWireSphere(EnemyHitbox.position, AttackRange); }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerinRange = true;
            playerObj = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerinRange = false;
            Invoke("DelayAttacktoIdle", stateDelay);
            currentShotDelay = 0;
        }
    }
    

    //void DelayAttacktoIdle()
    //{
    //    isPlayerinRange = false;
    //}
    void EnemyShoot()
    {
       float angle = Vector2.Angle(Vector2.right, playerObj.transform.position - spawnPoint.position);
       Quaternion rot = Quaternion.Euler(0f, 0f, angle);
       GameObject bulletObj = Instantiate(bullet, spawnPoint.position, rot) as GameObject;
    }
}
