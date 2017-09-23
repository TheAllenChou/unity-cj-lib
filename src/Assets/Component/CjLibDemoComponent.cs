using UnityEngine;

public class CjLibDemoComponent : MonoBehaviour
{

  protected virtual void DebugDraw() { }
  protected virtual void DrawGizmos() { }

  void Update()
  {
    DebugDraw();
  }

  void OnDrawGizmos()
  {
    DrawGizmos();
  }

}
