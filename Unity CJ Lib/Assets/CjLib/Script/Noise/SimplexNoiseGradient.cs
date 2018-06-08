/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;

namespace CjLib
{
  public class SimplexNoiseGradient
  {
    // common
    //-------------------------------------------------------------------------

    private static bool s_simplexGradientInit = false;
    private static ComputeShader s_simplexGradient;
    private static int s_simplexGradientGrid2KernelId;
    private static int s_simplexGradientGrid3KernelId;
    private static int s_simplexGradientCustom2KernelId;
    private static int s_simplexGradientCustum3KernelId;
    private static void InitSimplex()
    {
      if (s_simplexGradientInit)
        return;

      s_simplexGradient = (ComputeShader) Resources.Load("SimplexNoiseGradientCs");
      s_simplexGradientGrid2KernelId = s_simplexGradient.FindKernel("SimplexGradientGrid2");
      s_simplexGradientGrid3KernelId = s_simplexGradient.FindKernel("SimplexGradientGrid3");
      s_simplexGradientCustom2KernelId = s_simplexGradient.FindKernel("SimplexGradientCustom2");
      s_simplexGradientCustum3KernelId = s_simplexGradient.FindKernel("SimplexGradientCustom3");
    }

    private static void GetSimplexGradientGrid2(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradientGrid2KernelId;
    }

    private static void GetSimplexGradientGrid3(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradientGrid3KernelId;
    }

    private static void GetSimplexGradientCustom2(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradientCustom2KernelId;
    }

    private static void GetSimplexGradientCustom3(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradientCustum3KernelId;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2Array output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid2(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3Array output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid3(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2Array input,  Vector2Array output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientCustom2(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3Array input,  Vector3Array output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientCustom3(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
