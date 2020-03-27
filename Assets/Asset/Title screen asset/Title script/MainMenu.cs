using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public float InputValue;
    public int MenuIndex;

    public bool isHoldingLeftStick;
    public bool isStart;
    public bool isQuit;

    public SpriteRenderer ButtonSprite;
    public bool isSelected;
    public int ButtonNumber;
    // Start is called before the first frame update
    void Start()
    {
        ButtonSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        { ButtonSprite.color = Color.red; }
        else { ButtonSprite.color = Color.white; }

        if (MenuIndex > 2) { MenuIndex = 2; }
        else if (MenuIndex < -1) { MenuIndex = -1; }

        InputValue = Input.GetAxisRaw("Vertical");
        if (InputValue != 0)
        {
            if(isHoldingLeftStick == false)
            {
                if(InputValue > 0f) { MenuIndex -= 1; }
                else if (InputValue < 0f) { MenuIndex += 1; }
                isHoldingLeftStick = true;
            }
        }

        if (MenuIndex == ButtonNumber)
        { isSelected = true; }
        else { isSelected = false; }

        if (InputValue == 0) { isHoldingLeftStick = false; }

        if (isSelected == true && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (isStart) { SceneManager.LoadScene("level 1"); }
            if (isQuit) { Application.Quit(); }
        }



    }

    void OnMouseEnter()
    { 
        isSelected = true;
        MenuIndex = ButtonNumber;
    }

    void OnMouseExit()
    { 
        isSelected = false;
        MenuIndex = -1;
    }

    void OnMouseUp()
    {
        if (isStart)
        {SceneManager.LoadScene("level 1");}
        if (isQuit)
        {Application.Quit();}
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("proto1");
    }
}
