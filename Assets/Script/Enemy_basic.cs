using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public int HP;

    public float WalkSpeed;
    public float groundCheckRange = 1f;
    public float wallCheckRange = 1f;
    public float EnemyCheckRange = 10f;

    public bool IsWalkingLeft;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Vector2 RayOffset = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) { Destroy(gameObject); }
        Walking();
    }

    public void ReceiveDamage(int Damage)
    {
        HP -= Damage;
        Debug.Log("Damage taken");
    }

    void Walking()
    {
        if (IsWalkingLeft == true)
        {
            CheckWall(-Vector2.right);
            Debug.DrawRay(transform.position, -Vector2.up * groundCheckRange, Color.red);
            if (Physics2D.Raycast(transform.position, -Vector2.up, groundCheckRange, groundLayer))
            {
                //Walk Left, Check hole on left
                //EnemySprite.flipX = false;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * -WalkSpeed * Time.deltaTime);
            }
            else
            {
                IsWalkingLeft = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            CheckWall(Vector2.right);
            Debug.DrawRay(transform.position, -Vector2.up * groundCheckRange, Color.red);
            if (Physics2D.Raycast(transform.position, -Vector2.up, groundCheckRange, groundLayer))
            {
                //Walk Right, Check hole on right
                //EnemySprite.flipX = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * WalkSpeed * Time.deltaTime);
            }
            else
            {
                IsWalkingLeft = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    void CheckWall(Vector2 direction)
    {
        Debug.DrawRay(transform.position, direction * wallCheckRange, Color.cyan);
        if (Physics2D.Raycast(transform.position, direction, wallCheckRange, wallLayer))
        {
            if (IsWalkingLeft)
            {
                IsWalkingLeft = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                IsWalkingLeft = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
