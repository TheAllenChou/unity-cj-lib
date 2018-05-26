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
  public class ClassicNoisePeriodic
  {
    private static bool s_simplexInit = false;
    private static ComputeShader s_simplex;
    private static int s_simplex2KernelId;
    private static int s_simplex3KernelId;
    private static void InitClassicPeriodic()
    {
      NoiseCommon.InitCsId();

      if (s_simplexInit)
        return;

      s_simplex = (ComputeShader) Resources.Load("ClassicNoisePeriodicCs");
      s_simplex2KernelId = s_simplex.FindKernel("ClassicPeriodic2");
      s_simplex3KernelId = s_simplex.FindKernel("ClassicPeriodic3");
    }

    private static void GetClassicPeriodic2(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_simplex;
      kernelId = s_simplex2KernelId;
    }

    private static void GetClassicPeriodic3(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_simplex;
      kernelId = s_simplex3KernelId;
    }

    public static void Compute(float[] output, float scale, float offset, float period, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodic2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = new float[] { scale };
      float[] aOffset = new float[] { offset };
      float[] aPeriod = new float[] { period };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), aScale, aOffset, aPeriod, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodic2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), scale, offset, period, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodic3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float), scale, offset, period, numOctaves, octaveOffsetFactor);
    }
  }
}
