using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite[] cutSceneArr;
    public int currentImgIndex;
    public Image currentImg;
    public Player_cube_control player;
    void Start()
    {
        currentImg = GetComponent<Image>();
        currentImgIndex = 0;
        currentImg.sprite = cutSceneArr[currentImgIndex];
        Time.timeScale = 0;
        player = GameObject.FindObjectOfType<Player_cube_control>();
        player.JumpForce = 0;
        player.gameObject.GetComponentInChildren<AudioSource>().enabled = false;
        //player.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            player.JumpCount = 2;
            player.isGrounded = false;
            player.JumpForce = 0;
            if (currentImgIndex < cutSceneArr.Length-1)
            {
                player.isGrounded = false;
                player.JumpForce = 0;
                currentImgIndex += 1;
                currentImg.sprite = cutSceneArr[currentImgIndex];
            }
            else
            {
                player.gameObject.GetComponentInChildren<AudioSource>().enabled = true;
                player.isGrounded = true;
                Time.timeScale = 1;
                //player.gameObject.SetActive(true);
                Destroy(this.gameObject);
                player.JumpForce = 300;
            }

        }
    }
}
