using UnityEngine;

public class GridNode
{
  public Vector3 Position { get; }
  public Vector3 Size { get; }
  public Color Color { get; set; }

  public GridNode(Vector3 position, Vector3 size)
  {
    Position = position;
    Size = size;

    Color = Color.white;
  }
}