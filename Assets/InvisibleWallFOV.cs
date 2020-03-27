using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibleWallFOV : MonoBehaviour
{
    public Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            print("Collide Player");
            print(MainCamera.fieldOfView);
            MainCamera.fieldOfView = 80;
        }
        else
        {
            MainCamera.fieldOfView = 60;
        }
    }
    private void OnDestroy()
    {
        MainCamera.fieldOfView = 60;
    }
}
