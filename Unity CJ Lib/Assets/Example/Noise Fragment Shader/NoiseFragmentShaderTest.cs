/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

  This example is based on Noise Shader Library for Unity
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

public class NoiseFragmentShaderTest : MonoBehaviour
{
  public enum NoiseType
  {
    Classic,
    ClassicPeriodic,
    Random, 
    RandomVector, 
    Simplex,
    SimplexGradient,
  }

  public NoiseType m_noiseType = NoiseType.Simplex;
  public bool m_is3D;
  public int m_numOctaves = 6;
  public float m_octaveOffsetFactor = 1.2f;

  Material m_material;

  void Update()
  {
    if (m_material == null)
    {
      Shader shader = Shader.Find("CjLib/Example/NoiseTest");
      m_material = new Material(shader);
      m_material.hideFlags = HideFlags.DontSave;
      GetComponent<Renderer>().material = m_material;
    }

    m_material.shaderKeywords = null;

    if (m_noiseType == NoiseType.Classic)
      m_material.EnableKeyword("CNOISE");
    else if (m_noiseType == NoiseType.ClassicPeriodic)
      m_material.EnableKeyword("PNOISE");
    else if (m_noiseType == NoiseType.Simplex)
      m_material.EnableKeyword("SNOISE");
    else if (m_noiseType == NoiseType.SimplexGradient)
      m_material.EnableKeyword("SNOISE_GRAD");
    else if (m_noiseType == NoiseType.Random)
      m_material.EnableKeyword("RAND");
    else // RandomVector
      m_material.EnableKeyword("RAND_VEC");

    if (m_is3D)
      m_material.EnableKeyword("THREED");

    m_material.SetVector("_Offset", new Vector4(0.0f, -0.5f * Time.time, 0.0f, 0.0f));
    m_material.SetInt("_NumOctaves", m_numOctaves);
    m_material.SetFloat("_OctaveOffsetFactor", m_octaveOffsetFactor);
  }
}
