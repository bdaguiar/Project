﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyTurn : MonoBehaviour
{
    private double timer;
    private float turnAround = 1;
    private bool isTurned = false;
    public float turnOverX, turnOverY, turnOverRadius, turnOverDistance, underwater;
    public bool turnOver = false;

    private void Start()
    {
        if (gameObject.transform.position.y < 0)
        {
            GetComponent<Rigidbody2D>().gravityScale = -1;
            transform.localScale = new Vector3(1, -1);
        }
    }

    private void FixedUpdate()
    {
        underwater = gameObject.transform.position.y / System.Math.Abs(gameObject.transform.position.y);

        if (!isGrounded())
        {
            turnAround *= -1;
        }
        turnOverDistance = Mathf.Sqrt(Mathf.Pow(turnOverX - gameObject.transform.position.x, 2) + Mathf.Pow(turnOverY - gameObject.transform.position.y, 2));
        if (turnOver && turnOverDistance<turnOverRadius)
        {
            if (!isTurned)
            {
                timer = 0.0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().transform.Rotate(0, 0, 180);
                isTurned = true;
            }
            else
            {
                timer += Time.fixedDeltaTime;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * turnAround;
            turnOver = false;
        }
        if(timer>10 && isTurned)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * turnAround;
            GetComponent<Rigidbody2D>().transform.Rotate(0, 0, 180);
            turnOver = false;
            isTurned = false;
        }
        

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "character")
        {
            if (isTurned && collision.contacts[0].normal[1] == -1 * underwater)
            {
                GameObject.FindObjectOfType<TimerScript>().kills += 1;
                Destroy(gameObject);
            }
            else
            {
                GameObject.Find("character").GetComponent<charMove>().saveScores();
                SceneManager.LoadScene("ScoreBoard");
            }
        }
        if (collision.contacts[0].normal[0] != 0)
        {
            turnAround *= -1;
        }
    }



    public bool isGrounded()
    {
        // Get Bounds and Cast Range (extents.y+20%)
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float range = bounds.extents.y * 2f * underwater;
        // Calculate a position slightly 'below' the collider
        // (via transform.up so it's gravity tolerant)
        Vector2 v = bounds.center - transform.up * range;

        // Linecast upwards
        RaycastHit2D hit = Physics2D.Linecast(v, bounds.center);
        // Was there something in-between, or did we hit ourself?
        return (hit.collider.gameObject != gameObject);
    }
}
