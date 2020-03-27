using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform Player;
    public float Height_Threshold;
    public Vector3 Normal_Offset;
    public Vector3 Current_Offset;
    public Vector3 High_Offset;

    public bool isAtBoss = false;
    public int BossNumber;
    public Vector3 LV1BossCam;
    public Vector3 LV2BossCam;
    public Vector3 LV3BossCam;
    public float SmoothDamp;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAtBoss == false)
        {
            if (Player.position.y >= Height_Threshold)
            { Current_Offset = High_Offset; }
            else if (Player.position.y < Height_Threshold)
            { Current_Offset = Normal_Offset; }

            Vector3 TargetPosition = Player.position + Current_Offset;
            Vector3 SmoothedPosition = Vector3.Lerp(transform.position, TargetPosition, SmoothDamp);
            transform.position = SmoothedPosition;
        }
        else
        {
            if (BossNumber == 1)
            {
                Vector3 SmoothedPosition = Vector3.Lerp(transform.position, LV1BossCam, SmoothDamp);
                transform.position = SmoothedPosition;
            }
            else if (BossNumber == 2)
            {
                Vector3 SmoothedPosition = Vector3.Lerp(transform.position, LV2BossCam, SmoothDamp);
                transform.position = SmoothedPosition;
            }
            else if (BossNumber == 3)
            {
                Vector3 SmoothedPosition = Vector3.Lerp(transform.position, LV3BossCam, SmoothDamp);
                transform.position = SmoothedPosition;
            }

        }
    }
}

