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
    // GPU compute
    //-------------------------------------------------------------------------

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

    //-------------------------------------------------------------------------
    // end: GPU compute


    // CPU
    //-------------------------------------------------------------------------

    public static float Get(float v, int seed = 0)
    {
      // return frac(sin(mod(s, 6.2831853)) * 43758.5453123);
      seed = NoiseCommon.JumbleSeed(seed);
      float r = Mathf.Sin(Mathf.Repeat(seed + v, 6.2831853f)) * 43758.5453123f;
      r = (r >= 0.0f) ? r : -r;
      r -= Mathf.Floor(r);
      return r;
    }

    public static float Get(Vector2 v, int seed = 0)
    {
      // float d = dot(s + 0.1234567, float2(1111112.9819837, 78.237173));
      seed = NoiseCommon.JumbleSeed(seed);
      float d = 
          (seed + v.x + 0.1234567f) * 1111112.9819837f 
        + (seed + v.y + 0.1234567f) * 78.237173f;

      // return frac(sin(m) * 43758.5453123);
      float r = Mathf.Sin(d) * 43758.5453123f;
      r = (r >= 0.0f) ? r : -r;
      r -= Mathf.Floor(r);
      return r;
    }

    public static float Get(Vector3 v, int seed)
    {
      // float d = dot(s + 0.1234567, float3(11112.9819837, 378.237173, 3971977.9173179));
      seed = NoiseCommon.JumbleSeed(seed);
      float d =
          (seed + v.x + 0.1234567f) * 1111112.9819837f
        + (seed + v.y + 0.1234567f) * 378.237173f
        + (seed + v.z + 0.1234567f) * 3971977.9173179f;

      // return frac(sin(m) * 43758.5453123);
      float r = Mathf.Sin(d) * 43758.5453123f;
      r = (r >= 0.0f) ? r : -r;
      r -= Mathf.Floor(r);
      return r;
    }

    //-------------------------------------------------------------------------
    // end: CPU
  }
}
