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
    private static int s_classic1KernelId;
    private static int s_classic2KernelId;
    private static int s_classic3KernelId;
    private static void InitClassic()
    {
      NoiseCommon.InitCsId();

      if (s_classicInit)
        return;

      s_classic = (ComputeShader) Resources.Load("ClassicNoiseCs");
      s_classic1KernelId = s_classic.FindKernel("Classic1");
      s_classic2KernelId = s_classic.FindKernel("Classic2");
      s_classic3KernelId = s_classic.FindKernel("Classic3");
    }

    private static void GetClassic1(out ComputeShader shader, out int kernelId)
    {
      InitClassic();
      shader = s_classic;
      kernelId = s_classic1KernelId;
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

    public static void Compute(float[] output, float scale, float offset, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic1(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = new float[] { scale };
      float[] aOffset = new float[] { offset };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), aScale, aOffset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), scale, offset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassic3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), scale, offset, numOctaves, octaveOffsetFactor);
    }
  }
}
