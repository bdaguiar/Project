using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class mapBuilder : MonoBehaviour
{
    [Range(0, 100)]
    public int iniChance;

    [Range(0, 8)]
    public int birthLimit;

    [Range(0, 8)]
    public int deathLimit;


    [Range(1, 10)]
    public int numR;

    public int[,] terrainMap;
    public Vector3Int tMapSize;
    public Tilemap topMap, bottomMap;
    public Tile topTile, bottomTile;

    int width, height;

    private void Awake()
    {
        doSim(numR);
    }

    public void doSim(int NumR)
    {
        clearMaps(false);
        width = tMapSize.x;
        height = tMapSize.y;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();
        }

        for(int i = 0; i < numR; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(terrainMap[i,j] ==1)
                {
                    topMap.SetTile(new Vector3Int(-i + width / 2, -j + height / 2, 0), topTile);
                    //bottomMap.SetTile(new Vector3Int(-i + width / 2, -j + height / 2, 0), bottomTile);
                }
            }
        }
    }


    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighbours;
        BoundsInt myB = new BoundsInt(-1,-1,0,3,3,1);

        for(int i = 0; i<width;i++)
        {
            for (int j = 0; j<height;j++)
            {
                neighbours = 0;
                foreach(var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (i + b.x >= 0 && i + b.x < width && j + b.y >= 0 && j + b.y < height)
                    {
                        neighbours += oldMap[i + b.x, j + b.y];
                    }
                    else
                    {
                        neighbours++;
                    }
                }
                if(oldMap[i,j] == 1)
                {
                    if (neighbours < deathLimit) newMap[i, j] = 0;
                    else newMap[i, j] = 1;
                }
                if (oldMap[i, j] == 0)
                {
                    if (neighbours >birthLimit) newMap[i, j] = 1;
                    else newMap[i, j] = 0;
                }
            }
        }


        return newMap;
    }

    public void initPos()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j<height; j++)
            {
                terrainMap[i, j] = Random.Range(1, 101) < iniChance ? 1 : 0;
            }
        }
    }

    public void clearMaps(bool complete)
    {
        topMap.ClearAllTiles();
        bottomMap.ClearAllTiles();
        if(complete)
        {
            terrainMap = null;
        }
    }


}
