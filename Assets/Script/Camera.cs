using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Player;
    public float Height_Threshold;
    public Vector3 Normal_Offset;
    public Vector3 Current_Offset;
    public Vector3 High_Offset;

    public float SmoothDamp;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player.position.y >= Height_Threshold)
        {
            Current_Offset = High_Offset;
        } 
        else if (Player.position.y < Height_Threshold)
        {
            Current_Offset = Normal_Offset;
        }

        Vector3 TargetPosition = Player.position + Current_Offset;
        Vector3 SmoothedPosition = Vector3.Lerp(transform.position, TargetPosition, SmoothDamp);
        transform.position = SmoothedPosition;
        //transform.LookAt(Player);

        

        }
}

