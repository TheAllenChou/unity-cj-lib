/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

  Based on Noise Shader Library for Unity
  https://github.com/keijiro/NoiseShader

  Original work (webgl-noise) Copyright (C) 2011 Ashima Arts.
  Translation and modification was made by Keijiro Takahashi.

    Description : Array and textureless GLSL 2D simplex noise function.
        Author  : Ian McEwan, Ashima Arts.
    Maintainer  : ijm
        Lastmod : 20110822 (ijm)
        License : Copyright (C) 2011 Ashima Arts. All rights reserved.
                  Distributed under the MIT License. See LICENSE file.
                  https://github.com/ashima/webgl-noise
*/
/******************************************************************************/

using System;

using UnityEngine;

using CjLib;

[ExecuteInEditMode]
public class NoiseComputeShaderTest : MonoBehaviour
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

  public NoiseType m_noiseType = NoiseType.Simplex3D;
  public int m_numOctaves = 2;
  public float m_octaveOffsetFactor = 1.2f;
  public Color m_color = Color.white;
  public float m_elementSize = 0.15f;

  private float[] m_drawDimension = new float[] { 2.0f, 2.0f, 2.0f };

  void Update()
  {
    float[] offset = new float[] { 0.5f * Time.time, 0.0f * Time.time, 0.0f * Time.time };
    int extent = 6;
    float[] output1 = new float[extent];
    float[,] output2 = new float[extent, extent];
    float[,,] output3 = new float[extent, extent, extent];
    Vector2[] output1v2 = new Vector2[extent];
    Vector2[,] output2v2 = new Vector2[extent, extent];
    Vector3[] output1v3 = new Vector3[extent];
    Vector3[,] output2v3 = new Vector3[extent, extent];
    Vector3[,,] output3v3 = new Vector3[extent, extent, extent];
    float[] scale = new float[] { 3.0f, 3.0f, 3.0f };
    float[] period = new float[] { 0.15f, 0.15f, 0.15f };

    switch (m_noiseType)
    {
      case NoiseType.Classic1D:
        ClassicNoise.Compute(output1, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
        Draw(output1);
        break;
      case NoiseType.Classic2D:
        ClassicNoise.Compute(output2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output2);
        break;
      case NoiseType.Classic3D:
        ClassicNoise.Compute(output3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output3);
        break;
      case NoiseType.ClassicPeriodic1D:
        ClassicNoisePeriodic.Compute(output1, scale[0], offset[0], period[0], m_numOctaves, m_octaveOffsetFactor);
        Draw(output1);
        break;
      case NoiseType.ClassicPeriodic2D:
        ClassicNoisePeriodic.Compute(output2, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
        Draw(output2);
        break;
      case NoiseType.ClassicPeriodic3D:
        ClassicNoisePeriodic.Compute(output3, scale, offset, period, m_numOctaves, m_octaveOffsetFactor);
        Draw(output3);
        break;
      case NoiseType.Random1D:
        RandomNoise.Compute(output1, Time.frameCount);
        Draw(output1);
        break;
      case NoiseType.Random2D:
        RandomNoise.Compute(output2, Time.frameCount);
        Draw(output2);
        break;
      case NoiseType.Random3D:
        RandomNoise.Compute(output3, Time.frameCount);
        Draw(output3);
        break;
      case NoiseType.RandomVector1DVec2:
        RandomNoiseVector.Compute(output1v2, Time.frameCount);
        Draw(output1v2);
        break;
      case NoiseType.RandomVector2DVec2:
        RandomNoiseVector.Compute(output2v2, Time.frameCount);
        Draw(output2v2);
        break;
      case NoiseType.RandomVector1DVec3:
        RandomNoiseVector.Compute(output1v3, Time.frameCount);
        Draw(output1v3);
        break;
      case NoiseType.RandomVector2DVec3:
        RandomNoiseVector.Compute(output2v3, Time.frameCount);
        Draw(output2v3);
        break;
      case NoiseType.RandomVector3DVec3:
        RandomNoiseVector.Compute(output3v3, Time.frameCount);
        Draw(output3v3);
        break;
      case NoiseType.Simplex1D:
        SimplexNoise.Compute(output1, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
        Draw(output1);
        break;
      case NoiseType.Simplex2D:
        SimplexNoise.Compute(output2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output2);
        break;
      case NoiseType.Simplex3D:
        SimplexNoise.Compute(output3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output3);
        break;
      case NoiseType.SimplexGradient1DVec2:
        SimplexNoiseGradient.Compute(output1v2, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
        Draw(output1v2);
        break;
      case NoiseType.SimplexGradient2DVec2:
        SimplexNoiseGradient.Compute(output2v2, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output2v2);
        break;
      case NoiseType.SimplexGradient1DVec3:
        SimplexNoiseGradient.Compute(output1v3, scale[0], offset[0], m_numOctaves, m_octaveOffsetFactor);
        Draw(output1v3);
        break;
      case NoiseType.SimplexGradient2DVec3:
        SimplexNoiseGradient.Compute(output2v3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output2v3);
        break;
      case NoiseType.SimplexGradient3DVec3:
        SimplexNoiseGradient.Compute(output3v3, scale, offset, m_numOctaves, m_octaveOffsetFactor);
        Draw(output3v3);
        break;
    }
  }

  Vector3 ComputePoint(Array value, int x)
  {
    return
      new Vector3
      (
        ((((float)x) / value.GetLength(0)) - 0.5f) * m_drawDimension[0], 
        0.0f, 
        0.0f
      );
  }

  Vector3 ComputePoint(Array value, int x, int y)
  {
    return 
      new Vector3
      (
        ((((float) x) / value.GetLength(0)) - 0.5f) * m_drawDimension[0], 
        ((((float) y) / value.GetLength(1)) - 0.5f) * m_drawDimension[1], 
        0.0f
      );
  }

  Vector3 ComputePoint(Array value, int x, int y, int z)
  {
    return
      new Vector3
      (
        ((((float)x) / value.GetLength(0)) - 0.5f) * m_drawDimension[0], 
        ((((float)y) / value.GetLength(1)) - 0.5f) * m_drawDimension[1], 
        ((((float)z) / value.GetLength(2)) - 0.5f) * m_drawDimension[2]
      );
  }

  private void Draw(float[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
      DebugUtil.DrawSphere(ComputePoint(value, x), m_elementSize * value[x], 2, 4, m_color, true, DebugUtil.Style.FlatShaded);
  }

  private void Draw(float[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
        DebugUtil.DrawSphere(ComputePoint(value, x, y), m_elementSize * value[y, x], 2, 4, m_color, true, DebugUtil.Style.FlatShaded);
  }

  private void Draw(float[,,] value)
  {
    for (int z = 0; z < value.GetLength(2); ++z)
      for (int y = 0; y < value.GetLength(1); ++y)
        for (int x = 0; x < value.GetLength(0); ++x)
          DebugUtil.DrawSphere(ComputePoint(value, x, y, z), m_elementSize * value[z, y, x], 2, 4, m_color, true, DebugUtil.Style.FlatShaded);
  }

  private void Draw(Vector2[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
    {
      Vector3 p = ComputePoint(value, x);
      DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[x].x, value[x].y, 0.0f)), 0.05f, m_color, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector2[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
      {
        Vector3 p = ComputePoint(value, x, y);
        DebugUtil.DrawArrow(p, p + m_elementSize * (new Vector3(value[y, x].x, value[y, x].y, 0.0f)), 0.05f, m_color, true, DebugUtil.Style.FlatShaded);
      }
  }

  private void Draw(Vector3[] value)
  {
    for (int x = 0; x < value.GetLength(0); ++x)
    {
      Vector3 p = ComputePoint(value, x);
      DebugUtil.DrawArrow(p, p + m_elementSize * value[x], 0.05f, m_color, true, DebugUtil.Style.FlatShaded);
    }
  }

  private void Draw(Vector3[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
      {
        Vector3 p = ComputePoint(value, x, y);
        DebugUtil.DrawArrow(p, p + m_elementSize * value[y, x], 0.05f, m_color, true, DebugUtil.Style.FlatShaded);
      }
  }

  private void Draw(Vector3[,,] value)
  {
    for (int z = 0; z < value.GetLength(2); ++z)
      for (int y = 0; y < value.GetLength(1); ++y)
        for (int x = 0; x < value.GetLength(0); ++x)
        {
          Vector3 p = ComputePoint(value, x, y, z);
          DebugUtil.DrawArrow(p, p + m_elementSize * value[z, y, x], 0.05f, m_color, true, DebugUtil.Style.FlatShaded);
        }
  }
}
