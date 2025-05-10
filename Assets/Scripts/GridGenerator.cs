using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public interface IGridElement
{
    /// <summary>
    /// Get bounds of the grid element.
    /// </summary>
    /// <returns></returns>
    Bounds GetBounds();

    /// <summary>
    /// Get the game object of the grid element.
    /// </summary>
    /// <returns></returns>
    GameObject GetGameObject();

    /// <summary>
    /// set the scale of the grid element.
    /// </summary>
    void SetScale(Vector3 scale);

    /// <summary>
    /// Set the position of the grid element.
    /// </summary>
    void SetPosition(Vector3 position);
}

public class GridGenerator : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private GridElement gridObject;
    [SerializeField]
    private Transform gridParent;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private bool scaleToFitInGrid = false;

    [SerializeField]
    private int row;
    [SerializeField]
    private int column;

    [SerializeField]
    private float xSpacing;
    [SerializeField]
    private float ySpacing;

    [SerializeField]
    private float pivotX;
    [SerializeField]
    private float pivotY;

    #endregion

    public List<GridElement> gridElements = new List<GridElement>();
    private Vector3 gridElementBounds;
    private Vector3 gridElementNewScale;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        gridElementBounds = GetGridElementSize(gridObject);
    }

    /// <summary>
    /// Context menu editor method to generate the grid editor time.
    /// </summary>
    [ContextMenu("GenerateGrid")]
    public void GenerateGrid()
    {
        Initialize();
        ResetGrid();
        GenerateGrid(row,column);
    }

    /// <summary>
    /// Generate grid of given row and column.
    /// </summary>
    public void GenerateGrid(int row,int coloumn)
    {
        gridElementNewScale = GetScaleForGridElement();
        Debug.Log("Grid Element new scale is :" + gridElementNewScale);
        gridObject.transform.localScale = gridElementNewScale;

        float totalXSize = CalculateXPivot(GetGridElementSize(gridObject).x) * row;
        float totalYSize = CalculateYPivot(GetGridElementSize(gridObject).y) * coloumn;

        Vector2 position = Vector2.zero;
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < coloumn; j++)
            {
                GridElement gridElement =  Instantiate(gridObject, new Vector3(i, 0, j), Quaternion.identity);
                gridElement.SetScale(gridElementNewScale);
                gridElements.Add(gridElement);

                //set the obejct position in grid based on the index.
                position = (i * (GetGridElementSize(gridElement).x + xSpacing) * Vector2.right) + (j * (GetGridElementSize(gridElement).y + ySpacing) * Vector2.up);
                //Deducting the pivot values so that it can be arranged based on given pivot.
                position -= new Vector2(totalXSize - CalculateXPivot(GetGridElementSize(gridElement).x), totalYSize - CalculateYPivot(GetGridElementSize(gridElement).y));

                gridElement.transform.localPosition = position;
                gridElement.transform.SetParent(gridParent);
                gridElement.name = "Grid" + i + j;
            }
        }
    }

    /// <summary>
    /// Get the scale for grid element based on the given grid size and spacing.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetScaleForGridElement()
    {
        if (scaleToFitInGrid)
        {
            float xScale = gridElementBounds.x;
            float yScale = gridElementBounds.y;

            float xAspectRatio = xScale / yScale;
            float yAspectRatio = yScale / xScale;

            if (((yScale + ySpacing) * column) / gridSize.y > 1)
            {
                yScale = ((gridSize.y - (ySpacing * column)) / column);

                xScale = yScale * xAspectRatio;
            }

            if (((xScale + xSpacing) * row) / gridSize.x > 1)
            {
                xScale = ((gridSize.x - (xSpacing * row)) / row);

                yScale = xScale * yAspectRatio;
            }

            return new Vector2(xScale, yScale);
        }
        else
        {
            return gridElementBounds;
        }

    }

    /// <summary>
    /// Calculate the pivot value based on the given grid element size and spacing.
    /// </summary>
    /// <returns></returns>
    private float CalculateXPivot(float xSize)
    {
        return (xSize + xSpacing) * pivotX;
    }

    /// <summary>
    /// Calculate the pivot value based on the given grid element size and spacing.
    /// </summary>
    /// <returns></returns>
    private float CalculateYPivot(float ySize)
    {
        return (ySize + ySpacing) * pivotY;
    }


    /// <summary>
    /// Get the grid element size based on the given grid object.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetGridElementSize(GridElement gridElement)
    {
        Debug.Log("bounds is :" + gridElement.GetBounds().size);
        return gridElement.GetBounds().size;
    }

    /// <summary>
    /// Reset the grid by destroying all the grid elements.
    /// </summary>
    public void ResetGrid()
    {
        for (int i = 0; i < gridElements.Count; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(gridElements[i].GetGameObject());
#else
            Destroy(gridElements[i].GetGameObject());
#endif
        }
        gridElements.Clear();
    }
}
