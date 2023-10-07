using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAreaRenderer : Graphic
{
    public UIGridRenderer grid;
    public Vector2Int gridSize;

    public List<Vector2> points;

    float width;
    float height;
    float unitWidth;
    float unitHeight;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;

        if (points.Count < 2) return;


        float angle = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {

            Vector2 point = points[i];
            Vector2 point2 = points[i + 1];

            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 90f;
            }

            DrawVerticesForPoint(point, point2, angle, vh);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 4;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 1, index + 2, index + 3);

            if (index != 0)
            {
                vh.AddTriangle(index + 1, (index - 4) + 2, (index - 4) + 3);
                vh.AddTriangle(index , (index - 4) + 2, (index - 4) + 3);
            }
        }
    }
    public float GetAngle(Vector2 me, Vector2 target)
    {
        //panel resolution go there in place of 9 and 16

        return (float)(Mathf.Atan2(9f * (target.y - me.y), 16f * (target.x - me.x)) * (180 / Mathf.PI));
    }
    void DrawVerticesForPoint(Vector2 point, Vector2 point2, float angle, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(unitWidth * point.x, 0f);
        vh.AddVert(vertex);

        vertex.position = new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(unitWidth * point2.x, unitHeight * point2.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(unitWidth * point2.x, 0f);
        vh.AddVert(vertex);


    }


    private void Update()
    {
        if (grid != null && grid.gridSize != gridSize)
        {
            gridSize = grid.gridSize;
        }
    }
}
