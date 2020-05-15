using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_DrainHPCheat : MonoBehaviour
{
    [SerializeField] float TriggerValue;
    Enemy_hp Boss_HP_Script;
    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Script = GetComponent<Enemy_hp>();
    }

    // Update is called once per frame
    void Update()
    {
        TriggerValue = Input.GetAxis("LT");
        Boss_HP_Script.HP -= Mathf.RoundToInt(TriggerValue);
    }
}
