/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/Example/GpuParticlesWithColliders"
{
  SubShader
  {
    Tags { "RenderType" = "Opaque" }

    Pass
    {
      Name "Deferred"
      Tags { "LightMode" = "Deferred" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #define PASS_DEFERRED
      #include "ParticleCore.cginc"
      ENDCG
    }

    Pass
    {
      Name "Shadow Caster"
      Tags { "LightMode" = "ShadowCaster" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #define PASS_SHADOW_CASTER
      #include "ParticleCore.cginc"
      ENDCG
    }
  }
}