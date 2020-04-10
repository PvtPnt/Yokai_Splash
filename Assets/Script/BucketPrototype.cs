using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPrototype : MonoBehaviour
{
    public int Damage = 100;
    public float CanTank = 1;
    public GameObject ExplosionGameObject;
    public bool isBossType;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(CanTank == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Made contact2!");
            if (other.GetComponent<Enemy_basic>().onPush == true)
            {
                --CanTank;
                GameObject NewExplosion =
                Instantiate(ExplosionGameObject, transform.position, Quaternion.identity);

                NewExplosion.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Not on push");
            }
        }

        if (other.tag == "Wave" && isBossType == true)
        {

            --CanTank;
            GameObject NewExplosion =
            Instantiate(ExplosionGameObject, transform.position, Quaternion.identity);

            NewExplosion.gameObject.SetActive(true);
        }

        else
        {
            Debug.Log("Not on push");
        }
        }
    }


