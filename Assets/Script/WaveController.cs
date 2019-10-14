using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public bool PlayerWave;
    public float Speed = 3f;
    public float Damage = 10f;
    public bool isMovingLeft;
    public bool isWalkingLeft;
    public float LifeTime = 3f;
    public GameObject Wave;

    public Transform Player;
    public float ZOffset;
    public float YOffset;
    public float XOffset;

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
            transform.Translate(-Vector3.right * Time.deltaTime * Speed);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
