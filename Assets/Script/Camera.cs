using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    public float SmoothDamp = 0.125f;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 TargetPosition = Player.position + Offset;
        Vector3 SmoothedPosition = Vector3.Lerp(transform.position, TargetPosition, SmoothDamp);
        transform.position = SmoothedPosition;

        //transform.LookAt(Player);
    }
}
