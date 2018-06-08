/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

  Based on Noise Shader Library for Unity
  https://github.com/keijiro/NoiseShader

  Original work (webgl-noise) Copyright (C) 2011 Ashima Arts.
  Translation and modification was made by Keijiro Takahashi.

    Description : Array and textureless GLSL 2D simplex noise function.
        Author  : Ian McEwan, Ashima Arts.
    Maintainer  : ijm
        Lastmod : 20110822 (ijm)
        License : Copyright (C) 2011 Ashima Arts. All rights reserved.
                  Distributed under the MIT License. See LICENSE file.
                  https://github.com/ashima/webgl-noise
*/
/******************************************************************************/

Shader "CjLib/Example/NoiseTest"
{
  Properties
  {
    _Offset("Offset", Vector)                          = (0.0, 0.0, 0.0, 0.0)
    _NumOctaves("Num Octaves", Int)                    = 6
    _OctaveOffsetFactor("Octave Offset Factor", Float) = 1.2
  }

  CGINCLUDE
  #pragma multi_compile CNOISE PNOISE SNOISE SNOISE_GRAD RAND RAND_VEC
  #pragma multi_compile _ MULTIPLE_OCTAVES
  #pragma multi_compile _ THREED

  #include "UnityCG.cginc"

  #include "../../CjLib/Shader/Noise/Noise.cginc"

  float4 _Offset;
  uint _NumOctaves;
  float _OctaveOffsetFactor;

  struct v2f
  {
    float4 posSs : SV_POSITION;
    float3 posWs : TEXCOORD0;
  };

  v2f vert(appdata_base v)
  {
    v2f o;
    o.posSs = UnityObjectToClipPos(v.vertex);
    o.posWs = mul(unity_ObjectToWorld, v.vertex).xyz;
    return o;
  }

  float4 frag(v2f i) : SV_Target
  {
    #if defined(THREED)
      float3 s = i.posWs * 4.0;
      float3 offset = _Offset.xyz;
    #else
      float2 s = i.posWs.xy * 4.0;
      float2 offset = _Offset.xy;
    #endif

    #if defined(CNOISE)
      #define NOISE_FUNC cnoise
    #elif defined(PNOISE)
      #define NOISE_FUNC pnoise
      #if defined(THREED)
        float3 period = 1.0;
      #else
        float2 period = 1.0;
      #endif
    #elif defined(SNOISE)
      #define NOISE_FUNC snoise
    #elif defined(SNOISE_GRAD)
      #define NOISE_FUNC snoise_grad
    #elif defined(RAND)
      #define NOISE_FUNC rand
    #else // RAND_VEC
      #define NOISE_FUNC rand_vec
    #endif

    float4 o = 0.0;
    #if defined(PNOISE)
      o = NOISE_FUNC(s, offset, period, _NumOctaves, _OctaveOffsetFactor);
    #elif defined(SNOISE_GRAD)
      #if defined(THREED)
        o.rgb = NOISE_FUNC(s, offset, _NumOctaves, _OctaveOffsetFactor);
      #else
        o.rg = NOISE_FUNC(s, offset, _NumOctaves, _OctaveOffsetFactor);
        o.b = 0.0;
      #endif
    #else
      #if defined(RAND)
        o = NOISE_FUNC(s + offset);
      #elif defined(RAND_VEC)
        #if defined(THREED)
            o.rgb = NOISE_FUNC(s + offset);
        #else
            o.rg = NOISE_FUNC(s + offset);
            o.b = 0.0;
        #endif
      #else
        o = NOISE_FUNC(s, offset, _NumOctaves, _OctaveOffsetFactor);
      #endif
    #endif

    o.a = 1.0;
    return o;
  }
  ENDCG

  SubShader
  {
    Pass
    {
      CGPROGRAM
      #pragma target 3.0
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
    }
  }
}