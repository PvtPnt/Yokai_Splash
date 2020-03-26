using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnCondition : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject Dialogue;
    public GameObject canvas;

    public Camera MainCamera;

    [SerializeField]
    int numOfenemy;
    // Start is called before the first frame update
    void Start()
    {
        numOfenemy = enemy.Length;
    }

    // Update is called once per frame
    void Update()
    {
        checkRemainingEnemy();
        if(numOfenemy == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Instantiate(Dialogue,canvas.transform);
        MainCamera.fieldOfView = 60;
        
    }

    void checkRemainingEnemy()
    {
        int temp = 0;
        for (int i = enemy.Length - 1; i >= 0; i--)
        {

            if(enemy[i] != null)
            {
                temp += 1;
            }
            else
            {
                temp += 0;
            }

        }
        numOfenemy = temp;
    }
}
