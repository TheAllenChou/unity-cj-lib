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

    public static void Compute(FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicGrid2(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, scale.GetArray(), offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicCustom2(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicCustom3(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
