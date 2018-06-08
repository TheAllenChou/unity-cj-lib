/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_MESH_UTIL
#define CJ_LIB_MESH_UTIL

static float3 kCubeMeshVerts[8] = 
{
  float3(-0.5, -0.5, -0.5), 
  float3(-0.5,  0.5, -0.5),
  float3( 0.5,  0.5, -0.5),
  float3( 0.5, -0.5, -0.5),
  float3(-0.5, -0.5,  0.5),
  float3(-0.5,  0.5,  0.5),
  float3( 0.5,  0.5,  0.5),
  float3( 0.5, -0.5,  0.5),
};

static float3 kCubeMeshNorms[6] = 
{
  float3( 0.0,  0.0, -1.0), 
  float3( 1.0,  0.0,  0.0),
  float3( 0.0,  0.0,  1.0),
  float3(-1.0,  0.0,  0.0),
  float3( 0.0,  1.0,  0.0),
  float3( 0.0, -1.0,  0.0),
};

static int kCubeMeshVertIdx[36] = 
{
  0, 1, 2, 0, 2, 3,
  3, 2, 6, 3, 6, 7,
  7, 6, 5, 7, 5, 4,
  4, 5, 1, 4, 1, 0,
  1, 5, 6, 1, 6, 2,
  0, 3, 7, 0, 7, 4,
};

#endif
