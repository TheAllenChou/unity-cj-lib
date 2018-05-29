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
    // common
    //-------------------------------------------------------------------------

    private static bool s_randomInit = false;
    private static ComputeShader s_random;
    private static int s_random1KernelId;
    private static int s_random2KernelId;
    private static int s_random3KernelId;
    private static void InitRandom()
    {
      if (s_randomInit)
        return;

      s_random = (ComputeShader) Resources.Load("RandomNoiseCs");
      s_random1KernelId = s_random.FindKernel("RandomGrid1");
      s_random2KernelId = s_random.FindKernel("RandomGrid2");
      s_random3KernelId = s_random.FindKernel("RandomGrid3");
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

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(FloatArray output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandom1(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, seed);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // CPU
    //-------------------------------------------------------------------------

    public static float Get(float v, int seed = 0)
    {
      // return frac(sin(mod(s, 6.2831853)) * 43758.5453123);
      float s = NoiseCommon.JumbleSeed(seed);
      float r = Mathf.Sin(Mathf.Repeat(s + v, 6.2831853f)) * 43758.5453123f;
      r = (r >= 0.0f) ? r : -r;
      r -= Mathf.Floor(r);
      return r;
    }

    public static float Get(Vector2 v, int seed = 0)
    {
      // float d = dot(s + 0.1234567, float2(1111112.9819837, 78.237173));
      float s = NoiseCommon.JumbleSeed(seed);
      float d = 
          (s + v.x + 0.1234567f) * 1111112.9819837f 
        + (s + v.y + 0.1234567f) * 78.237173f;

      // return frac(sin(m) * 43758.5453123);
      float r = Mathf.Sin(d) * 43758.5453123f;
      r = (r >= 0.0f) ? r : -r;
      r -= Mathf.Floor(r);
      return r;
    }

    public static float Get(Vector3 v, int seed = 0)
    {
      // float d = dot(s + 0.1234567, float3(11112.9819837, 378.237173, 3971977.9173179));
      float s = NoiseCommon.JumbleSeed(seed);
      float d =
          (s + v.x + 0.1234567f) * 1111112.9819837f
        + (s + v.y + 0.1234567f) * 378.237173f
        + (s + v.z + 0.1234567f) * 3971977.9173179f;

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
