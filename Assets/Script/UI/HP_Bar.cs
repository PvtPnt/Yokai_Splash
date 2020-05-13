using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    public Transform Bar;
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] float HP_Percentage;
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = GameObject.Find("Player").GetComponent<Player_cube_control>().HP;
    }

    // Update is called once per frame
    void Update()
    {
        HP = GameObject.Find("Player").GetComponent<Player_cube_control>().HP;
        if (HP<=0) { HP = 0; }
        HP_Percentage = HP / MaxHP;

        Bar.localScale = new Vector3(HP_Percentage, 1f);
    }
}
