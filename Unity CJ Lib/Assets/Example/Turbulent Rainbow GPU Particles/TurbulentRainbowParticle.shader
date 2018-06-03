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
      Name "Deferred"
      Tags { "LightMode" = "Deferred" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma geometry geom
      #pragma fragment frag
      #define PASS_DEFERRED
      #include "TurbulentRainbowParticleCore.cginc"
      ENDCG
    }

    Pass
    {
      Name "Deferred"
      Tags { "LightMode" = "ShadowCaster" }
      CGPROGRAM
      #pragma target 5.0
      #pragma vertex vert
      #pragma geometry geom
      #pragma fragment frag
      #define PASS_SHADOW_CASTER
      #include "TurbulentRainbowParticleCore.cginc"
      ENDCG
    }
  }
}