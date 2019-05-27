using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newMovement : MonoBehaviour
{
    public int jumpCount = 0, diveCount = 0;
    public bool isFloating = false, isStanding = false, canJump = false, isJumping = false, canDive = false;
    private float gravity = 30, movementSpeed = 2;
    public float underwater;
    private string axis = "Vertical", axis2 = "Horizontal";
    
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        underwater = gameObject.transform.position.y / System.Math.Abs(gameObject.transform.position.y);
        isFloating = gameObject.transform.position.y > -0.05f && gameObject.transform.position.y < 0.05f;
        if(isFloating)
        {
            isJumping = false;
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * gravity * underwater);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isStanding = collision.contacts[0].normal[1] == underwater;
        isJumping = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isStanding = false;
    }

    private void Update()
    {
        canJump = isFloating || (isStanding && underwater == 1);
        canDive = isFloating || (isStanding && underwater == -1);

        //control vertical movement
        float v = Input.GetAxisRaw(axis) * 40;
        //control horizontal
        float h = Input.GetAxisRaw(axis2);

        

        //vertical movement
        if (canJump)
        {
            
            if(v>0 && Input.anyKeyDown)
            {

                isJumping = true;
                jumpCount++;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, v) * 15);
                Debug.Log("jumping");
            }

        }
        if(canDive)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, v) * 15);
            if (v < 0 && Input.anyKeyDown)
            {
                diveCount++;
            }
        }

        //horizontal movement
        GetComponent<Rigidbody2D>().velocity = new Vector2(h, 0) * movementSpeed;
    }
}
