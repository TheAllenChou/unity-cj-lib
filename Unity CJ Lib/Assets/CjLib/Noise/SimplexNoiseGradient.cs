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
    private static int s_simplexGradient2KernelId;
    private static int s_simplexGradient3KernelId;
    private static int s_simplexGradientGrid2KernelId;
    private static int s_simplexGradientGrid3KernelId;
    private static void InitSimplex()
    {
      NoiseCommon.InitCsId();

      if (s_simplexGradientInit)
        return;

      s_simplexGradient = (ComputeShader) Resources.Load("SimplexNoiseGradientCs");
      s_simplexGradient2KernelId = s_simplexGradient.FindKernel("SimplexGradient2");
      s_simplexGradient3KernelId = s_simplexGradient.FindKernel("SimplexGradient3");
      s_simplexGradientGrid2KernelId = s_simplexGradient.FindKernel("SimplexGradientGrid2");
      s_simplexGradientGrid3KernelId = s_simplexGradient.FindKernel("SimplexGradientGrid3");
    }

    private static void GetSimplexGradient2(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradient2KernelId;
    }

    private static void GetSimplexGradient3(out ComputeShader shader, out int kernelId)
    {
      InitSimplex();
      shader = s_simplexGradient;
      kernelId = s_simplexGradient3KernelId;
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

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = { scale, 1.0f, 1.0f };
      float[] aOffset = { offset, 0.0f, 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector2[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = { scale, 1.0f, 1.0f };
      float[] aOffset = { offset, 0.0f, 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[,,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradientGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2[] input, Vector2[] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradient2(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[] input, Vector3[] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetSimplexGradient3(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
