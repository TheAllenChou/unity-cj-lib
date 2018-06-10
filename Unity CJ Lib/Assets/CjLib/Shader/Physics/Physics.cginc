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

inline CollisionResult resolveCollision(float3 pos, float3 norm, float3 vel, float penetration, float restitution, float friction)
{
  float d = dot(vel, norm);   // projected relative speed onto contact normal
  float f = -d / length(vel); // ratio of relative speed along contact normal
  float3 velN = d * norm;     // normal relative velocity
  float3 velT = vel - velN;   // tangential relative velocity
  float3 velResolution = -(1.0 + restitution) * velN - friction * f * velT;

  CollisionResult res;
  res.position = pos + penetration * norm;
  res.velocity = vel + step(kEpsilon, penetration) * velResolution;
  return res;
}

//-----------------------------------------------------------------------------
// end: common


// VS plane
//-----------------------------------------------------------------------------

inline CollisionResult pointVsPlane(float3 p, float4 plane, float3 vel, float restitution, float friction)
{
  float penetration = max(0.0, -dot(float4(p, 1.0), plane));
  float3 norm = plane.xyz;
  return resolveCollision(p, norm, vel, penetration, restitution, friction);
}

inline CollisionResult sphereVsPlane(float4 s, float4 plane, float3 vel, float restitution, float friction)
{
  float penetration = max(0.0, s.w - dot(float4(s.xyz, 1.0), plane));
  float3 norm = plane.xyz;
  return resolveCollision(s.xyz, norm, vel, penetration, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS plane


// VS sphere
//-----------------------------------------------------------------------------

inline CollisionResult pointVsSphere(float3 p, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 centerDiff = p - sphere.xyz;
  float centerDiffLen = length(centerDiff);
  float penetration = max(0.0, sphere.w - centerDiffLen);
  float3 norm = centerDiff / centerDiffLen;
  return resolveCollision(p, norm, vel, penetration, restitution, friction);
}

inline CollisionResult sphereVsSphere(float4 s, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 centerDiff = s.xyz - sphere.xyz;
  float centerDiffLen = length(centerDiff);
  float penetration = max(0.0, s.w + sphere.w - centerDiffLen);
  float3 norm = centerDiff / centerDiffLen;
  return resolveCollision(s.xyz, norm, vel, penetration, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS sphere

#endif
