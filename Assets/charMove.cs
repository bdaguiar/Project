using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class charMove : MonoBehaviour
{
    public Sprite pJump, gJump, sJump, sDive, gDive, pDive, pSmash, gSmash, sSmash, sSlide, gSlide, pSlide;
    private Vector3 fMove, lMove, fAct, lAct;
    public int jumpCount = 0, diveCount = 0, smashCount = 0, slideCount = 0;
    public bool isFloating = false, isStanding = false, canJump = false, isJumping = false, canDive = false, canSmash = false, hasSmashed = false, canSlide = false, mightSlide = false, hasSlided = false;
    private float gravity = 5, movementSpeed = 2, jumpPower, divePower, smashPower, slidePower; //slide power might change to time
    public float underwater, highestPos = 0, deepestPos = 0, highestVerticalDelta = 0, thisDeepest = 0, thisHighest = 0, thisDelta = 0;

    private void Start()
    {
        int[,] terrain = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().terrainMap;
        int range = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().tMapSize.x;
        int middle = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().tMapSize.y / 2;
        for (int i =0; i < range; i++)
        {
            if (terrain[i, middle] == 0)
            {
                gameObject.transform.position = new Vector3((-i + range/2) * 0.2f + 0.1f, 0.1f);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        underwater = gameObject.transform.position.y / System.Math.Abs(gameObject.transform.position.y);
        isFloating = gameObject.transform.position.y > -0.05f && gameObject.transform.position.y < 0.05f;
        if (isFloating)
        {
            isJumping = false;
        }
        if (isGrounded() && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) <= movementSpeed)
        {
            hasSlided = false;
        }
       
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * gravity * underwater);
       


        //Record storing
        if(gameObject.transform.position.y > highestPos)
        {
            highestPos = gameObject.transform.position.y;
        }
        if(gameObject.transform.position.y < deepestPos)
        {
            deepestPos = gameObject.transform.position.y;
        }
        if(isStanding)
        {
            thisDeepest = 0;
            thisHighest = 0;
            thisDelta = 0;
        }
        else
        {
            if(gameObject.transform.position.y > thisHighest)
            {
                thisHighest = gameObject.transform.position.y;
            }
            if(gameObject.transform.position.y < thisDeepest)
            {
                thisDeepest = gameObject.transform.position.y;
            }
            thisDelta = thisHighest - thisDeepest;
        }
        if(thisDelta>highestVerticalDelta)
        {
            highestVerticalDelta = thisDelta;
        }



        //atribute development
        improveJump();
        improveDive();
        improveSmash();
        improveSlide();
    }


    //Character control
    private void Update()
    {
        canJump = isFloating || (isStanding && underwater == 1);
        canDive = isFloating || (isStanding && underwater == -1);
        canSmash = !isFloating && !isStanding && !hasSmashed;
        if(underwater == -1)
        {
            mightSlide = true;
        }
        else if(isStanding)
        {
            mightSlide = false;
        }
        canSlide = mightSlide && underwater == 1;

        GetComponent<Animator>().SetBool("isGrounded", isStanding);
        GetComponent<Animator>().SetBool("isSmashing", hasSmashed);
        GetComponent<Animator>().SetBool("isSliding", hasSlided);




        //input handling
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(i).position.x < Screen.width / 2) //left side, It's a movement (right/left only)
                {
                    //Identify start and end of swipe
                    fMove = Input.GetTouch(i).rawPosition;
                    lMove = Input.GetTouch(i).position;


                    //Swipe is bigger than 5% of screen
                    if (Mathf.Abs(fMove.x - lMove.x) > Screen.width / 20)
                    {
                        //Identify direction
                        if (fMove.x > lMove.x)
                        {
                            //movement left
                            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
                            gameObject.transform.localScale = new Vector3(1, underwater);
                            GetComponent<Animator>().SetBool("isMoving", true);
                        }
                        else
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
                            gameObject.transform.localScale = new Vector3(-1, underwater);
                            GetComponent<Animator>().SetBool("isMoving", true);
                        }
                    }
                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        GetComponent<Animator>().SetBool("isMoving", false);

                    }
                }
                else //right side, It's an action
                {
                    fAct = Input.GetTouch(i).rawPosition;
                    lAct = Input.GetTouch(i).position;

                    //Swipe is bigger than 5% horizontal or 10% vertical
                    if (Mathf.Abs(fAct.x - lAct.x) > Screen.width / 20 || Mathf.Abs(fAct.y - lAct.y) > Screen.width / 10)
                    {
                        //Identify orientation
                        if (Mathf.Abs(fAct.x - lAct.x) >= Mathf.Abs(fAct.y - lAct.y))  //horizontal swipe
                        {
                            //Identify direction
                            if (fAct.x > lAct.x)
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    //action left
                                    if(canSlide && !hasSlided)
                                    {
                                        slideCount++;
                                        hasSlided = true;
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.left * slidePower);
                                    }
                                }
                            }
                            else
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    //action right
                                    
                                    if (canSlide && !hasSlided)
                                    {
                                        slideCount++;
                                        hasSlided = true;
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.right * slidePower);
                                    }
                                }
                            }
                        }
                        else //horizontal swipe
                        {
                            //Identify direction
                            if (fAct.y > lAct.y)
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    //action down
                                    if (canDive)
                                    {
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.down * divePower);
                                        isStanding = false;
                                        diveCount++;
                                    }
                                    else if (canSmash && underwater == 1)
                                    {
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.down * smashPower);
                                        smashCount++;
                                        hasSmashed = true;
                                    }
                                    
                                }
                            }
                            else
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                                {
                                    //action up
                                    if(canJump)
                                    {
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
                                        isStanding = false;
                                        jumpCount++;
                                    }
                                    else if (canSmash && underwater == -1)
                                    {
                                        GetComponent<Rigidbody2D>().AddForce(Vector2.up * smashPower);
                                        hasSmashed = true;
                                        smashCount++;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }




    private void OnCollisionStay2D(Collision2D collision)
    {
        isStanding = isGrounded();
        if(isStanding)
        {
            hasSmashed = false;
        }
        isJumping = !isStanding;
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        isStanding = isGrounded();
        if (isStanding)
        {
            hasSmashed = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasSmashed && collision.contacts[0].normal[1] == underwater && GameObject.FindGameObjectWithTag("turnover"))
        {
            GameObject[] turnovers = GameObject.FindGameObjectsWithTag("turnover");
            for (int i = 0; i < turnovers.Length; i++)
            {
                turnovers[i].GetComponent<enemyTurn>().turnOver = true;
                turnovers[i].GetComponent<enemyTurn>().turnOverX = gameObject.transform.position.x;
                turnovers[i].GetComponent<enemyTurn>().turnOverY = gameObject.transform.position.y;
                turnovers[i].GetComponent<enemyTurn>().turnOverRadius = 0.005f * smashPower;
            }
            
        }
        hasSmashed = false;
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



    private void improveJump()
    {
        if (jumpCount > 360)
        {
            jumpPower = 240;
        }
        else if (jumpCount > 265)
        {
            GameObject.Find("jumpImage").GetComponent<Image>().sprite = pJump;
            jumpPower = 220;
        }
        else if (jumpCount > 210)
        {
            
            jumpPower = 200;
        }
        else if (jumpCount > 160)
        {
            jumpPower = 180;
        }
        else if (jumpCount > 120)
        {
            GameObject.Find("jumpImage").GetComponent<Image>().sprite = gJump;
            jumpPower = 160;
        }
        else if (jumpCount > 80)
        {
            jumpPower = 140;
        }
        else if (jumpCount > 50)
        {
            jumpPower = 130;
        }
        else if (jumpCount > 25)
        {
            GameObject.Find("jumpImage").GetComponent<Image>().sprite = sJump;
            jumpPower = 120;
        }
        else if (jumpCount > 10)
        {
            jumpPower = 110;
        }
        else
        {
            jumpPower = 90;
        }

    }

    private void improveDive()
    {
        if (diveCount > 460)
        {
            divePower = 324;
        }
        else if (diveCount > 400)
        {
            GameObject.Find("diveImage").GetComponent<Image>().sprite = pDive;
            divePower = 304.8f;
        }
        else if (diveCount > 350)
        {
            divePower = 285.6f;
        }
        else if (diveCount > 300)
        {
            divePower = 266.4f;
        }
        else if (diveCount > 250)
        {
            
            divePower = 247.2f;
        }
        else if (diveCount > 205)
        {
            GameObject.Find("diveImage").GetComponent<Image>().sprite = gDive;
            divePower = 228;
        }
        else if (diveCount > 160)
        {
            divePower = 208.8f;
        }
        else if (diveCount > 130)
        {
            
            divePower = 189.6f;
        }
        else if (diveCount > 105)
        {
            divePower = 170.4f;
        }
        else if (diveCount > 85)
        {
            GameObject.Find("diveImage").GetComponent<Image>().sprite = sDive;
            divePower = 151.2f;
        }
        else if (diveCount > 67)
        {
            
            divePower = 132;
        }
        else if (diveCount > 52)
        {
            divePower = 120;
        }
        else if (diveCount > 38)
        {
            divePower = 110;
        }
        else if (diveCount > 22)
        {
            GameObject.Find("diveImage").GetComponent<Image>().sprite = sDive;
            divePower = 100f;
        }
        else if (diveCount > 10)
        {
            divePower = 90f;
        }
        else
        {
            divePower = 80;
        }
    }

    private void improveSmash()
    {
        if(smashCount > 105)
        {
            smashPower = 400;
        }
        else if(smashCount > 81)
        {
            GameObject.Find("smashImage").GetComponent<Image>().sprite = pSmash;
            smashPower = 350f;
        }
        else if (smashCount > 60)
        {
            smashPower = 300f;
        }
        else if (smashCount > 42)
        {
            GameObject.Find("smashImage").GetComponent<Image>().sprite = gSmash;
            smashPower = 250f;
        }
        else if (smashCount > 27)
        {
            smashPower = 200f;
        }
        else if (smashCount > 15)
        {
            GameObject.Find("smashImage").GetComponent<Image>().sprite = sSmash;
            smashPower = 150f;
        }
        else if (smashCount > 6)
        {
            smashPower = 100f;
        }
        else
        {
            smashPower = 50f;
        }

    }

    private void improveSlide()
    {
        if (slideCount > 105)
        {
            slidePower = 400;
        }
        else if (slideCount > 81)
        {
            GameObject.Find("slideImage").GetComponent<Image>().sprite = pSlide;
            slidePower = 350f;
        }
        else if (slideCount > 60)
        {
            slidePower = 300f;
        }
        else if (slideCount > 42)
        {
            GameObject.Find("slideImage").GetComponent<Image>().sprite = gSlide;
            slidePower = 250f;
        }
        else if (slideCount > 27)
        {
            slidePower = 200f;
        }
        else if (slideCount > 15)
        {
            GameObject.Find("slideImage").GetComponent<Image>().sprite = sSlide;
            slidePower = 150f;
        }
        else if (slideCount > 6)
        {
            slidePower = 100f;
        }
        else
        {
            slidePower = 50f;
        }

    }


    public void saveScores()
    {
        PlayerPrefs.SetFloat("timer", (float)GameObject.Find("Kills").GetComponent<TimerScript>().timer);
        PlayerPrefs.SetFloat("high", highestPos);
        PlayerPrefs.SetFloat("deep", deepestPos);
        PlayerPrefs.SetFloat("delta", highestVerticalDelta);
        PlayerPrefs.SetInt("jump", jumpCount);
        PlayerPrefs.SetInt("dive", diveCount);
        PlayerPrefs.SetInt("smash", smashCount);
        PlayerPrefs.SetInt("slide", slideCount);
        PlayerPrefs.SetInt("kills", GameObject.Find("Kills").GetComponent<TimerScript>().kills);
        PlayerPrefs.SetInt("fromGame", 1);
    }


    

}
