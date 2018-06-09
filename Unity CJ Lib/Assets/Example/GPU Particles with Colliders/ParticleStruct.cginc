/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef PARTICLE_STRUCT
#define PARTICLE_STRUCT

struct Particle
{
  float3 position;
  float  damping;

  float4 rotation;

  float3 linearVelocity;
  float  scale;

  float4 angularVelocity;

  float4 lifetime; // (head, body, tail, current)

  float4 color;
};

#endif
