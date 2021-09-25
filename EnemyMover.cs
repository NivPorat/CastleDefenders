using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float Speed = 1f;

    List<Node> Path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }
   

    void RecalculatePath(bool resetPath)
    {

        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        StopAllCoroutines();//stop current pathfinding
        Path.Clear();//clear existing path
        Path = pathFinder.GetNewPath(coordinates);//get new path
        StartCoroutine(FollowPath());//resume with new path
    }

    void ReturnToStart()
    {

        transform.position = gridManager.getPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);//for pool reuse

    }

    IEnumerator FollowPath()
    {
        for(int i = 1; i < Path.Count; i++)
        {
            Vector3 StartPosiition = transform.position;
            Vector3 EndPosition = gridManager.getPositionFromCoordinates(Path[i].coordinates);
            float TravelPercent = 0f;

            transform.LookAt(EndPosition);

            while(TravelPercent < 1)
            {
                TravelPercent += Time.deltaTime * Speed;
                transform.position = Vector3.Lerp(StartPosiition, EndPosition, TravelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }
}
