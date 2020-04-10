using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public int Damage = 25;
    public float secondsToWait = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("endPush");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {


                other.SendMessage("ReceiveDamage", Damage);
            Debug.Log("Dealt" + Damage + "damage");
            }


    }


    IEnumerator endPush()
    {

        yield return new WaitForSeconds(secondsToWait);
        Destroy(this.gameObject);
    }
}
