using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hp : MonoBehaviour
{
    public int HP;
    public int Defense;

    public bool isObject = false;
    public GameObject bucketToSpawn;
    public float knockbackForce;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     if (HP <= 0) {
            if (isObject == true)
            {
                bucketToSpawn.gameObject.SetActive(true);
            }
            Destroy(gameObject); }   
    }

    public void ReceiveDamage(int Damage)
    { 
        if (Damage < Defense) { Defense = Damage; }
        HP -= (Damage - Defense);

        Debug.Log("Took" + Damage + "damage");
        Debug.Log("Damage result" + (HP -= (Damage - Defense)));
        SpriteRenderer enemySprite = gameObject.GetComponent<SpriteRenderer>();
        enemySprite.color = Color.red;
        Invoke("changeOriginalColor", 0.5f);
        //ApplyKnockBackForce();
    }

    public void DefDown(int DefDownValue)
    {   if (Defense > DefDownValue) { Defense -= DefDownValue; }
        else { Defense = 0; }
    }

    IEnumerator Delay(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    void changeOriginalColor()
    {
        SpriteRenderer enemySprite = gameObject.GetComponent<SpriteRenderer>();
        enemySprite.color = Color.white;
    }

    /*void ApplyKnockBackForce()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

        print(rigidbody2D.velocity.x);
        if (rigidbody2D.velocity.x >= 0 && !sprite.flipX)
        { 
            rigidbody2D.AddForceAtPosition(new Vector2(knockbackForce, 0.0f), gameObject.transform.position);
        }
        else if (rigidbody2D.velocity.x >= 0 && sprite.flipX)
        {
            rigidbody2D.AddForceAtPosition(new Vector2(knockbackForce, 0.0f), gameObject.transform.position);
        }
        else if(rigidbody2D.velocity.x <= 0)
        {

            rigidbody2D.AddForceAtPosition(new Vector2(-knockbackForce, 0.0f), gameObject.transform.position);

        }
    }*/

}
