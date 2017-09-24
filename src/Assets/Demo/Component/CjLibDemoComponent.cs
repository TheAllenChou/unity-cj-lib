using UnityEngine;

public class CjLibDemoComponent : MonoBehaviour
{

  protected virtual void DebugDraw() { }

  void Update()
  {
    DebugDraw();
  }

  void OnDrawGizmos()
  {
    DebugDraw();
  }

}
