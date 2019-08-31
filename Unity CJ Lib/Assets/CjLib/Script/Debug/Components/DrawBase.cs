/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;

namespace CjLib
{
  public abstract class DrawBase : MonoBehaviour
  {
    public Color WireframeColor = Color.white;
    public Color ShadededColor = Color.gray;

    public bool Wireframe = false;
    public DebugUtil.Style Style = DebugUtil.Style.FlatShaded;

    public bool DepthTest = true;

    private void Update()
    {
      if (Style != DebugUtil.Style.Wireframe)
        Draw(ShadededColor, Style, DepthTest);

      if (Style == DebugUtil.Style.Wireframe || Wireframe)
        Draw(WireframeColor, DebugUtil.Style.Wireframe, DepthTest);
    }

    protected abstract void Draw(Color color, DebugUtil.Style style, bool depthTest);
  }
}
