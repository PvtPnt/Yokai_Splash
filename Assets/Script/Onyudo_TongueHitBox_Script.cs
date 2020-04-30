using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onyudo_TongueHitBox_Script : MonoBehaviour
{
    public bool isRock;
    public bool isImpaler;
    public int Damage;
    private Vector3 StartingPos;
    public float Impaler_speed;
    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isImpaler)
        {
            float step = Impaler_speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, StartingPos + new Vector3(0f,6.5f,0f), step);

            if(transform.position == StartingPos + new Vector3(0f, 6.5f, 0f))
            {
                StartCoroutine("DestroyImpaler");
            }
        }
    }

    IEnumerator DestroyImpaler()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        { 
            other.GetComponent<Player_cube_control>().P_ReceiveDamage(Damage); 
            if (isRock) { Destroy(this.gameObject); }
        }

        if (other.tag == "Ground" && !isImpaler)
        { Destroy(this.gameObject); }
    }
}
