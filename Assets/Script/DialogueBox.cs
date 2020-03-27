using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{

    public Sprite charPic;
    public string charName;
    public string message;

    //Obj Area
    public Image charPicObj;
    public Text charNameObj;
    public Text messageObj;


    // Start is called before the first frame update
    void Start()
    {
        charPicObj.sprite = charPic;
        charNameObj.text = charName;
        messageObj.text = message;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Time.timeScale = 1;
            Destroy(this.gameObject);
        }
    }
}
