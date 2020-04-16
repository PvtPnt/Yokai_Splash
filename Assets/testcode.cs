using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetBool("Attacking", false);
        if (Input.GetKeyDown(KeyCode.X))
        {


            GetComponent<Animator>().SetBool("Attacking", true);
        }
    }
}
