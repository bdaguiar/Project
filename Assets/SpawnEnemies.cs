using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


public class SpawnEnemies : MonoBehaviour
{
    public GameObject rEnemy, tOEnemy, smEnemy, slEnemy;

    private GameObject[] enemies;

    private Tilemap tMap;

    private Vector3Int tileBuilder;

    public int quantity, counter = 0;

    public int[,] terrain;

    private void Start()
    {
        enemies = new GameObject[] { rEnemy, tOEnemy, slEnemy, smEnemy };
        terrain = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().terrainMap;
        tMap = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().topMap;
        tileBuilder = GameObject.Find("tileBuilder").GetComponent<mapBuilder>().tMapSize;
        while (counter < quantity)
        {
            for (int i = 1; i < tileBuilder.x - 1; i++)
            {
                for (int j = 1; j < tileBuilder.y - 1; j++)
                {
                    int shouldSpawn = Random.Range(1, 51);
                    int whatToSpawn = Random.Range(0, 4);
                    if (isSpawnable(i, j) && shouldSpawn < 5)
                    {
                        Instantiate(enemies[whatToSpawn], new Vector3((-i + tileBuilder.x / 2) * 0.2f + 0.1f, (-j + tileBuilder.y / 2) * 0.2f + 0.1f, 0), Quaternion.identity);
                        counter++;
                    }
                    if (counter > quantity) break;
                }
            }
        }
    }

    private bool isSpawnable(int x, int y)
    {
        bool stillTrue = true;
        for (int i = -1; i < 1; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ((y < tileBuilder.y / 2 && j == 1) || (y >= tileBuilder.y / 2 && j == -1))
                {
                    stillTrue = stillTrue && terrain[x + i, y + j] == 1;
                }
                else
                {
                    stillTrue = stillTrue && terrain[x + i, y + j] == 0;
                }
            }
        }
        return stillTrue;
    }
}
