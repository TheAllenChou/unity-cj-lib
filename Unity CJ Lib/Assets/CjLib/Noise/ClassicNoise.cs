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
  public class ClassicNoise
  {
    // common
    //-------------------------------------------------------------------------

    private static bool s_classicInit = false;
    private static ComputeShader s_classic;
    private static int s_classicGrid2KernelId;
    private static int s_classicGrid3KernelId;
    private static int s_classicCustom2KernelId;
    private static int s_classicCustom3KernelId;
    private static void InitClassic()
    {
      NoiseCommon.InitCsId();

      if (s_classicInit)
        return;

      s_classic = (ComputeShader) Resources.Load("ClassicNoiseCs");
      s_classicGrid2KernelId = s_classic.FindKernel("ClassicGrid2");
      s_classicGrid3KernelId = s_classic.FindKernel("ClassicGrid3");
      s_classicCustom2KernelId = s_classic.FindKernel("ClassicCustom2");
      s_classicCustom3KernelId = s_classic.FindKernel("ClassicCustom3");
    }

    private static void GetClassicGrid2(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classicGrid2KernelId;
    }

    private static void GetClassicGrid3(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classicGrid3KernelId;
    }

    private static void GetClassicCustom2(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classicCustom2KernelId;
    }

    private static void GetClassicCustom3(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classicCustom3KernelId;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(float[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = new float[] { scale, 1.0f, 1.0f };
      float[] aOffset = new float[] { offset, 0.0f, 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = new float[] { scale[0], scale[1], 1.0f };
      float[] aOffset = new float[] { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      float[] aScale = new float[] { scale[0], scale[1], scale[2] };
      float[] aOffset = new float[] { offset[0], offset[1], offset[2] };
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
      GetClassicCustom2(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[] input, float[] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicCustom3(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
