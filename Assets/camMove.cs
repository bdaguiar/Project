using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMove : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(GameObject.Find("character").transform.position.x, GameObject.Find("character").transform.position.y, -10f);
    }

    
}
