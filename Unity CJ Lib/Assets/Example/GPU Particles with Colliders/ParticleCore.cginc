/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#include "AutoLight.cginc"
#include "UnityCG.cginc"

#include "../../CjLib/Shader/Math/Math.cginc"

#include "ParticleStruct.cginc"

struct appdata
{
  float4 posOs  : POSITION;
  float3 normOs : NORMAL;
  uint   id     : SV_InstanceId;
};

struct v2f
{
  float4 posCs  : SV_POSITION;
  float3 normWs : NORMAL;
  float4 color  : COLOR0;
};

struct fout
{
  fixed4 c0 : COLOR0; // diffuse (rgb), occlusion (a)
  fixed4 c1 : COLOR1; // spec (rgb), smoothness (a)
  fixed4 c2 : COLOR2; // normalWs (rgb), 
  fixed4 c3 : COLOR3; // emission
};

StructuredBuffer<Particle> particleBuffer;
float4x4 viewMatrix;
float4x4 projMatrix;

v2f vert(appdata i)
{
  v2f o;

  Particle p = particleBuffer[i.id];

  float scale =
    lerp
    (
      0.0,
      lerp(0.0, p.scale, saturate(p.lifetime.w / p.lifetime.x)),
      saturate(dot(p.lifetime, float4(1.0, 1.0, 1.0, -1.0)) / p.lifetime.z)
    );

  float3 posWs = 
    p.position 
    + scale * quat_rot(p.rotation, i.posOs.xyz);

  o.posCs = UnityObjectToClipPos(posWs);
  o.normWs = quat_rot(p.rotation, i.normOs);
  o.color = p.color;

  return o;
}

fout frag(v2f i)
{
  fout o;

  o.c0 = i.color;
  o.c1.rgb = 0.0;
  o.c1.a = 1.0;
  o.c2.rgb = i.normWs;
  o.c2.a = 0.0;
  o.c3 = 0.3 * i.color;

  return o;
}
