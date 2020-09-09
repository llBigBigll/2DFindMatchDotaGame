using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Options 
{
    opt1,
    opt2,
    PervoeSlovo
}

public class HeroesGridLayout : LayoutGroup
{
    public bool LockRows;
    public int Rows;

    public bool LockCols;
    public int Cols;

    public bool LockSpacing;
    public Vector2 Spacing;

    public bool LockSize;
    public Vector2 CellSize;

    public override void CalculateLayoutInputVertical()
    {
        throw new System.NotImplementedException();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        
        if ((!LockCols) && (LockRows))
        {
            Cols = Mathf.CeilToInt((float)transform.childCount / (float)Rows);
        }        
        
        if ((!LockRows) && (LockCols))
        {
            Rows = Mathf.CeilToInt((float)transform.childCount / (float)Cols);
        }
        
        if ((!LockCols) && (!LockRows))
        {
            float sqrt = Mathf.Sqrt(transform.childCount);
            Rows = Mathf.CeilToInt(sqrt);
            Cols = Mathf.CeilToInt(sqrt);
        }

        if ((!LockSize)) 
        {
            CellSize.x = (parentWidth - (Cols - 1) * Spacing.x) / Cols;
            CellSize.y = (parentHeight - (Rows - 1) * Spacing.y) / Rows;
        }

        if ((!LockSize) && (!LockCols) && (!LockCols)) 
        {
        }

        int rowIdx;
        int colIdx;

        for (int i = 0; i < rectChildren.Count; i++) 
        {
            rowIdx = i / Cols;
            colIdx = i % Cols;

            var child = rectChildren[i];

            var xPos = (CellSize.x * colIdx) + Spacing.x * colIdx;
            var yPos = (CellSize.y * rowIdx) + Spacing.y * rowIdx;

            SetChildAlongAxis(child, 0, xPos, CellSize.x);
            SetChildAlongAxis(child, 1, yPos, CellSize.y);
        }
    }



    public override void SetLayoutHorizontal()
    {
        throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        throw new System.NotImplementedException();
    }
}
