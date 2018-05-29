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
    // common
    //-------------------------------------------------------------------------

    private static bool s_classicPeriodicInit = false;
    private static ComputeShader s_classicPeriodic;
    private static int s_classicPeriodicGrid2KernelId;
    private static int s_classicPeriodicGrid3KernelId;
    private static int s_classicPeriodicCustom2KernelId;
    private static int s_classicPeriodicCustom3KernelId;
    private static void InitClassicPeriodic()
    {
      if (s_classicPeriodicInit)
        return;

      s_classicPeriodic = (ComputeShader) Resources.Load("ClassicNoisePeriodicCs");
      s_classicPeriodicGrid2KernelId = s_classicPeriodic.FindKernel("ClassicPeriodicGrid2");
      s_classicPeriodicGrid3KernelId = s_classicPeriodic.FindKernel("ClassicPeriodicGrid3");
      s_classicPeriodicCustom2KernelId = s_classicPeriodic.FindKernel("ClassicPeriodicGrid2");
      s_classicPeriodicCustom3KernelId = s_classicPeriodic.FindKernel("ClassicPeriodicGrid3");
    }

    private static void GetClassicPeriodicGrid2(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_classicPeriodic;
      kernelId = s_classicPeriodicGrid2KernelId;
    }

    private static void GetClassicPeriodicGrid3(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_classicPeriodic;
      kernelId = s_classicPeriodicGrid3KernelId;
    }

    private static void GetClassicPeriodicCustom2(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_classicPeriodic;
      kernelId = s_classicPeriodicCustom2KernelId;
    }

    private static void GetClassicPeriodicCustom3(out ComputeShader shader, out int kernelId)
    {
      InitClassicPeriodic();
      shader = s_classicPeriodic;
      kernelId = s_classicPeriodicCustom3KernelId;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    public static void Compute(FloatArray output, NoiseScale scale, NoiseOffset offset, NoisePeriod period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicGrid2(out shader, out kernelId);
      NoiseCommon.Compute(output, shader, kernelId, scale, offset, period, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, NoisePeriod period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicCustom2(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, period, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3Array input, FloatArray output, NoiseScale scale, NoiseOffset offset, NoisePeriod period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicCustom3(out shader, out kernelId);
      NoiseCommon.Compute(input, output, shader, kernelId, scale, offset, period, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
