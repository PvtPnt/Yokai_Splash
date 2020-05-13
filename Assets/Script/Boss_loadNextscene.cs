using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_loadNextscene : MonoBehaviour
{
    bool isSeaching = true;
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
        if (isSeaching)
        {
            if (Boss.GetComponent<Enemy_hp>().HP <= 0)
            {
                Prompt.SetActive(true);
                isSeaching = false;
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.E) || Boss == null && Input.GetKeyDown(KeyCode.JoystickButton7))
            { SceneManager.LoadScene(Scene_to_load); }
        }
    }
}
