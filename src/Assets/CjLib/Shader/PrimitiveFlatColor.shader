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
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
      };

      struct v2f
      {
        float4 vertex : SV_POSITION;
      };

      float4 _Color;
      float4 _Dimensions;

      sampler2D _MainTex;
      float4 _MainTex_ST;

      v2f vert (appdata v)
      {
        v2f o;
        const float ySign = sign(v.vertex.y) * sign(_Dimensions.w);
        v.vertex.y -= ySign * 0.5f;
        v.vertex.xyz *= _Dimensions.xyz;
        v.vertex.y += ySign * 0.5f * _Dimensions.w;
        o.vertex = UnityObjectToClipPos(v.vertex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      {
        return _Color;
      }
      ENDCG
    }
  }
}
