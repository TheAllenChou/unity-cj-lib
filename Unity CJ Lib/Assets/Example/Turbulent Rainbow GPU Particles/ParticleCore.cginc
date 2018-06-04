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

#include "../../CjLib/Math/MathUtil.cginc"
#include "../../CjLib/Render/MeshUtil.cginc"

#include "ParticleStruct.cginc"


struct appdata
{
  uint id : SV_VertexID;
};

struct v2g
{
  uint id : TEXCOORD0;
};

struct g2f
{
  float4 posCs : SV_POSITION;

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

// particle data
StructuredBuffer<Particle> particleBuffer;

v2g vert(appdata i)
{
  v2g o;
  o.id = i.id;
  return o;
}


[maxvertexcount(36)]
void geom(point v2g i[1], inout TriangleStream<g2f> stream)
{
  Particle p = particleBuffer[i[0].id];

  g2f o;

  o.color = p.color;

  float scale =
    lerp
    (
      0.0,
      lerp(0.0, p.scale, saturate(p.lifetime.w / p.lifetime.x)),
      saturate(dot(p.lifetime, float4(1.0, 1.0, 1.0, -1.0)) / p.lifetime.z)
    );

  for (uint i = 0; i < 12; ++i)
  {
    o.normWs = quat_mul(p.rotation, kCubeMeshNorms[i / 2]);
    for (uint j = 0; j < 3; ++j)
    {
      float3 posWs = 
        p.position 
        + scale * quat_mul(p.rotation, kCubeMeshVerts[kCubeMeshVertIdx[i * 3 + j]]);

      o.posCs = UnityObjectToClipPos(posWs);
      stream.Append(o);
    }

    stream.RestartStrip();
  }
}

fout frag(g2f i)
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
