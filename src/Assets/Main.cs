/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using CjLib;
using UnityEngine;

public class Main : MonoBehaviour
{

  void Update()
  {
    DebugUtil.DrawSphere(new Vector3(0.0f, 0.0f, 1.0f), 2.0f, Quaternion.identity, 1, 5, Color.white);
    DebugUtil.DrawCapsule(new Vector3(3.0f, 3.0f, 0.0f), new Vector3(-1.0f, -1.0f, 0.0f), 0.5f, 3, 7, Color.white);
  }

  void OnDrawGizmos()
  {
    DebugUtil.DrawSphere(transform.position, 2.0f, Quaternion.identity, 1, 5, Color.white);
  }

}
