using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 2f;
    public int Damage;
    public SpriteRenderer bulletSprite;


    public Transform AttackPos;
    public LayerMask PlayerLayer;
    public float AttackRange;
    public bool isMovingLeft;

    // Start is called before the first frame update
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingLeft)
        {
            bulletSprite.flipX = true;
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
        }
        else
        { transform.Translate(Vector3.right * Time.deltaTime * speed); }

        Collider2D[] DamageEnemy = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, PlayerLayer);
        for (int i = 0; i < DamageEnemy.Length; i++)
        {
            if (DamageEnemy[i].gameObject.GetComponent<Player_cube_control>() != null)
            {
                DamageEnemy[i].gameObject.GetComponent<Player_cube_control>().P_ReceiveDamage(Damage);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }


}
