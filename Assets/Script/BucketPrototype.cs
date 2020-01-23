using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPrototype : MonoBehaviour
{
    public float Damage = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Made contact!");
            //Deal dmg to enemy
            other.SendMessage("ReceiveDamage", Damage);
            Destroy(this.gameObject);
        }
    }
}
