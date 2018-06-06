/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/Primitive"
{
  SubShader
  {
    Tags { "Queue"="Transparent" "RenderType"="Transparent" "DisableBatching"="true" }
    LOD 100

    Pass
    {
      Blend SrcAlpha OneMinusSrcAlpha

      CGPROGRAM
      #pragma shader_feature NORMAL_ON
      #pragma shader_feature CAP_SHIFT_SCALE
      #pragma vertex vert
      #pragma fragment frag
      #include "PrimitiveCore.cginc"
      ENDCG
    }
  }
}
