/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib

  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

    Based on an Andy Gryc's version of the common one-line shader random noise
    http://byteblacksmith.com/improvements-to-the-canonical-one-liner-glsl-rand/

*/
/******************************************************************************/


#ifndef RANDOM_NOISE
#define RANDOM_NOISE

#include "NoiseCommon.cginc"

float rand(float s)
{
  return frac(sin(mod(s, 6.2831853)) * 43758.5453123);
}

float rand(float2 s)
{
  float d = dot(s + 0.1234567, float2(1111112.9819837, 78.237173));
  float m = mod(d, 6.2831853);
  return frac(sin(m) * 43758.5453123);
}

float2 rand_vec(float2 s)
{
  return normalize(float2(rand(s), rand(s * 1.23456789)));
}

float rand(float3 s)
{
  float d = dot(s + 0.1234567, float3(11112.9819837, 378.237173, 3971977.9173179));
  float m = mod(d, 6.2831853);
  return frac(sin(m) * 43758.5453123);
}

float3 rand_vec(float3 s)
{
  return normalize(float3(rand(s), rand(s * 1.23456789), rand(s * 9876.654321)));
}

#endif
