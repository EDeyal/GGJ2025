using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoSingleton<GridHandler>
{
    [SerializeField] GameObject floorGO;
    [SerializeField] public GridNode floorPrefab;
    [SerializeField] public int gridMaxX;
    [SerializeField] public int gridMaxY;
    void Start()
    {
        if (gridMaxX == 0)
        {
            gridMaxX = 10;
        }
        if (gridMaxY == 0)
        {
            gridMaxY = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnFloor()
    {
        for (int x = 0; x < gridMaxX; x++)
        {
            for (int y = 0; y < gridMaxY; y++)
            {
               GridNode gridNode = Instantiate(floorPrefab, new Vector3(x, 0, y),Quaternion.identity,floorGO.transform);
                gridNode.xPos = x;
                gridNode.yPos = y;
            }
        }
    }
}
