using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_mover : MonoBehaviour
{
    public bool isBoss;
    public bool isStage;

    bool Stage_moving;
    public GameObject Boss;
    public GameObject To_move;
    public Vector3 Destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Stage_moving)
        { To_move.transform.position = Vector3.MoveTowards(To_move.transform.position, Destination, 1.5f); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isBoss)
        {Boss.GetComponent<Baku_Script>().enabled = true;}

        if (other.tag == "Player" && isStage)
        { Stage_moving = true; }
    }
}
