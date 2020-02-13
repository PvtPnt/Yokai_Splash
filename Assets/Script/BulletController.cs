using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool PlayerBullet;
    public float Speed = 3f;
    public int Damage;
    public bool isMovingLeft;
    public float LifeTime = 1f;
    public GameObject Bullet;
    public SpriteRenderer bulletSprite;

    public Transform AttackPos;
    public LayerMask EnemyLayer;

    public float AttackRange;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Expire", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingLeft)
        {
            bulletSprite.flipX = true;
            transform.Translate(-Vector3.right * Time.deltaTime * Speed);
        }
        else
        {transform.Translate(Vector3.right * Time.deltaTime * Speed);}

        Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, EnemyLayer);
        for (int i = 0; i < DamageEnemy.Length; i++)
        {
            DamageEnemy[i].GetComponent<Enemy_hp>().DefDown(5);
            DamageEnemy[i].GetComponent<Enemy_hp>().ReceiveDamage(Damage);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (PlayerBullet && other.tag == "Enemy")
    //    {
    //        //Deal dmg to enemy
    //        other.SendMessage("ReceiveDamage", Damage);
    //        Destroy(this.gameObject);
    //    }

    //    if (!PlayerBullet && other.tag == "Player")
    //    {
    //        //Deal dmg to player
    //        other.SendMessage("ReceiveDamage", Damage);
    //        Destroy(this.gameObject);
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }


}
