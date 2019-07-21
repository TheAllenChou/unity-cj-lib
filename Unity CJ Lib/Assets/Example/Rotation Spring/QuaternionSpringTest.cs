using UnityEngine;

using CjLib;

public class QuaternionSpringTest : MonoBehaviour
{
  public Transform RotationTarget;

  private QuaternionSpring m_spring;

  public void Start()
  {
    m_spring.Reset();
  }

  void Update()
  {
    transform.rotation = m_spring.TrackDampingRatio(RotationTarget.rotation, 20.0f, 0.2f, Time.deltaTime);
  }
}
