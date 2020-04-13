using UnityEngine;

public class GridNode
{
  public Vector3 Position { get; }
  public Vector3 Size { get; }
  public Color Color { get; set; }
  public bool Occupied { get; set; }
  public float GCost { get; set; }
  public float HCost { get; set; }
  public float FCost => GCost + HCost;
  public Vector3 CenterPoint { get; }
  public GridNode Parent { get; set; }
  public int CoordX { get; }
  public int CoordZ { get; }

  public GridNode(Vector3 position, Vector3 size, int coordX, int coordZ)
  {
    Position = position;
    Size = size;
    CenterPoint = new Vector3(Position.x + Size.x / 2, Position.y, Position.z + Size.z / 2);
    CoordX = coordX;
    CoordZ = coordZ;

    ResetColor();
  }

  public void ResetColor()
  {
    Color = Occupied ? Color.red : Color.white;
  }

  public void ResetCost()
  {
    GCost = 0;
    HCost = 0;
  }
}