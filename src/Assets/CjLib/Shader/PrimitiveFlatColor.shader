/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/PrimitiveFlatColor"
{
  Properties
  {
    _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    _MainTex ("Texture", 2D) = "white" {}

    // (x, y, z) == (dimensionX, dimensionY, dimensionZ)
    // w == shiftY (0.0 -> no shif;, >0.0 -> shift 0.5 towards origin, scale by dimensions, and then shoft back 0.5 * offsetY)
    _Dimensions ("Dimensions", Vector) = (1.0, 1.0, 1.0, 0.0)
  }

  SubShader
  {
    Tags { "Queue"="Transparent" "RenderType"="Transparent" "DisableBatching" = "true" }
    LOD 100

    Pass
    {
      Blend SrcAlpha OneMinusSrcAlpha

      CGPROGRAM
      #include "PrimitiveFlatColorCore.cginc"
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
    }
  }
}
