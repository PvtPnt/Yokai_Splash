using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScirpt : MonoBehaviour
{
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
        {
            ButtonSprite.color = Color.red;
        }
        else { ButtonSprite.color = Color.white; }
    }

    void OnMouseEnter()
    { isSelected = true; }

    void OnMouseExit()
    { isSelected = false; }
}
