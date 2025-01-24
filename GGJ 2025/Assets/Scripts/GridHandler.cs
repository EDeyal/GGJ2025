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
    //public List<Node> GetNodesOneTileAway(Vector3 playerPosition, List<Node> allNodes)
    //{
    //    List<Node> nodesOneTileAway = new List<Node>();

    //    foreach (Node node in allNodes)
    //    {
    //        // Calculate the distance between the player's position and the node
    //        float distance = Vector3.Distance(playerPosition, node.position);

    //        // Check if the distance is exactly 1 tile (adjust for grid-based calculations)
    //        if (Mathf.Approximately(distance, 1f))
    //        {
    //            nodesOneTileAway.Add(node);
    //        }
    //    }

    //    return nodesOneTileAway;
    //}
}
