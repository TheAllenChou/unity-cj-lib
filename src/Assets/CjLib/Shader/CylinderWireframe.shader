/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/CylinderWireframe"
{
  Properties
  {
    _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    _Dimensions ("Dimensions", Vector) = (1.0, 1.0, 0.0, 0.0) // height, radius
    _MainTex ("Texture", 2D) = "white" {}
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
        v.vertex.xyz *= _Dimensions.yxy;
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
