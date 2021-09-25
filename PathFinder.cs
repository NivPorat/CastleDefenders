using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }


    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {

        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates]; 
            //start and end tiles are walkable for enemies 
            //but not placeable for towers
            
        }
    }
    void Start()
    {
      
        GetNewPath();
    }
    //get new path from start node
    public List<Node> GetNewPath()
    { 
        return GetNewPath(startCoordinates);
    }

    //get new path from current enemy position
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        breadthFirstSearch(coordinates);
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        //loop through 4 directions to find neighbor tiles
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(grid[neighborCoordinates]);

            }
        }
        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }
    void breadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        //put the current grind node in Queue and use it in pathfinding
        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        { 
            currentSearchNode = frontier.Dequeue();//remove start node 
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates)//break
            {
                isRunning = false;
            }
        }
    }

    //build a path backwards from end point to enemy start point
    List<Node> BuildPath()
    {
        List<Node> Path = new List<Node>();//return this list
        Node currentNode = destinationNode;//first node is the end node and we run it back

        Path.Add(currentNode);
        currentNode.isPath = true;

        //like a reverse linked list
        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            Path.Add(currentNode);
            currentNode.isPath = true;
        }
        //reverse the reversed path - > path is from start to end
        Path.Reverse();
        return Path;
    }

    //will placing tower in coordinate blocks enemy path
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            //means that new path is blocked -> recalculate path
            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        // tower places will not block enemy path

        return false;
    }
    //broadcast messege to enemies even w/o listeners
    public void NotifyRecievers()
    {
        BroadcastMessage("RecalculatePath", false,SendMessageOptions.DontRequireReceiver);
    }
}
