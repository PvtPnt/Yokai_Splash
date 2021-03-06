﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutScene : MonoBehaviour
{
    public Sprite[] cutSceneArr;
    public int currentImgIndex;
    public Image currentImg;
    public Player_cube_control player;

    public bool CutSceneEnd;


    public int BoxIndex;
    public Sprite[] charPic;
    public string[] charName;
    public string[] message;

    //Obj Area
    public Image charPicObj;
    public Text charNameObj;
    public Text messageObj;

    public GameObject DialogueObj;

    // Start is called before the first frame update
    void Start()
    { 
        currentImg = GetComponent<Image>();
        currentImgIndex = 0;
        currentImg.sprite = cutSceneArr[currentImgIndex];
        Time.timeScale = 0;
        player = GameObject.FindObjectOfType<Player_cube_control>();
        player.JumpForce = 0;
        player.gameObject.SetActive(false);
        CutSceneEnd = false;



        charPicObj.sprite = charPic[BoxIndex];
        charNameObj.text = charName[BoxIndex];
        messageObj.text = message[BoxIndex];
        DialogueObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            player.JumpCount = 2;
            player.isGrounded = false;
            player.JumpForce = 0;

            if (!CutSceneEnd)
            {
                if (currentImgIndex < cutSceneArr.Length - 1)
                {
                    player.isGrounded = false;
                    player.JumpForce = 0;
                    currentImgIndex += 1;
                    currentImg.sprite = cutSceneArr[currentImgIndex];
                }
                else
                {
                    currentImg.sprite = null;
                    currentImg.enabled = false;
                    DialogueObj.SetActive(true);
                    CutSceneEnd = true;
                }
            }
            else
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
}
