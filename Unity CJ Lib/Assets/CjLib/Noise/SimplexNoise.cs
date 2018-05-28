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
      NoiseCommon.InitCsId();

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

    public static void Compute(float[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = { scale, 1.0f, 1.0f };
      float[] aOffset = { offset, 0.0f, 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2[] input, float[] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexCustom2(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[] input, float[] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexCustom3(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
