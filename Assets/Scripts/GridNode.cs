using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
  public Vector3 Position { get; }
  public Vector3 Size { get; }
  public Color Color { get; set; }
  public bool Occupied { get; set; }
  public List<GridNode> NearNodes { get; set; }
  public float DistanceToTarget { get; set; } = 0f;
  public float DistanceToSource { get; set; } = 0f;
  public Vector3 CenterPoint { get; }

  public GridNode(Vector3 position, Vector3 size)
  {
    Position = position;
    Size = size;
    NearNodes = new List<GridNode>();
    CenterPoint = new Vector3(Position.x + Size.x / 2, Position.y, Position.z + Size.z / 2);

    ResetColor();
  }

  public void ResetColor()
  {
    Color = Color.white;
  }
}