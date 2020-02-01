using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Bar : MonoBehaviour
{
    public Transform W_Bar;
    [SerializeField] float Water;
    [SerializeField] float MaxWater;
    [SerializeField] float Water_Percentage;
    // Start is called before the first frame update
    void Start()
    {
        MaxWater = GameObject.Find("Player").GetComponent<Player_cube_control>().MaxWater;
    }

    // Update is called once per frame
    void Update()
    {
        Water = GameObject.Find("Player").GetComponent<Player_cube_control>().Water;
        Water_Percentage = Water / MaxWater;

        W_Bar.localScale = new Vector3(Water_Percentage, 1f);
    }
}
