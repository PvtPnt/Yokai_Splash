using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Player;
    public float ZOffset;
    public float YOffset;
    public float XOffset;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player)
        {
            ZOffset = transform.position.z - Player.position.z;
            YOffset = 3.5f;
            XOffset = 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 TargetPosition;
        TargetPosition = Player.transform.position + Vector3.forward * ZOffset + Vector3.up * YOffset;
        Vector3 Velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, 0.025f);
    }
}
