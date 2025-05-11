using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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

public interface IPoolObject
{
    public void OnGetFromPool();
    public void OnReleaseToPool();
}

[System.Serializable]
public class GridData
{
    public int row;
    public int column;
    public float xSpacing;
    public float ySpacing;
    public float pivotX;
    public float pivotY;
    public GridData(int row, int column, float xSpacing, float ySpacing, float pivotX, float pivotY)
    {
        this.row = row;
        this.column = column;
        this.xSpacing = xSpacing;
        this.ySpacing = ySpacing;
        this.pivotX = pivotX;
        this.pivotY = pivotY;
    }
}

public class GridGenerator : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private GameObject gridBg;
    [SerializeField]
    private GridElement gridObject;
    [SerializeField]
    private Transform gridParent;
    [SerializeField]
    private Transform poolParent;
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
    private Vector3 originalScale;
    private GridData gridData;

    public GridData GridData { get => gridData; }

    /// <summary>
    /// Context menu editor method to generate the grid editor time.
    /// </summary>
    [ContextMenu("GenerateGrid")]
    public void GenerateGrid()
    {
        //GenerateGrid(out gridElements);
    }

    public void Initialize()
    {
        GridElementPool.Instance.Initialize(gridObject);
    }    

    public void GenerateGrid(GridData gridData,out List<GridElement> outGridElements)
    {
        row = gridData.row;
        column = gridData.column;
        xSpacing = gridData.xSpacing;
        ySpacing = gridData.ySpacing;
        pivotX = gridData.pivotX;
        pivotY = gridData.pivotY;

        GenerateGrid(out outGridElements);
    }

    /// <summary>
    /// Generate grid of given row and column.
    /// </summary>
    public void GenerateGrid(out List<GridElement> outGridElements)
    {
        gridBg.transform.localScale = gridSize;
        ResetGrid();
        gridData = new GridData(row, column, xSpacing, ySpacing, pivotX, pivotY);
        GenerateGrid(row, column, out outGridElements);
    }

    private GridElement GetGridElementFromPool()
    {
        return GridElementPool.Instance.Pool.Get();
    }

    /// <summary>
    /// Generate grid of given row and column.
    /// </summary>
    public void GenerateGrid(int row, int coloumn,out List<GridElement> outGridElements)
    {
        originalScale = gridObject.transform.localScale;

        Vector2 scaleForGridElement = GetScaleForGridElement();
        gridObject.transform.localScale = scaleForGridElement;

        float xSize = GetGridObjectBounds().x;
        float ySize = GetGridObjectBounds().y;

        float xPivot = CalculateXPivot();
        float yPivot = CalculateYPivot();

        Vector2 pivotOffset = new Vector2(
            (CalculateXPivot() * row) - xPivot, 
            (CalculateYPivot() * coloumn) - yPivot
        );
        Vector2 position = Vector2.zero;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < coloumn; j++)
            {
                GridElement grid = GetGridElementFromPool();
                grid.transform.localPosition = new Vector3(i, 0, j);
                grid.transform.localScale = scaleForGridElement;
                gridElements.Add(grid);

                //set the obejct position in grid based on the index.
                position = (i * (xSize + xSpacing) * Vector2.right) + (j * (ySize + ySpacing) * Vector2.up);
                //Deducting the pivot values so that it can be arranged based on given pivot.
                position -= pivotOffset;

                grid.transform.SetParent(gridParent);
                grid.transform.localPosition = position;
                grid.name = "Grid" + i + j;
                grid.gridPosition = new Vector2(i, j);
            }
        }
        outGridElements = gridElements;
        gridObject.SetScale(originalScale);
    }

    /// <summary>
    /// Get the scale for grid element based on the given grid size and spacing.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetScaleForGridElement()
    {

        if (scaleToFitInGrid)
        {
            float xSize = GetGridObjectBounds().x;
            float ySize = GetGridObjectBounds().y;

            float xAspectRatio = gridObject.transform.localScale.x / gridObject.transform.localScale.y;
            float yAspectRatio = gridObject.transform.localScale.y / gridObject.transform.localScale.x;

            //By default shrinking height to fit into grid height.
            {
                ySize = (gridSize.y - (ySpacing * column)) / (column * ySize);

                xSize = ySize * xAspectRatio;
            }

            //Storing the original size width for checking.
            float newXScale = GetGridObjectBounds().x;

            //Applying the scale to the grid object to get new bounds.
            gridObject.transform.localScale = new Vector3(xSize, ySize);

            // Post shrinking height checking if the new width is greater than the grid width.
            // If yes shrinking the width to fit into grid width.
            if (((GetGridObjectBounds().x + xSpacing) * row) / gridSize.x > 1)
            {
                xSize = (gridSize.x - (xSpacing * row)) / ( row * newXScale);

                ySize = xSize * yAspectRatio;
            }

            return new Vector2(xSize, ySize);
        }
        else
        {
            return GetGridObjectBounds();
        }

    }

    /// <summary>
    /// Calculate the pivot value based on the given grid element size and spacing.
    /// </summary>
    /// <returns></returns>
    private float CalculateXPivot()
    {
        return (GetGridObjectBounds().x + xSpacing) * pivotX;
    }

    /// <summary>
    /// Calculate the pivot value based on the given grid element size and spacing.
    /// </summary>
    /// <returns></returns>
    private float CalculateYPivot()
    {
        return (GetGridObjectBounds().y + ySpacing) * pivotY;
    }

    /// <summary>
    /// Get the grid element size based on the given grid object.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetGridObjectBounds()
    {
        return gridObject.GetBounds().size;
    }

    /// <summary>
    /// Reset the grid by destroying all the grid elements.
    /// </summary>
    public void ResetGrid()
    {
        for (int i = 0; i < gridElements.Count; i++)
        {
            GridElementPool.Instance.Pool.Release(gridElements[i]);
            gridElements[i].transform.SetParent(GridElementPool.Instance.transform);
        }
        gridElements.Clear();
    }
}
