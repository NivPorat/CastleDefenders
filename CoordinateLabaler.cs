using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]//start in play and edit mode
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabaler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, .5f, 0f);
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = true;
        displayCurrentCoordinates();
    }
    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)//only in edit mode
        {
            displayCurrentCoordinates();//display tile coordinates
            UpdateObjectName();//update obj name on change
        }
        SetLabelColor();//set the text label of tile color
        ToggleLabels();//can toggle visibility of text
    }

    //toggle labels on and off
    void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.enabled;
        }
    }

      void SetLabelColor()
    {
        if (gridManager == null) return;

        Node node = gridManager.GetNode(coordinates);
        if (node == null) return;

        if (!node.isWalkable) label.color = blockedColor;
        else if (node.isPath) label.color = pathColor;
        else if (node.isExplored) label.color = exploredColor;
        else label.color = defaultColor;
    }

    void displayCurrentCoordinates()
    {
        if (gridManager == null) return;

        coordinates.x = Mathf.RoundToInt( transform.parent.position.x/gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = coordinates.x+","+ coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }


}
