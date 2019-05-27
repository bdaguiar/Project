using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapConstruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (gameObject.name == "tile")
        {
            int n = 0;
            for (int i = -17; i < 20; i++)
            {
                for (int j = -19; j < 18; j++)
                {
                    if (Random.Range(1, 3) % 2 == 0)
                    {
                        GameObject newTile = Instantiate(gameObject, new Vector3((float)j / 4f, (float)i / 4f), gameObject.transform.rotation);
                        newTile.name = "newTile";
                        n++;
                    }
                    if (n > 17)
                    {
                        break;
                    }

                }
                n = 0;
            }
        }
    }
}
