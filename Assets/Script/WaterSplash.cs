using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public int Damage = 25;
    public float secondsToWait = 2.0f;
    public GameObject flash;

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
            GameObject NewFlash =
              Instantiate(flash, transform.position, Quaternion.identity);

            NewFlash.gameObject.SetActive(true);

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
