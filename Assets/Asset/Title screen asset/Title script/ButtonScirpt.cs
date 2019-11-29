using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScirpt : MonoBehaviour
{
    public SpriteRenderer ButtonSprite;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {ButtonSprite.color = Color.red;}

    void OnMouseExit()
    {ButtonSprite.color = Color.white;}
}
