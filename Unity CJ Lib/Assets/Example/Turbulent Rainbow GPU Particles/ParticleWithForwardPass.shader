/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

Shader "CjLib/Example/TurbulentRainbowParticle"
{
  SubShader
  {
    Tags { "RenderType" = "Opaque" }

    Pass
    {
      Name "ForwardBase"
      Tags { "LightMode" = "ForwardBase" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #pragma multi_compile_fwdbase
      #pragma multi_compile_prepassfinal noshadowmask nodynlightmap nodirlightmap nolightmap
      #define PASS_FORWARD
      #define PASS_FORWARD_BASE
      #include "ParticleCoreWithForwardPass.cginc"
      ENDCG
    }

    Pass
    {
      Name "ForwardAdd"
      Tags { "LightMode" = "ForwardAdd" }
      Blend One One
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #pragma multi_compile_fwdbase
      #pragma multi_compile_prepassfinal noshadowmask nodynlightmap nodirlightmap nolightmap
      #define PASS_FORWARD
      #define PASS_FORWARD_ADD
      #include "ParticleCoreWithForwardPass.cginc"
      ENDCG
    }

    Pass
    {
      Name "Deferred"
      Tags { "LightMode" = "Deferred" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #pragma multi_compile_prepassfinal noshadowmask nodynlightmap nodirlightmap nolightmap
      #define PASS_DEFERRED
      #include "ParticleCoreWithForwardPass.cginc"
      ENDCG
    }

    Pass
    {
      Name "ShadowCaster"
      Tags { "LightMode" = "ShadowCaster" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_instancing
      #pragma multi_compile_prepassfinal noshadowmask nodynlightmap nodirlightmap nolightmap
      #define PASS_SHADOW_CASTER
      #include "ParticleCoreWithForwardPass.cginc"
      ENDCG
    }
  }
}