/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/Example/TurbulentRainbowBox"
{
  SubShader
  {
    Pass
    {
      Tags{ "RenderType" = "Opaque" }
      LOD 200

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #pragma target 5.0

      #include "UnityCG.cginc"

      #include "../../CjLib/Math/MathUtil.cginc"

      #include "TurbulentRainbowParticleStruct.cginc"

    struct appdata
    {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
      UNITY_VERTEX_INPUT_INSTANCE_ID
    };

      struct v2f
      {
        float4 position : SV_POSITION;
        float4 normal   : NORMAL;
        float4 color    : COLOR0;
      };

      // particles' data
      StructuredBuffer<Particle> particleBuffer;

      v2f vert(appdata i, uint instance_id : SV_InstanceID)
      {
        v2f o;

        UNITY_SETUP_INSTANCE_ID(i);

        float3 posOs = i.vertex;
        float4 rotOs = particleBuffer[instance_id].rotation;
        posOs = quat_mul(rotOs, posOs);

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
        float3 normWs = quat_mul(rotOs, i.normal);

        o.position = UnityObjectToClipPos(posWs);
        o.normal = mul(UNITY_MATRIX_IT_MV, normWs);
        o.color = particleBuffer[instance_id].color;

        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 color = i.color;
        color.rgb *= 0.7 * i.normal.z + 0.3;
        return color;
      }

      ENDCG
    }
  }
}