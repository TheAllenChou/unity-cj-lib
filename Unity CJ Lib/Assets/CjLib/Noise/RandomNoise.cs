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
  public class RandomNoise
  {
    private static bool s_randomInit = false;
    private static ComputeShader s_random;
    private static int s_random1KernelId;
    private static int s_random2KernelId;
    private static int s_random3KernelId;
    private static void InitRandom()
    {
      NoiseCommon.InitCsId();

      if (s_randomInit)
        return;

      s_random = (ComputeShader) Resources.Load("RandomNoiseCs");
      s_random1KernelId = s_random.FindKernel("Random1");
      s_random2KernelId = s_random.FindKernel("Random2");
      s_random3KernelId = s_random.FindKernel("Random3");
    }

    private static void GetRandom1(out ComputeShader shader, out int kernelId)
    {
      InitRandom();
      shader = s_random;
      kernelId = s_random1KernelId;
    }

    private static void GetRandom2(out ComputeShader shader, out int kernelId)
    {
      InitRandom();
      shader = s_random;
      kernelId = s_random2KernelId;
    }

    private static void GetRandom3(out ComputeShader shader, out int kernelId)
    {
      InitRandom();
      shader = s_random;
      kernelId = s_random3KernelId;
    }

    public static void Compute(float[] output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandom1(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float));
    }

    public static void Compute(float[,] output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandom2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float));
    }

    public static void Compute(float[,,] output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandom3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      NoiseCommon.Compute(output, shader, kernelId, seed, dimension, sizeof(float));
    }
  }
}
