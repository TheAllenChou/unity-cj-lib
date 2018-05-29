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
  // GPU compute
  //-------------------------------------------------------------------------

  public class RandomNoiseVector
  {
    private static bool s_randomVecInit = false;
    private static ComputeShader s_randomVec;
    private static int s_randomVec2KernelId;
    private static int s_randomVec3KernelId;
    private static void InitRandomVec()
    {
      if (s_randomVecInit)
        return;

      s_randomVec = (ComputeShader) Resources.Load("RandomNoiseVectorCs");
      s_randomVec2KernelId = s_randomVec.FindKernel("RandomVec2");
      s_randomVec3KernelId = s_randomVec.FindKernel("RandomVec3");
    }

    private static void GetRandomVec2(out ComputeShader shader, out int kernelId)
    {
      InitRandomVec();
      shader = s_randomVec;
      kernelId = s_randomVec2KernelId;
    }

    private static void GetRandomVec3(out ComputeShader shader, out int kernelId)
    {
      InitRandomVec();
      shader = s_randomVec;
      kernelId = s_randomVec3KernelId;
    }

    public static void Compute(Vector2Array output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandomVec2(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, seed);
    }

    public static void Compute(Vector3Array output, int seed = 0)
    {
      ComputeShader shader;
      int kernelId;
      GetRandomVec3(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, seed);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute


    // CPU
    //-------------------------------------------------------------------------

    public static Vector2 GetVector2(float v, int seed = 0)
    {
      // TODO
      return new Vector2();
    }

    public static Vector2 GetVector2(Vector2 v, int seed = 0)
    {
      // TODO
      return new Vector2();
    }

    public static Vector3 GetVector3(float v, int seed = 0)
    {
      // TODO
      return new Vector3();
    }

    public static Vector3 GetVector3(Vector2 v, int seed = 0)
    {
      // TODO
      return new Vector3();
    }

    public static Vector3 GetVector3(Vector3 v, int seed = 0)
    {
      // TODO
      return new Vector3();
    }

    //-------------------------------------------------------------------------
    // end: CPU
  }
}
