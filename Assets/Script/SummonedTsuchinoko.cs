using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedTsuchinoko : MonoBehaviour
{
    public float speed;
    public bool GoLeft;
    public bool GoRight;
    public Sprite goo_sprite;
    SpriteRenderer summon_sprite;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        summon_sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GoLeft)
        { 
            summon_sprite.flipX = true;
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3 (100,0,0), step);

        }

        else if (GoRight)
        {
            summon_sprite.flipX = false;
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(100, 0, 0), step);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            speed = 0;
            Destroy(GetComponent<Animator>());
            transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            summon_sprite.sprite = goo_sprite;
            summon_sprite.color = new Color(0.6544433f, 0, 1f, 1f);
            other.GetComponent<Player_cube_control>().P_ReceiveDamage(5);
            Destroy(this.gameObject, 1f);
        }

        if (other.tag == "InvisibleWall")
        {
            speed = 0;
            Destroy(GetComponent<Animator>());
            transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            summon_sprite.sprite = goo_sprite;
            summon_sprite.color = new Color(0.6544433f, 0, 1f, 1f);
            Destroy(this.gameObject);
        }
    }
}
