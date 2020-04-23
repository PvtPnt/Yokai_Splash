using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{

    public int BoxIndex;
    public Sprite[] charPic;
    public string[] charName;
    public string[] message;

    //Obj Area
    public Image charPicObj;
    public Text charNameObj;
    public Text messageObj;

    public Player_cube_control player;


    // Start is called before the first frame update
    void Start()
    {
        charPicObj.sprite = charPic[BoxIndex];
        charNameObj.text = charName[BoxIndex];
        messageObj.text = message[BoxIndex];
        Time.timeScale = 0;

        player = GameObject.FindObjectOfType<Player_cube_control>();
        player.JumpForce = 0;
        player.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            BoxIndex++;
            if (BoxIndex <= charPic.Length - 1)
            {
                Time.timeScale = 0;                                 
                charPicObj.sprite = charPic[BoxIndex];
                charNameObj.text = charName[BoxIndex];
                messageObj.text = message[BoxIndex];
            }
            else
            {
                Time.timeScale = 1;
                player.gameObject.SetActive(true);
                player.JumpForce = 300;
                Destroy(this.gameObject);
            }

        }
    }
}
