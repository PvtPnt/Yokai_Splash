﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPrototype : MonoBehaviour
{
    public float Damage = 100f;
    public float CanTank = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(CanTank == 0)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Made contact!");
            //Deal dmg to enemy
            if (other.GetComponent<Enemy_basic>().onPush == true)
            {
                other.SendMessage("ReceiveDamage", Damage);
                --CanTank;
            }
        }
    }
}
