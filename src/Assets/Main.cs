using UnityEngine;
using CjLib;

public class Main : MonoBehaviour
{

  void Update()
  {
    DebugUtil.DrawSphere(Vector3.zero, 2.0f, Quaternion.AxisAngle(new Vector3(1.0f, 1.0f, 0.0f), Time.time), 32, 16, Color.white);
  }

}
