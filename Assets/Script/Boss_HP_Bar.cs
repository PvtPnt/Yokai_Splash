using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Bar : MonoBehaviour
{
    public Transform Bar;
    public GameObject Boss;
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] float HP_Percentage;
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = Boss.GetComponent<Enemy_hp>().HP;
    }

    // Update is called once per frame
    void Update()
    {
        HP = Boss.GetComponent<Enemy_hp>().HP;
        HP_Percentage = HP / MaxHP;

        Bar.localScale = new Vector3(HP_Percentage, 1f);
    }
}
