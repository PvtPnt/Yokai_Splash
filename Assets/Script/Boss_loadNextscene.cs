using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_loadNextscene : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Prompt;
    public string Scene_to_load;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null)
        {
            Prompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) || Boss == null && Input.GetKeyDown(KeyCode.JoystickButton7))
            { SceneManager.LoadScene(Scene_to_load); }
        }
    }
}
