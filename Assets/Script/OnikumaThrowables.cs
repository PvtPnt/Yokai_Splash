using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnikumaThrowables : MonoBehaviour
{
    public float Speed = 3f;
    public float LifeTime;
    public int Damage;

    public bool isMovingLeft;

    public GameObject Throwables;
    public SpriteRenderer ThrowableSprite;

    public Transform AttackPos;
    public LayerMask PlayerLayer;

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
            //ThrowableSprite.flipX = true;
            transform.Translate(-Vector3.right * Time.deltaTime * Speed);
        }
        else
        { transform.Translate(Vector3.right * Time.deltaTime * Speed); }

        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, PlayerLayer);
        for (int i = 0; i < DamagePlayer.Length; i++)
        {
            DamagePlayer[i].gameObject.GetComponent<Player_cube_control>().P_ReceiveDamage(Damage);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }
}
