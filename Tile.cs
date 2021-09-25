using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]  bool isPlaceable;

    [SerializeField] Tower TowerPrefab;
    public bool IsPlaceable { get { return isPlaceable; } }

    GridManager gridManager;
    PathFinder pathFinder;

    Vector2Int coordinates = new Vector2Int();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

     void Start()
    {
        if(gridManager != null)
        {
            //converting world position into [x,y] coordinates
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if(!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        //if current tile can be walked on, and placing tower wont block all enemy path
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            //if i can place a tower, do i have enough coins to do so?
            bool isSuccessfull = TowerPrefab.CreateTower(TowerPrefab, transform.position);
            if (isSuccessfull)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyRecievers();
            }
        }
    }
}
