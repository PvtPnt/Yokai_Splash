using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Def_Bar : MonoBehaviour
{
    public Transform Bar;
    public GameObject Boss;
    [SerializeField] float Def;
    [SerializeField] float MaxDef;
    [SerializeField] float Def_Percentage;
    // Start is called before the first frame update
    void Start()
    {
        MaxDef = Boss.GetComponent<Enemy_hp>().Defense;
    }

    // Update is called once per frame
    void Update()
    {
        Def = Boss.GetComponent<Enemy_hp>().Defense;
        if (Def <= 0) { Def = 0; }
        Def_Percentage = Def / MaxDef;

        Bar.localScale = new Vector3(Def_Percentage, 1f);
    }
}
