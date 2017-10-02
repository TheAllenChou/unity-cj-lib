/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef PRIMITIVE_FLAT_COLOR_CORE
#define PRIMITIVE_FLAT_COLOR_CORE

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

#endif
