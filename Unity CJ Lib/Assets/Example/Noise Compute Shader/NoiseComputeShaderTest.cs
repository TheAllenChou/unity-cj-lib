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

using UnityEngine;

using CjLib;

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
    RandomVector2Dv1D, 
    RandomVector2Dv2D, 
    RandomVector2Dv3D, 
    RandomVector3Dv1D, 
    RandomVector3Dv2D, 
    RandomVector3Dv3D, 
    Simplex1D, 
    Simplex2D, 
    Simplex3D, 
    SimplexGradient2Dv1D, 
    SimplexGradient2Dv2D, 
    SimplexGradient2Dv3D, 
    SimplexGradient3Dv1D, 
    SimplexGradient3Dv2D, 
    SimplexGradient3Dv3D, 
  }

  public NoiseType m_noiseType = NoiseType.Simplex3D;
  public int m_numOctaves = 2;
  public float m_octaveOffsetFactor = 1.2f;
  public Color m_color = Color.white;

  private float[] m_drawDimension = new float[] { 2.0f, 2.0f, 2.0f };

  void Update()
  {
    float[] offset = new float[] { 0.5f * Time.time, 0.25f * Time.time, 0.25f * Time.time };
    float[,] output2 = new float[10, 10];
    float[,,] output3 = new float[10, 10, 10];
    Vector2[,] output2v2 = new Vector2[10, 10];
    Vector3[,] output2v3 = new Vector3[10, 10];
    Vector3[,,] output3v3 = new Vector3[10, 10, 10];
    float[] scale = new float[] { 0.02f, 0.02f, 0.02f };
    float[] period = new float[] { 0.15f, 0.15f, 0.15f };

    switch (m_noiseType)
    {
      case NoiseType.Classic1D:
        break;
      case NoiseType.Classic2D:
        break;
      case NoiseType.Classic3D:
        break;
      case NoiseType.ClassicPeriodic1D:
        break;
      case NoiseType.ClassicPeriodic2D:
        break;
      case NoiseType.ClassicPeriodic3D:
        break;
      case NoiseType.Random1D:
        break;
      case NoiseType.Random2D:
        break;
      case NoiseType.Random3D:
        break;
      case NoiseType.RandomVector2Dv1D:
        break;
      case NoiseType.RandomVector2Dv2D:
        break;
      case NoiseType.RandomVector2Dv3D:
        break;
      case NoiseType.RandomVector3Dv1D:
        break;
      case NoiseType.RandomVector3Dv2D:
        break;
      case NoiseType.RandomVector3Dv3D:
        break;
      case NoiseType.Simplex1D:
        break;
      case NoiseType.Simplex2D:
        break;
      case NoiseType.Simplex3D:
        break;
      case NoiseType.SimplexGradient2Dv1D:
        break;
      case NoiseType.SimplexGradient2Dv2D:
        break;
      case NoiseType.SimplexGradient2Dv3D:
        break;
      case NoiseType.SimplexGradient3Dv1D:
        break;
      case NoiseType.SimplexGradient3Dv2D:
        break;
      case NoiseType.SimplexGradient3Dv3D:
        break;
    }
  }

  Vector3 ComputePoint(float[,] value, int x, int y)
  {
    return 
      new Vector3
      (
        ((((float) x) / value.GetLength(0)) - 0.5f) * m_drawDimension[0], 
        ((((float) y) / value.GetLength(1)) - 0.5f) * m_drawDimension[1], 
        0.0f
      );
  }

  Vector3 ComputePoint(float[,,] value, int x, int y, int z)
  {
    return
      new Vector3
      (
        ((((float)x) / value.GetLength(0)) - 0.5f) * m_drawDimension[0], 
        ((((float)y) / value.GetLength(1)) - 0.5f) * m_drawDimension[1], 
        ((((float)z) / value.GetLength(2)) - 0.5f) * m_drawDimension[2]
      );
  }

  private void Draw(float[,] value)
  {
    for (int y = 0; y < value.GetLength(1); ++y)
      for (int x = 0; x < value.GetLength(0); ++x)
        DebugUtil.DrawSphere(ComputePoint(value, x, y), 0.2f * value[x, y], 2, 4, m_color, true, DebugUtil.Style.FlatShaded);
  }

  private void Draw(float[,,] value)
  {
    for (int z = 0; z < value.GetLength(2); ++z)
      for (int y = 0; y < value.GetLength(1); ++y)
        for (int x = 0; x < value.GetLength(0); ++x)
          DebugUtil.DrawSphere(ComputePoint(value, x, y, z), 0.2f * value[x, y, z], 2, 4, m_color, true, DebugUtil.Style.FlatShaded);
  }

  private void Draw(Vector3[,] value)
  {

  }

  private void Draw(Vector3[,,] value)
  {
    
  }
}
