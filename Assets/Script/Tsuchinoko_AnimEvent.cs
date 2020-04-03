using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsuchinoko_AnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private Enemy_basic Tsuchinoko;
    void Start()
    {
        Tsuchinoko = GetComponent<Enemy_basic>();
    }

    // Update is called once per frame
    void StartAttackCollider()
    {
        Tsuchinoko.isAttacking = true;
    }

    void EndAttackCollider()
    {
        Tsuchinoko.isAttacking = false;
    }
}
