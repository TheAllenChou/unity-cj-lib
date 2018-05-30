/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_PRIMITIVE_CORE
#define CJ_LIB_PRIMITIVE_CORE

#include "UnityCG.cginc"

struct appdata
{
  float4 vertex : POSITION;

#ifdef NORMAL_ON
  float3 normal : NORMAL;
#endif
};

struct v2f
{
  float4 vertex : SV_POSITION;

#ifdef NORMAL_ON
  float3 normal : NORMAL;
#endif
};

float4 _Color;

// (x, y, z) == (dimensionX, dimensionY, dimensionZ)
// w == capShiftScale (shift 0.5 towards X-Z plane, scale by dimensions, and then shoft back 0.5 * capShiftScale)
float4 _Dimensions;

float _ZBias;

sampler2D _MainTex;
float4 _MainTex_ST;

v2f vert (appdata v)
{
  v2f o;

#ifdef CAP_SHIFT_SCALE
  const float ySign = sign(v.vertex.y);
  v.vertex.y -= ySign * 0.5f;
#endif

  v.vertex.xyz *= _Dimensions.xyz;

#ifdef CAP_SHIFT_SCALE
  v.vertex.y += ySign * 0.5f * _Dimensions.w;
#endif

  o.vertex = UnityObjectToClipPos(v.vertex);
  o.vertex.z += _ZBias;

#ifdef NORMAL_ON
  float4x4 scaleInverseTranspose = float4x4
  (
    1.0f / _Dimensions.x, 0.0f, 0.0f, 0.0f, 
    0.0f, 1.0f / _Dimensions.y, 0.0f, 0.0f, 
    0.0f, 0.0f, 1.0f / _Dimensions.z, 0.0f, 
    0.0f, 0.0f, 0.0f, 1.0f
  );
  o.normal = mul(mul(UNITY_MATRIX_IT_MV, scaleInverseTranspose), float4(v.normal, 0.0f)).xyz;
#endif

  return o;
}

fixed4 frag (v2f i) : SV_Target
{
  fixed4 color = _Color;

#ifdef NORMAL_ON
  i.normal = normalize(i.normal);
  color.rgb *= 0.7f * i.normal.z + 0.3f; // darkest at 0.3f
#endif

  return color;
}

#endif
