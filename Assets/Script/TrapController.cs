using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public bool PlayerTrap;
    public float Speed = 3f;
    public float Damage = 10f;
    public bool IsWalkingLeft;
    public float LifeTime = 1f;
    public GameObject Trap;
    public GameObject Wave;

    public Transform Player;
    public float ZOffset;
    public float YOffset;
    public float XOffset;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Expire", LifeTime);
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player)
        {
            ZOffset = transform.position.z - Player.position.z;
            //YOffset = 2.0f;
            XOffset = 3.0f;
        }

    
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        //Direction Control
        if (Direction.x < 0)
        { IsWalkingLeft = true; }
        else if (Direction.x > 0)
        { IsWalkingLeft = false; }*/


        if (Input.GetAxis("Fire1") > 0 || Input.GetKeyDown(KeyCode.P))
        {
            if (IsWalkingLeft == false)
            {
                GameObject NewWave =
                Instantiate(Wave, transform.position + Vector3.right * XOffset, Quaternion.identity);
                NewWave.GetComponent<WaveController>().DirectionIsLeft = false;
            }
            else if (IsWalkingLeft == true)
            {
                GameObject NewWave =
                Instantiate(Wave, transform.position + Vector3.left * XOffset, Quaternion.identity);
                NewWave.GetComponent<WaveController>().DirectionIsLeft = true;
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerTrap && other.tag == "Enemy")
        {
            //Deal dmg to enemy
            other.SendMessage("ReceiveDamage", Damage);
            Destroy(this.gameObject);
        }

        if (!PlayerTrap && other.tag == "Player")
        {
            //Deal dmg to player
            other.SendMessage("ReceiveDamage", Damage);
            Destroy(this.gameObject);
        }
    }


}
