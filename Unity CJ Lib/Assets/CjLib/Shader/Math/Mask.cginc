/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_MASK
#define CJ_LIB_MASK

// make_mask
//-----------------------------------------------------------------------------

inline float make_mask(float a)
{
  return 1.0f - step(0.0f, -a);
}

inline float2 make_mask(float2 a)
{
  return 1.0f - step(0.0f, -a);
}

inline float3 make_mask(float3 a)
{
  return 1.0f - step(0.0f, -a);
}

inline float4 make_mask(float4 a)
{
  return 1.0f - step(0.0f, -a);
}


// mask_and
//-----------------------------------------------------------------------------

inline float mask_and(float a, float b)
{
  return make_mask(make_mask(a) * make_mask(b));
}

inline float2 mask_and(float2 a, float2 b)
{
  return make_mask(make_mask(a) * make_mask(b));
}

inline float3 mask_and(float3 a, float3 b)
{
  return make_mask(make_mask(a) * make_mask(b));
}

inline float4 mask_and(float4 a, float4 b)
{
  return make_mask(make_mask(a) * make_mask(b));
}


// mask_ior
//-----------------------------------------------------------------------------

inline float mask_ior(float a, float b)
{
  return make_mask(make_mask(a) + make_mask(b));
}

inline float2 mask_ior(float2 a, float2 b)
{
  return make_mask(make_mask(a) + make_mask(b));
}

inline float3 mask_ior(float3 a, float3 b)
{
  return make_mask(make_mask(a) + make_mask(b));
}

inline float4 mask_ior(float4 a, float4 b)
{
  return make_mask(make_mask(a) + make_mask(b));
}


// mask_not
//-----------------------------------------------------------------------------

inline float mask_not(float a)
{
  return step(a, 0.0f);
}

inline float2 mask_not(float2 a)
{
  return step(a, 0.0f);
}

inline float3 mask_not(float3 a)
{
  return step(a, 0.0f);
}

inline float4 mask_not(float4 a)
{
  return step(a, 0.0f);
}


// mask_eq
//-----------------------------------------------------------------------------

inline float mask_eq(float a, float b)
{
  return
    mask_ior
    (
      mask_and(mask_not(a), mask_not(b)), 
      mask_and(a, b)
    );
}

inline float2 mask_eq(float2 a, float2 b)
{
  return
    mask_ior
    (
      mask_and(mask_not(a), mask_not(b)), 
      mask_and(a, b)
    );
}

inline float3 mask_eq(float3 a, float3 b)
{
  return
    mask_ior
    (
      mask_and(mask_not(a), mask_not(b)), 
      mask_and(a, b)
    );
}

inline float4 mask_eq(float4 a, float4 b)
{
  return
    mask_ior
    (
      mask_and(mask_not(a), mask_not(b)), 
      mask_and(a, b)
    );
}


// mask_xor
//-----------------------------------------------------------------------------

inline float mask_xor(float a, float b)
{
  return
    mask_and
    (
      mask_ior(a, b), 
      mask_ior(mask_not(a), mask_not(b))
    );
}


inline float2 mask_xor(float2 a, float2 b)
{
  return
    mask_and
    (
      mask_ior(a, b), 
      mask_ior(mask_not(a), mask_not(b))
    );
}

inline float3 mask_xor(float3 a, float3 b)
{
  return
    mask_and
    (
      mask_ior(a, b), 
      mask_ior(mask_not(a), mask_not(b))
    );
}

inline float4 mask_xor(float4 a, float4 b)
{
  return
    mask_and
    (
      mask_ior(a, b), 
      mask_ior(mask_not(a), mask_not(b))
    );
}

#endif
