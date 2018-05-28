/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System;

using UnityEngine;

using CjLib;

public class NoiseGpuComputeAndCpuTest : MonoBehaviour
{
  public enum NoiseType
  {
    Classic1D, 
    Classic2D, 
    Classic3D, 
    ClassicPeriodic1D, 
    ClassicPeriodic2D, 
    ClassicPeriodic3D, 
    Random1D, 
    Random2D, 
    Random3D, 
    RandomVector1DVec2, 
    RandomVector2DVec2, 
    RandomVector1DVec3, 
    RandomVector2DVec3, 
    RandomVector3DVec3, 
    Simplex1D, 
    Simplex2D, 
    Simplex3D, 
    SimplexGradient1DVec2, 
    SimplexGradient2DVec2, 
    SimplexGradient1DVec3, 
    SimplexGradient2DVec3, 
    SimplexGradient3DVec3, 
  }

  public enum Mode
  {
    kGpuComputeGridSamples, 
    kGpuComputeCustomSamples, 
    kCpu, 
  }

  public NoiseType m_noiseType = NoiseType.Simplex3D;
  public Mode m_mode = Mode.kGpuComputeGridSamples;
  public int m_numOctaves = 1;
  public float m_octaveOffsetFactor = 1.2f;
  public Color m_color = Color.white;
  public float m_elementSize = 0.15f;
  public int m_gridExtent = 6;

  private float[] m_drawDimension = new float[] { 2.0f, 2.0f, 2.0f };

  void Update()
  {
    float[] offset = new float[] { 0.5f * Time.time, 0.25f * Time.time, 0.1f * Time.time };
    int seed = (int) Mathf.Floor(2.0f * Time.time);
    float[] output1 = new float[m_gridExtent];
    float[] output1Sqr = new float[m_gridExtent * m_gridExtent];
    float[] output1Cub = new float[m_gridExtent * m_gridExtent * m_gridExtent];
    float[,] output2 = new float[m_gridExtent, m_gridExtent];
    float[,,] output3 = new float[m_gridExtent, m_gridExtent, m_gridExtent];
    Vector2[] output1v2 = new Vector2[m_gridExtent];
    Vector2[] output1v2Sqr = new Vector2[m_gridExtent * m_gridExtent];
    Vector2[,] output2v2 = new Vector2[m_gridExtent, m_gridExtent];
    Vector3[] output1v3 = new Vector3[m_gridExtent];
    Vector3[] output1v3Sqr = new Vector3[m_gridExtent * m_gridExtent];
    Vector3[] output1v3Cub = new Vector3[m_gridExtent * m_gridExtent * m_gridExtent];
    Vector3[,] output2v3 = new Vector3[m_gridExtent, m_gridExtent];
    Vector3[,,] output3v3 = new Vector3[m_gridExtent, m_gridExtent, m_gridExtent];
    float[] scale = new float[] { 3.0f, 3.0f, 3.0f };
    float[] period = new float[] { 0.15f, 0.15f, 0.15f };

    Vector2[] input1v2 = new Vector2[m_gridExtent];
    Vector2[] input1v2Sqr = new Vector2[m_gridExtent * m_gridExtent];
    Vector3[] input1v3 = new Vector3[m_gridExtent];
    Vector3[] input1v3Sqr = new Vector3[m_gridExtent * m_gridExtent];
    Vector3[] input1v3Cub = new Vector3[m_gridExtent * m_gridExtent * m_gridExtent];
    for (int z = 0; z < m_gridExtent; ++z)
      for (int y = 0; y < m_gridExtent; ++y)
        for (int x = 0; x < m_gridExtent; ++x)
        {
          int i = ((z * m_gridExtent) + y) * m_gridExtent + x;
          if (z == 0)
          {
            if (y == 0)
            {
              Vector3 p1 = new Vector3(x, y, z);
              input1v2[i].Set(p1.x, p1.y);
              input1v3[i] = p1;
            }

            Vector3 p2 = new Vector3(x, y, z);
            input1v2Sqr[i].Set(p2.x, p2.y);
            input1v3Sqr[i] = p2;
          }

          Vector3 p3 = new Vector3(x, y, z);
          input1v3Cub[i] = p3;
        }

    switch (m_noiseType)
    {
      case NoiseType.Classic1D:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoise.Compute(output1, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
            Draw(output1);
            break;
          case Mode.kGpuComputeCustomSamples:
            ClassicNoise.Compute(input1v2, output1, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2, output1);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.Classic2D:
        offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoise.Compute(output2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output2);
            break;
          case Mode.kGpuComputeCustomSamples:
            ClassicNoise.Compute(input1v2Sqr, output1Sqr, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2Sqr, output1Sqr);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.Classic3D:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoise.Compute(output3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output3);
            break;
          case Mode.kGpuComputeCustomSamples:
            ClassicNoise.Compute(input1v3Cub, output1Cub, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3Cub, output1Cub);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.ClassicPeriodic1D:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoisePeriodic.Compute(output1, scale[0], offset[0], period[0], m_numOctaves, m_octaveOffsetFactor);
            Draw(output1);
            break;
          case Mode.kGpuComputeCustomSamples:
            ClassicNoisePeriodic.Compute(input1v2, output1, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2, output1);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.ClassicPeriodic2D:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoisePeriodic.Compute(output2, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
            Draw(output2);
            break;
          case Mode.kGpuComputeCustomSamples:
            offset[2] = 0.0f;
            ClassicNoisePeriodic.Compute(input1v2Sqr, output1Sqr, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2Sqr, output1Sqr);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.ClassicPeriodic3D:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            ClassicNoisePeriodic.Compute(output3, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
            Draw(output3);
            break;
          case Mode.kGpuComputeCustomSamples:
            ClassicNoisePeriodic.Compute(input1v3Cub, output1Cub, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3Cub, output1Cub);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.Random1D:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
          case Mode.kGpuComputeCustomSamples:  // pointless
            RandomNoise.Compute(output1, seed);
            Draw(output1);
            break;
          case Mode.kCpu:
            for (int x = 0; x < output1.GetLength(0); ++x)
              output1[x] = RandomNoise.Get(x, seed);
            Draw(output1);
            break;
        }
        break;
      case NoiseType.Random2D:
        offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
          case Mode.kGpuComputeCustomSamples:  // pointless
            RandomNoise.Compute(output2, seed);
            Draw(output2);
            break;
          case Mode.kCpu:
            for (int y = 0; y < output2.GetLength(1); ++y)
              for (int x = 0; x < output2.GetLength(0);  ++x)
                output2[y, x] = RandomNoise.Get(new Vector2(x, y), seed);
            Draw(output2);
            break;
        }
        break;
      case NoiseType.Random3D:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
          case Mode.kGpuComputeCustomSamples: // pointless
            RandomNoise.Compute(output3, seed);
            break;
          case Mode.kCpu:
            for (int z = 0; z < output3.GetLength(2); ++z)
              for (int y = 0; y < output3.GetLength(1); ++y)
                for (int x = 0; x < output3.GetLength(0); ++x)
                  output3[z, y, x] = RandomNoise.Get(new Vector3(x, y, z), seed);
            break;
        }
        Draw(output3);
        break;
      case NoiseType.RandomVector1DVec2:
        offset[1] = offset[2] = 0.0f;
        RandomNoiseVector.Compute(output1v2, seed);
        Draw(output1v2);
        break;
      case NoiseType.RandomVector2DVec2:
        offset[2] = 0.0f;
        RandomNoiseVector.Compute(output2v2, seed);
        Draw(output2v2);
        break;
      case NoiseType.RandomVector1DVec3:
        offset[1] = offset[2] = 0.0f;
        RandomNoiseVector.Compute(output1v3, seed);
        Draw(output1v3);
        break;
      case NoiseType.RandomVector2DVec3:
        offset[2] = 0.0f;
        RandomNoiseVector.Compute(output2v3, seed);
        Draw(output2v3);
        break;
      case NoiseType.RandomVector3DVec3:
        RandomNoiseVector.Compute(output3v3, seed);
        Draw(output3v3);
        break;
      case NoiseType.Simplex1D:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoise.Compute(output1, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
            Draw(output1);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoise.Compute(input1v2, output1, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2, output1);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.Simplex2D:
        offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoise.Compute(output2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output2);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoise.Compute(input1v2Sqr, output1Sqr, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2Sqr, output1Sqr);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.Simplex3D:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoise.Compute(output3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output3);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoise.Compute(input1v3Cub, output1Cub, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3Cub, output1Cub);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.SimplexGradient1DVec2:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoiseGradient.Compute(output1v2, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
            Draw(output1v2);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoiseGradient.Compute(input1v2, output1v2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2, output1v2);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.SimplexGradient2DVec2:
        offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoiseGradient.Compute(output2v2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output2v2);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoiseGradient.Compute(input1v2Sqr, output1v2Sqr, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v2Sqr, output1v2Sqr);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.SimplexGradient1DVec3:
        offset[1] = offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoiseGradient.Compute(output1v3, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
            Draw(output1v3);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoiseGradient.Compute(input1v3, output1v3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3, output1v3);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.SimplexGradient2DVec3:
        offset[2] = 0.0f;
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoiseGradient.Compute(output2v3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output2v3);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoiseGradient.Compute(input1v3Sqr, output1v3Sqr, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3Sqr, output1v3Sqr);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
      case NoiseType.SimplexGradient3DVec3:
        switch (m_mode)
        {
          case Mode.kGpuComputeGridSamples:
            SimplexNoiseGradient.Compute(output3v3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(output3v3);
            break;
          case Mode.kGpuComputeCustomSamples:
            SimplexNoiseGradient.Compute(input1v3Cub, output1v3Cub, scale, offset, m_numOctaves, m_octaveOffsetFactor);
            Draw(input1v3Cub, output1v3Cub);
            break;
          case Mode.kCpu:
            // TODO
            break;
        }
        break;
    }
  }

  Vector3 ComputePoint(int x)
  {
    return
      new Vector3
      (
        ((((float)x) / m_gridExtent) - 0.5f) * m_drawDimension[0],
        0.0f,
        0.0f
      );
  }

  Vector3 ComputePoint(int x, int y)
  {
    return
      new Vector3
      (
        ((((float)x) / m_gridExtent) - 0.5f) * m_drawDimension[0],
        ((((float)y) / m_gridExtent) - 0.5f) * m_drawDimension[1],
        0.0f
      );
  }

  Vector3 ComputePoint(int x, int y, int z)
  {
    return
      new Vector3
      (
        ((((float)x) / m_gridExtent) - 0.5f) * m_drawDimension[0],
        ((((float)y) / m_gridExtent) - 0.5f) * m_drawDimension[1],
        ((((float)z) / m_gridExtent) - 0.5f) * m_drawDimension[2]
      );
  }

  Color ComputeColor(Vector3 p, Vector3 near, Vector3 far, float farMult)
  {
    Vector3 c = Camera.main.transform.position;
    Vector3 v = (far - c).normalized;
    float n = Vector3.Dot(v, near - c) + 0.5f;
    float f = Vector3.Dot(v, far - c) - 0.5f;
    float d = Vector3.Dot(v, p - c);
    float t = Mathf.Clamp01((f - d) / (f - n));
    float m = Mathf.Lerp(1.0f, farMult, t);
    return new Color(m_color.r * m, m_color.g * m, m_color.b * m, m_color.a);
  }

  private void Draw(float[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
    {
      Vector3 p = ComputePoint(x);
      Color c = ComputeColor(p, ComputePoint(0), ComputePoint(m_gridExtent - 1), 0.8f);
      DebugUtil.DrawSphere(p, m_elementSize * value[x], 2, 4, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(float[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
      {
        Vector3 p = ComputePoint(x, y);
        Color c = ComputeColor(p, ComputePoint(0, 0), ComputePoint(m_gridExtent - 1, m_gridExtent - 1), 0.6f);
        DebugUtil.DrawSphere(p, m_elementSize * value[y, x], 2, 4, c, true, DebugUtil.Style.FlatShaded);
      }
  }

  private void Draw(float[,,] value)
  {
    for (int z = 0; z < value.GetLength(2); ++z)
      for (int y = 0; y < value.GetLength(1); ++y)
        for (int x = 0; x < value.GetLength(0); ++x)
        {
          Vector3 p = ComputePoint(x, y, z);
          Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), 0.1f);
          DebugUtil.DrawSphere(p, m_elementSize * value[z, y, x], 2, 4, c, true, DebugUtil.Style.FlatShaded);
        }
  }

  private void Draw(Vector2[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
    {
      Vector3 p = ComputePoint(x);
      Color c = ComputeColor(p, ComputePoint(0), ComputePoint(m_gridExtent - 1), 0.8f);
      DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[x].x, value[x].y, 0.0f)), 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector2[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
      {
        Vector3 p = ComputePoint(x, y);
        Color c = ComputeColor(p, ComputePoint(0, 0), ComputePoint(m_gridExtent - 1, m_gridExtent - 1), 0.6f);
        DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[y, x].x, value[y, x].y, 0.0f)), 0.05f, c, true, DebugUtil.Style.FlatShaded);
      }
  }

  private void Draw(Vector3[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
    {
      Vector3 p = ComputePoint(x);
      Color c = ComputeColor(p, ComputePoint(0), ComputePoint(m_gridExtent - 1), 0.8f);
      DebugUtil.DrawArrow(p, p + m_elementSize * value[x], 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector3[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
      {
        Vector3 p = ComputePoint(x, y);
        Color c = ComputeColor(p, ComputePoint(0, 0), ComputePoint(m_gridExtent - 1, m_gridExtent - 1), 0.6f);
        DebugUtil.DrawArrow(p, p + m_elementSize * value[y, x], 0.05f, c, true, DebugUtil.Style.FlatShaded);
      }
  }

  private void Draw(Vector3[,,] value)
  {
    for (int z = 0; z < value.GetLength(2); ++z)
      for (int y = 0; y < value.GetLength(1); ++y)
        for (int x = 0; x < value.GetLength(0); ++x)
        {
          Vector3 p = ComputePoint(x, y, z);
          Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), 0.1f);
          DebugUtil.DrawArrow(p, p + m_elementSize * value[z, y, x], 0.05f, c, true, DebugUtil.Style.FlatShaded);
        }
  }

  private void Draw(Vector2[] input, float[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult =
        (input.Length == m_gridExtent)
          ? 0.8f
          : 0.6f;
      Vector3 p =
        (input.Length == m_gridExtent)
          ? ComputePoint((int)input[i].x)
          : ComputePoint((int)input[i].x, (int)input[i].y);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawSphere(p, m_elementSize * value[i], 2, 4, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector3[] input, float[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult =
        (input.Length == m_gridExtent)
          ? 0.8f
          : (input.Length == m_gridExtent * m_gridExtent)
            ? 0.6f
            : 0.1f;
      Vector3 p =
        (input.Length == m_gridExtent)
          ? ComputePoint((int)input[i].x)
          : (input.Length == m_gridExtent * m_gridExtent)
            ? ComputePoint((int)input[i].x, (int)input[i].y)
            : ComputePoint((int)input[i].x, (int)input[i].y, (int)input[i].z);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawSphere(p, m_elementSize * value[i], 2, 4, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector2[] input, Vector2[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult = 
        (input.Length == m_gridExtent) 
          ? 0.8f 
          : 0.6f;
      Vector3 p = 
        (input.Length == m_gridExtent) 
          ? ComputePoint((int) input[i].x) 
          : ComputePoint((int) input[i].x, (int) input[i].y);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[i].x, value[i].y, 0.0f)), 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector3[] input, Vector2[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult = 
        (input.Length == m_gridExtent) 
          ? 0.8f 
          : (input.Length == m_gridExtent * m_gridExtent)
            ? 0.6f
            : 0.1f;
      Vector3 p = 
        (input.Length == m_gridExtent) 
          ? ComputePoint((int) input[i].x)
          : (input.Length == m_gridExtent * m_gridExtent)
            ? ComputePoint((int) input[i].x, (int) input[i].y)
            : ComputePoint((int) input[i].x, (int) input[i].y, (int) input[i].z);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[i].x, value[i].y, 0.0f)), 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector2[] input, Vector3[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult = 
        (input.Length == m_gridExtent) 
          ? 0.8f 
          : 0.6f;
      Vector3 p = 
        (input.Length == m_gridExtent) 
          ? ComputePoint((int) input[i].x) 
          : ComputePoint((int) input[i].x, (int) input[i].y);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawArrow(p, p + m_elementSize * value[i], 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector3[] input, Vector3[] value)
  {
    for (int i = 0; i < input.Length; ++i)
    {
      float farMult = 
        (input.Length == m_gridExtent) 
          ? 0.8f 
          : (input.Length == m_gridExtent * m_gridExtent)
            ? 0.6f
            : 0.1f;
      Vector3 p = 
        (input.Length == m_gridExtent) 
          ? ComputePoint((int) input[i].x)
          : (input.Length == m_gridExtent * m_gridExtent)
            ? ComputePoint((int) input[i].x, (int) input[i].y)
            : ComputePoint((int) input[i].x, (int) input[i].y, (int) input[i].z);
      Color c = ComputeColor(p, ComputePoint(0, 0, m_gridExtent - 1), ComputePoint(m_gridExtent - 1, m_gridExtent - 1, 0), farMult);
      DebugUtil.DrawArrow(p, p + m_elementSize * value[i], 0.05f, c, true, DebugUtil.Style.FlatShaded);
    }
  }
}
