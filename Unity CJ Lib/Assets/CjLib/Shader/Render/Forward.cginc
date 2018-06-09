/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_FORWARD
#define CJ_LIB_FORWARD

#include "UnityStandardCore.cginc"

#define CJ_LIB_SHADE_MAIN_LIGHT(v, posWs, normWs)                    \
(                                                                    \
  dot(normWs, normalize(WorldSpaceLightDir(float4(posWs.xyz, 1.0)))) \
  * LIGHT_ATTENUATION(v)                                             \
)

#define CJ_LIB_SHADE_POINT_LIGHTS(posWs, normWs) \
(                                                \
  Shade4PointLights                              \
  (                                              \
    unity_4LightPosX0,                           \
    unity_4LightPosY0,                           \
    unity_4LightPosZ0,                           \
    unity_LightColor[0].rgb,                     \
    unity_LightColor[1].rgb,                     \
    unity_LightColor[2].rgb,                     \
    unity_LightColor[3].rgb,                     \
    unity_4LightAtten0,                          \
    posWs.xyz,                                   \
    normWs.xyz                                   \
  )                                              \
)

#endif
