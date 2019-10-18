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
    public float LifeTime = 3f;
    public GameObject Wave;

    public Transform Player;
    public float ZOffset;
    public float YOffset;
    public float XOffset;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (Direction.x < 0)
        { IsWalkingLeft = true; }
        else if (Direction.x > 0)
        { IsWalkingLeft = false; }
        StartCoroutine("Expire", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsWalkingLeft == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Speed);
        }
        else if (IsWalkingLeft == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Wave && other.tag == "Enemy")
        {
            //Deal dmg to enemy
            other.SendMessage("ReceiveDamage", Damage);
            Destroy(this.gameObject);
        }

        if (Wave && other.tag == "Player")
        {
            //Deal dmg to player
            other.SendMessage("ReceiveDamage", Damage);
            Destroy(this.gameObject);
        }
    }
}
