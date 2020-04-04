using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
  public Vector3 Position { get; }
  public Vector3 Size { get; }
  public Color Color { get; set; }
  public bool Occupied { get; set; }
  public List<GridNode> NearNodes { get; set; }

  public GridNode(Vector3 position, Vector3 size)
  {
    Position = position;
    Size = size;
    NearNodes = new List<GridNode>();

    ResetColor();
  }

  public void ResetColor()
  {
    Color = Color.white;
  }
}