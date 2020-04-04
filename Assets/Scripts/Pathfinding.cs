using UnityEngine;

public class Pathfinding : MonoBehaviour
{
  public GridNode CurrentNode;
  public Grid NodeGrid;

  // Start is called before the first frame update
  void Start()
  {
    if (NodeGrid != null)
    {
      var currentNode = NodeGrid.GetNodeFromPosition(transform.position.x, transform.position.z);

      NodeGrid.UpdateNode(currentNode, node => node.Color = Color.green);
      foreach(var nearNode in currentNode.NearNodes)
      {
        NodeGrid.UpdateNode(nearNode, node => node.Color = Color.yellow);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
  }
}