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
  public class SimplexNoise
  {
    // common
    //-------------------------------------------------------------------------

    private static bool s_simplexInit = false;
    private static ComputeShader s_simplex;
    private static int s_simplexGrid2KernelId;
    private static int s_simplexGrid3KernelId;
    private static int s_simplexCustom2KernelId;
    private static int s_simplexCustom3KernelId;
    private static void InitSimplex()
    {
      if (s_simplexInit)
        return;

      s_simplex = (ComputeShader) Resources.Load("SimplexNoiseCs");
      s_simplexGrid2KernelId = s_simplex.FindKernel("SimplexGrid2");
      s_simplexGrid3KernelId = s_simplex.FindKernel("SimplexGrid3");
      s_simplexCustom2KernelId = s_simplex.FindKernel("SimplexCustom2");
      s_simplexCustom3KernelId = s_simplex.FindKernel("SimplexCustom3");
    }

    private static void GetSimplexGrid2(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplex;
      kernelId = s_simplexGrid2KernelId;
    }

    private static void GetSimplexGrid3(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplex;
      kernelId = s_simplexGrid3KernelId;
    }

    private static void GetSimplexCustom2(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplex;
      kernelId = s_simplexCustom2KernelId;
    }

    private static void GetSimplexCustom3(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplex;
      kernelId = s_simplexCustom3KernelId;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGrid2(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexCustom2(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexCustom3(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples


    // CPU
    //-------------------------------------------------------------------------

    

    //-------------------------------------------------------------------------
    // end: CPU
  }
}
