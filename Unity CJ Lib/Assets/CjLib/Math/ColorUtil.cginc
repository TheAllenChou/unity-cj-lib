/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_COLOR_UTIL_H
#define CJ_LIB_COLOR_UTIL_H

float3 hsv2rgb(float3 hsv) // hue is in degrees
{
  float h = hsv.x - 360.0 * floor(hsv.x / 360.0);
  float c = hsv.y * hsv.z;
  float hp = hsv.x / 60.0;
  hp = h - 2.0 * floor(h / 2.0) - 1.0;
  float x = c * (1.0 - abs(hp));
  float m = hsv.z - c;

  if (h >= 0.0f && h < 60.0)
    return float3(c, x, 0.0) + m;
  else if (h >= 60.0 && h < 120.0)
    return float3(x, c, 0.0) + m;
  else if (h >= 120.0 && h < 180.0)
    return float3(0.0, c, x) + m;
  else if (h >= 180.0 && h < 240.0)
    return float3(0.0, x, c) + m;
  else if (h >= 240.0 && h < 300.0)
    return float3(x, 0.0, c) + m;
  else
    return float3(c, 0.0, x) + m;
}

#endif
