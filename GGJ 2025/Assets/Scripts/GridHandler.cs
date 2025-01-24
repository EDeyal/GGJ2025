using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public void IncreaseGridSize(int gridX, int gridY)
    {
        gridMaxX += gridX;
        gridMaxY += gridY;
        UnspawnFloor();
        grid = new GridNode[gridMaxX, gridMaxY];
        SpawnFloor();
        UpdateEnemySpawnPositions();
    }
    void UnspawnFloor()
    {
        foreach (var tile in grid)
        {
            Destroy(tile.gameObject);
        }
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
        enemySpawnablePositions.Clear();
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
    public void ShowAbilityRange(int xPos, int yPos,int abilityRange)
    {
        List<GridNode> nodesInRange = new List<GridNode>();

        foreach (var node in grid)
        {
            float distance = Vector3.Distance(new Vector2(xPos, yPos), new Vector2(node.transform.position.x,node.transform.position.z));
            if (Mathf.Approximately(distance, 1f))
            {
                nodesInRange.Add(node);
            }
        }

        foreach (var node in nodesInRange)
        {
            node.HighlightFloor(true, Color.red);
        }
    }
    public void UnshowAbilityRange()
    {
        foreach (var node in grid)
        {
            node.HighlightFloor(false, Color.gray);
        }
    }
    public void UnoccupyTile(Vector2 tilePos)
    {
        foreach (var node in grid)
        {
            if (tilePos.x == node.xPos && tilePos.y == node.yPos)
            {
                node.isOccupied = false;
            }
        }
    }
}
