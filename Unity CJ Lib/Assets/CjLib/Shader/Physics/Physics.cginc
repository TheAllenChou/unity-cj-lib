/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_PHYSICS
#define CJ_LIB_PHYSICS


#include "../Math/Math.cginc"


// common
//-----------------------------------------------------------------------------

struct CollisionResult
{
  float3 position;
  float3 velocity;
};

inline CollisionResult resolveCollision(float3 pos, float3 norm, float3 vel, float penetrationDepth, float restitution, float friction)
{
  float d = dot(vel, norm);
  float f = -d / length(vel);
  float3 velPerp = d * norm;
  float3 velPara = vel - velPerp;
  float3 velResolution = -(1.0 + restitution) * velPerp - friction * f * velPara;

  CollisionResult res;
  res.position = pos + penetrationDepth * norm;
  res.velocity = vel + step(kEpsilon, penetrationDepth) * velResolution;
  return res;
}

//-----------------------------------------------------------------------------
// end: common


// VS plane
//-----------------------------------------------------------------------------

inline CollisionResult pointVsPlane(float3 p, float4 plane, float3 vel, float restitution, float friction)
{
  float d = max(0.0, -dot(float4(p, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(p, n, vel, d, restitution, friction);
}

inline CollisionResult sphereVsPlane(float4 s, float4 plane, float3 vel, float restitution, float friction)
{
  float d = max(0.0, s.w - dot(float4(s.xyz, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(s.xyz, n, vel, d, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS plane


// VS sphere
//-----------------------------------------------------------------------------

inline CollisionResult pointVsSphere(float3 p, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 r = p - sphere.xyz;
  float d = max(0.0, sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(p, n, vel, d, restitution, friction);
}

inline CollisionResult sphereVsSphere(float4 s, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 r = s.xyz - sphere.xyz;
  float d = max(0.0, s.w + sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(s.xyz, n, vel, d, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS sphere

#endif
