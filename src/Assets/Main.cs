using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

  void Update()
  {
    DebugUtil.DrawSphereCircles(Vector3.zero, 2.0f, 32, Color.white);
  }

}
