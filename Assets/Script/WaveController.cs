using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public bool PlayerWave;
    public float Speed = 3f;
    public float Damage = 10f;
    public bool isMovingLeft;
    public bool IsWalkingLeft;
    public bool DirectionIsLeft;
    public float LifeTime = 3f;
    public GameObject Wave;

    public Transform Player;
    public bool waveDirectionLeft;
    public float ZOffset;
    public float YOffset;
    public float XOffset;

    // Start is called before the first frame update
    void Start()
    {
        /*Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (Direction.x < 0)
        { IsWalkingLeft = true; }
        else if (Direction.x > 0)
        { IsWalkingLeft = false; }
        */
        StartCoroutine("Expire", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {

        if (DirectionIsLeft == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Speed);
            waveDirectionLeft = true;
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed);
            waveDirectionLeft = false;
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Made contact!");
            //Deal dmg to enemy
            other.SendMessage("ReceiveDamage", Damage);
            other.SendMessage("PushedBack", DirectionIsLeft);
            Destroy(this.gameObject);
        }

    }
}
