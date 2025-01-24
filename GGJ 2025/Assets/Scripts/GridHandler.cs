using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] GameObject floorGO;
    [SerializeField] public GridNode floorPrefab;
    [SerializeField] public int gridMaxX;
    [SerializeField] public int gridMaxY;
    public GridNode[,] grid;
    public List<GridNode> enemySpawnablePositions;
    void Awake()
    {
        if (gridMaxX == 0)
        {
            gridMaxX = 10;
        }
        if (gridMaxY == 0)
        {
            gridMaxY = 10;
        }
        grid = new GridNode[gridMaxX, gridMaxY];
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
                grid[x, y] = gridNode;
            }
        }
    }

    public bool CheckIsNodeOccupied(int x, int y)
    {
        if (x < 0 || x >= gridMaxX || y < 0 || y >= gridMaxY)
        {
            //hit wall
            return true;
        }
       return grid[x, y].isOccupied;
    }
    public bool SetIsNodeOccupied(int x, int y, bool isOccupied)
    {
        //Debug.Log("Node " + x + y + " is occupied " + isOccupied);
        return grid[x, y].isOccupied = isOccupied;
    }

    public void UpdateEnemySpawnPositions()
    {
        foreach (var node in grid)
        {
            if (node.xPos == gridMaxX - 1 || node.yPos == gridMaxY - 1)
            {
                if (!enemySpawnablePositions.Contains(node))
                { 
                    enemySpawnablePositions.Add(node);
                }
            }
        }
    }
    public GridNode GetSpawnableEnemyLocation()
    {
        int randomIndex = Random.Range(0, enemySpawnablePositions.Count);
        if (enemySpawnablePositions[randomIndex].isOccupied)
        {
            return GetSpawnableEnemyLocation();
        }
        return enemySpawnablePositions[randomIndex];
    }
}
