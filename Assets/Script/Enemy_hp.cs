using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hp : MonoBehaviour
{
    public int HP;
    public int Defense;

    public bool isObject = false;
    public GameObject bucketToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
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
    { HP -= (Damage - Defense);

        Debug.Log("Took" + Damage + "damage");
        Debug.Log("Damage result" + (HP -= (Damage - Defense)));

    }

    public void DefDown(int DefDownValue)
    {   if (Defense > DefDownValue) { Defense -= DefDownValue; }
        else { Defense = 0; }
    }

}
