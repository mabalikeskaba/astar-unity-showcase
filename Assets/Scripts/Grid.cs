using UnityEngine;

public class Grid : MonoBehaviour
{
  public int NodesPerLine = 1;

  private GridNode[,] GridNodes;
  private const float NodeHeight = 0.25f;

  // Start is called before the first frame update
  void Start()
  {
    GridNodes = new GridNode[NodesPerLine, NodesPerLine];

    var nodeSize = GetNodeSize();

    var bottomLeftX = transform.position.x - transform.localScale.x / 2 + nodeSize.x / 2;
    var bottomLeftY = transform.position.y;
    var bottomLeftZ = transform.position.z - transform.localScale.z / 2 + nodeSize.z / 2;

    for (int i = 0; i < NodesPerLine; i++)
    {
      for (int j = 0; j < NodesPerLine; j++)
      {
        GridNodes[i, j] = new GridNode(new Vector3(bottomLeftX + nodeSize.x * i, bottomLeftY, bottomLeftZ + nodeSize.z * j), nodeSize);
      }
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireCube(transform.position, transform.localScale);
    if(GridNodes != null)
    {
      foreach(var node in GridNodes)
      {
        Gizmos.DrawCube(node.Position, node.Size);
      }
    }
  }

  private Vector3 GetNodeSize()
  {
    if (NodesPerLine > 0)
    {
      var width = transform.localScale.x;
      var height = transform.localScale.z;

      return new Vector3(width / NodesPerLine, NodeHeight, height / NodesPerLine);
    }

    Debug.LogError("Nodes per line must be larger than 0!");
    return Vector3.one;
  }
}