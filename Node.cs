using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//data container
[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;//can this node be added to the path tree
    public bool isExplored;//was this node explored in PF tree?
    public bool isPath;//is this node part of path of enemy?
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool isWalkable)//class C'tor
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }

}
