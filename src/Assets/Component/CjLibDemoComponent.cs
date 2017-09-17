using UnityEngine;

public class CjLibDemoComponent : MonoBehaviour
{

  protected virtual void Draw() { }

  void Update()
  {
    Draw();
  }

  void OnDrawGizmos()
  {
    Draw();
  }

}
