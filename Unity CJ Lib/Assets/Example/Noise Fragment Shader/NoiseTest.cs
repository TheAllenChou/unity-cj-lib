/******************************************************************************/
/*
  Project   - Starblaze
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using UnityEngine;

using CjLib;

public class NoiseTest : MonoBehaviour
{
  public enum NoiseType
  {
    Classic,
    Periodic,
    Simplex,
    SimplexGradient,
    Random, 
    RandomVector, 
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
    else if (m_noiseType == NoiseType.Periodic)
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

    /*
    float[] output0 = new float[1000];
    float[,] output1 = new float[1000, 2];
    float[,,] output2 = new float[1000, 2, 2];
    RandomNoise.Compute(output0);
    RandomNoise.Compute(output1);
    RandomNoise.Compute(output2);
    output0[0] = output0[0];
    */
  }
}
