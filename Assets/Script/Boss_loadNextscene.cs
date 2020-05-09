using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_loadNextscene : MonoBehaviour
{
    public GameObject Boss;
    public string Scene_to_load;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null && Input.GetKeyDown(KeyCode.E) || Boss == null && Input.GetKeyDown(KeyCode.JoystickButton0))
        {SceneManager.LoadScene(Scene_to_load);}
    }
}
