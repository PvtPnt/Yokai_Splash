using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletSpawner : MonoBehaviour
{
    public float Player_maxHP;
    public GameObject Tsuchinoko;
    public GameObject Fireball;
    public GameObject Onikuma;
    public GameObject Player;

    public GameObject SpawnEntity;

    public Transform Spawnpoint;
    public bool isOnTrigger;
    public bool isTsuchinoko;
    public bool isFireball;
    public bool isOnikuma;
    // Start is called before the first frame update
    void Start()
    {
        Player_maxHP = Player.GetComponent<Player_cube_control>().HP;

        if (isTsuchinoko == true)
        { SpawnEntity = Tsuchinoko; }

        else if (isFireball == true)
        { SpawnEntity = Fireball; }

        else if (isOnikuma == true)
        { SpawnEntity = Onikuma; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(SpawnEntity, Spawnpoint);
            Player.GetComponent<Player_cube_control>().HP = Player_maxHP;
        }
    }
}
