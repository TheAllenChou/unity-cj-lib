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
#include "../../CjLib/Shader/Render/Forward.cginc"

#include "ParticleStruct.cginc"


struct appdata
{
  float4 vertex : POSITION;

#if defined(PASS_FORWARD) || defined(PASS_DEFERRED)
  float3 normal : NORMAL;
  UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
};

struct v2f
{
  float4 pos : SV_POSITION;

#if defined(PASS_FORWARD) || defined(PASS_DEFERRED)
  float3 normWs : NORMAL;
  float4 color  : COLOR0;
#endif

#if defined (PASS_FORWARD)
  float3 posWs  : COLOR1;
#endif

#if defined(PASS_FORWARD_BASE)
  float3 vertLight : COLOR2;
#endif

#if defined(PASS_FORWARD)
  LIGHTING_COORDS(0, 1)
#endif
};

struct fout
{
  fixed4 c0 : COLOR0; // diffuse (rgb), occlusion (a)

#if defined(PASS_DEFERRED)
  fixed4 c1 : COLOR1; // spec (rgb), smoothness (a)
  fixed4 c2 : COLOR2;
  fixed4 c3 : COLOR3;
#endif
};

// particle data
StructuredBuffer<Particle> particleBuffer;

v2f vert(appdata v, uint instance_id : SV_InstanceID)
{
  v2f o;

  float3 posOs = v.vertex;
  float4 rotOs = particleBuffer[instance_id].rotation;
  posOs = quat_rot(rotOs, posOs);

  float scale = particleBuffer[instance_id].scale;
  float4 lifetime = particleBuffer[instance_id].lifetime;
  scale = 
    lerp
    (
      0.0, 
      lerp(0.0, scale, saturate(lifetime.w / lifetime.x)),
      saturate(dot(lifetime, float4(1.0, 1.0, 1.0, -1.0)) / lifetime.z)
    );

  float3 posWs = scale * posOs + particleBuffer[instance_id].position;

  o.pos = UnityObjectToClipPos(posWs);

#if defined(PASS_FORWARD) || defined(PASS_DEFERRED)
  float3 normWs = quat_rot(rotOs, v.normal);
  o.normWs = normWs;
  o.color = particleBuffer[instance_id].color;

  v.vertex = float4(posWs, 1.0);

  UNITY_SETUP_INSTANCE_ID(v);
#endif

#if defined(PASS_FORWARD_BASE)
  o.vertLight = o.color * CJ_LIB_SHADE_POINT_LIGHTS(posWs, normWs);
#endif

#if defined(PASS_FORWARD)
  o.posWs = posWs;
  TRANSFER_VERTEX_TO_FRAGMENT(o);
#endif

  return o;
}

fout frag(v2f i)
{
  fout o;

#if defined(PASS_SHADOW_CASTER)
  o.c0.rgba = float4(0.0, 0.0, 0.0, 1.0);
#elif defined(PASS_FORWARD)
  o.c0.rgb = i.color.rgb * CJ_LIB_SHADE_MAIN_LIGHT(i, i.posWs, i.normWs);
  o.c0.a = i.color.a;
#elif defined(PASS_DEFERRED)
  o.c0.rgb = i.color;
  o.c0.a = 0.0;
  o.c1.rgb = 0.0;
  o.c1.a = 0.0;
  o.c2.rgb = i.normWs;
  o.c2.a = 0.0;
  o.c3 = 0.0;
#endif

#if defined(PASS_FORWARD_BASE)
  o.c0.rgb += i.color.rgb * i.vertLight;
#endif

  return o;
}
