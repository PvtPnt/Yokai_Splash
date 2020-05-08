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
    int damagedsound;
    public AudioClip impact;
    public AudioClip impact2;
    AudioSource audioSource;
    public bool isboss =false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (isboss)
        {
            damagedsound = Random.Range(0, 2);
            if (damagedsound == 0)
            {
                audioSource.PlayOneShot(impact, 0.7F);
            }
            if (damagedsound == 1)
            {
                audioSource.PlayOneShot(impact2, 0.7F);
            }
        }
        //ApplyKnockBackForce();
    }

    public void DefDown(int DefDownValue)
    {   if (Defense >= DefDownValue) { Defense = Defense - DefDownValue; }
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
