using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public int Damage;
    public float DetectionRange;
    public float DetectToShootTime;
    public float ShootRate;

    public LayerMask playerLayer;
    public Transform EnemyHitbox;

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
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Collider2D FindPlayer = Physics2D.OverlapCircle(transform.position, DetectionRange, playerLayer);
        if (FindPlayer.gameObject.gameObject.GetComponent<Player_cube_control>() != null)
        { isPlayerinRange = true; }
        else { isPlayerinRange = false; }

        if (!isPlayerinRange)
        {
            CancelInvoke();
            return;
        }
        else
        {
            //InvokeRepeating("EnemyShoot", DetectToShootTime, ShootRate);
            currentShotDelay += Time.deltaTime;
            if (currentShotDelay > shotDelay)
            {
                EnemyShoot();
            }
        }

    }



    void EnemyShoot()
    {
        float angle = Vector2.Angle(Vector2.right, playerObj.transform.position - spawnPoint.position);
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);
        GameObject bulletObj = Instantiate(bullet, spawnPoint.position, rot) as GameObject;
        currentShotDelay = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }
}
