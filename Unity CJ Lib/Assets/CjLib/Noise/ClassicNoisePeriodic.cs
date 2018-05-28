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
      NoiseCommon.InitCsId();

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

    public static void Compute(float[] output, float scale, float offset, float period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), 1, 1 };
      float[] aScale = { scale, 1.0f, 1.0f };
      float[] aOffset = { offset, 0.0f, 0.0f };
      float[] aPeriod = new float[] { period, 1.0f, 1.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, aPeriod, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicGrid2(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), 1 };
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      float[] aPeriod = { period[0], period[1], 1.0f };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, aPeriod, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(float[,,] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicGrid3(out shader, out kernelId);
      int[] dimension = new int[] { output.GetLength(0), output.GetLength(1), output.GetLength(2) };
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      float[] aPeriod = { period[0], period[1], period[2] };
      NoiseCommon.Compute(output, shader, kernelId, dimension, aScale, aOffset, aPeriod, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    public static void Compute(Vector2[] input, float[] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicCustom2(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], 1.0f };
      float[] aOffset = { offset[0], offset[1], 0.0f };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, period, numOctaves, octaveOffsetFactor);
    }

    public static void Compute(Vector3[] input, float[] output, float[] scale, float[] offset, float[] period, int numOctaves, float octaveOffsetFactor)
    {
      ComputeShader shader;
      int kernelId;
      GetClassicPeriodicCustom3(out shader, out kernelId);
      float[] aScale = { scale[0], scale[1], scale[2] };
      float[] aOffset = { offset[0], offset[1], offset[2] };
      NoiseCommon.Compute(input, output, shader, kernelId, aScale, aOffset, period, numOctaves, octaveOffsetFactor);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
