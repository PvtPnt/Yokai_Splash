using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public float InputValue;
    public int MenuIndex;
    public int MaxButton;

    public bool isHoldingLeftStick;
    public bool isBack;
    public bool isStart;
    public bool isHowToPlay;
    public bool isCredit;
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
        { ButtonSprite.color = new Color(0,0.95f,1,1); }
        else { ButtonSprite.color = Color.white; }

        if (MenuIndex > MaxButton) { MenuIndex = MaxButton; }
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
            if (isBack) { SceneManager.LoadScene("Title scene"); }
            if (isStart) { SceneManager.LoadScene("level 1"); }
            if (isCredit) { SceneManager.LoadScene("Credits"); }
            if (isHowToPlay) { SceneManager.LoadScene("How2Play"); }
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

        if (isBack) { SceneManager.LoadScene("Title scene"); }
        if (isCredit) { SceneManager.LoadScene("Credits"); }
        if (isHowToPlay) { SceneManager.LoadScene("How2Play"); }

        if (isQuit)
        {Application.Quit();}
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("proto1");
    }
}
