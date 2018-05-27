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
    private static bool s_classicInit = false;
    private static ComputeShader s_classic;
    private static int s_classic2KernelId;
    private static int s_classic3KernelId;
    private static void InitClassic()
    {
      NoiseCommon.InitCsId();

      if (s_classicInit)
        return;

      s_classic = (ComputeShader) Resources.Load("ClassicNoiseCs");
      s_classic2KernelId = s_classic.FindKernel("Classic2");
      s_classic3KernelId = s_classic.FindKernel("Classic3");
    }

    private static void GetClassic2(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classic2KernelId;
    }

    private static void GetClassic3(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classic3KernelId;
    }

    public static void Compute(float[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = new float[] { scale, 1.0f, 1.0f };
      float[] aOffset = new float[] { offset, 0.0f, 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, sizeof(float), aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = new float[] { scale[0], scale[1], 1.0f };
      float[] aOffset = new float[] { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, sizeof(float), aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      float[] aScale = new float[] { scale[0], scale[1], scale[2] };
      float[] aOffset = new float[] { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(output, shader, kernelId, dimension, sizeof(float), aScale, aOffset, numOctaves, octaveOffsetFactor);
    }
  }
}
