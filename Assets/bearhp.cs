using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bearhp : MonoBehaviour
{
    public int HP;
    public int Defense;

    public bool isObject = false;
    int damagedsound;
    public AudioClip impact;
    public AudioClip impact2;
    AudioSource audioSource;
    public GameObject bucketToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            if (isObject == true)
            {
                bucketToSpawn.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    public void ReceiveDamage(int Damage)
    {
        if (Damage < Defense) { Defense = Damage; }
        HP -= (Damage - Defense);

        Debug.Log("Took" + Damage + "damage");
        Debug.Log("Damage result" + (HP -= (Damage - Defense)));
        damagedsound = Random.Range(0, 1);
        if(damagedsound == 0)
        {
            audioSource.PlayOneShot(impact, 0.7F);
        }
        if (damagedsound == 1)
        {
            audioSource.PlayOneShot(impact2, 0.7F);
        }
    }

    public void DefDown(int DefDownValue)
    {
        if (Defense > DefDownValue) { Defense -= DefDownValue; }
        else { Defense = 0; }
    }

}