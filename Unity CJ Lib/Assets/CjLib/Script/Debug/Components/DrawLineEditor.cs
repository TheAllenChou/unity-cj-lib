/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CjLib
{
  [CustomEditor(typeof(DrawLine)), CanEditMultipleObjects]
  public class DrawLineEditor : Editor
  {
    private void OnSceneGUI()
    {
      var drawLine = (DrawLine) target;

      Vector3 oldLineEnd = drawLine.transform.position + drawLine.transform.TransformVector(drawLine.LocalEndVector);

      Vector3 newLineEnd = 
        Handles.PositionHandle
        (
          oldLineEnd, 
          Tools.pivotRotation == PivotRotation.Global 
            ? Quaternion.identity 
            : drawLine.transform.rotation
        ) ;

      Vector3 delta = newLineEnd - oldLineEnd;

      if (delta.sqrMagnitude <= 0.0f)
        return;

      drawLine.LocalEndVector += drawLine.transform.InverseTransformVector(delta);

      EditorApplication.QueuePlayerLoopUpdate();
    }
  }
}
#endif
