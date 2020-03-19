using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject MainCamera;
    Camera MainCamInst;

    public GameObject[] enemy;
    public Enemy_basic[] enemyObj;

    bool isEnemyVisible;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindWithTag("MainCamera");
        MainCamInst = MainCamera.GetComponent<Camera>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        getEnemyObjComponent();

    }

    // Update is called once per frame
    void Update()
    {
       // getEnemyObjComponent();
        print(MainCamInst.pixelHeight);
        print(MainCamInst.pixelWidth);
        for(int i = 0; i < enemyObj.Length; i++)
        {
            if(enemyObj[i] != null)
            {
                if (checkInRange(enemyObj[i].transform.position))
                {

                    enemyObj[i].isWalk = true;
                }
                else
                {
                    enemyObj[i].isWalk = false;
                }
            }
            else
            {
                continue;
            }
           
        }
       
        /*if (checkInRange(enemy[].transform.position))
        {
            print("Detected Enemy");
        }
        else
        {
            print("O.B.");
        }*/
    }

    bool checkInRange(Vector3 objToCheck)
    {
            Vector3 viewPos = MainCamInst.WorldToViewportPoint(objToCheck);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                //In Range
                return true;
            }
            else
            {
                return false;
            }
    }

    void getEnemyObjComponent()
    {
        for(int i = 0; i < enemy.Length; i++)
        {
            enemyObj[i] = enemy[i].gameObject.GetComponent<Enemy_basic>();
        }
    }
}
