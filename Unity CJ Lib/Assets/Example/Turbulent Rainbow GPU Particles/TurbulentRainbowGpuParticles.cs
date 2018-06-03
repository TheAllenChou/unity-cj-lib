/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/


using System.Runtime.InteropServices;

using UnityEngine;

using CjLib;

public class TurbulentRainbowGpuParticles : MonoBehaviour
{
  public ComputeShader m_shader;
  public Material m_material;
  public MaterialPropertyBlock m_materialProperties;

  private const int kNumParticles = 10000;

  private struct Particle
  {
    // 4 floats
    Vector3 m_position;
    float m_damping;

    // 4 floats
    Quaternion m_rotation;

    // 4 floats
    Vector3 m_linearVelocity;
    float m_scale;

    // 4 floats
    Quaternion m_angularVelocity;

    // 4 floats
    Vector4 m_lifetime;

    // 4 floats
    Color m_color;
  };

  private ComputeBuffer m_computeBuffer;
  private Particle[] m_debugBuffer;
  private Matrix4x4[] m_aMatrix;

  private Mesh m_mesh;

  private int m_csInitKernelId;
  private int m_csStepKernelId;

  private int m_csParticleBufferId;
  private int m_csScaleId;
  private int m_csDampingId;
  private int m_csSpeedId;
  private int m_csLifetimeId;
  private int m_csNumParticlesId;
  private int m_csTimeId;

  void Start ()
  {
    int particleStride = sizeof(float) * 24;
    m_computeBuffer = new ComputeBuffer(kNumParticles, particleStride);

    m_mesh = new Mesh();
    m_mesh.vertices = new Vector3[kNumParticles];
    {
      int[] aIndex = new int[kNumParticles];
      for (int i = 0; i < kNumParticles; ++i)
        aIndex[i] = i;
      m_mesh.SetIndices(aIndex, MeshTopology.Points, 0);
    }
      
    

    m_debugBuffer = new Particle[kNumParticles];
    m_aMatrix = new Matrix4x4[kNumParticles];
    for (int i = 0; i < kNumParticles; ++i)
      m_aMatrix[i] = Matrix4x4.identity;

    m_csInitKernelId = m_shader.FindKernel("Init");
    m_csStepKernelId = m_shader.FindKernel("Step");

    m_csParticleBufferId = Shader.PropertyToID("particleBuffer");
    m_csScaleId          = Shader.PropertyToID("scale");
    m_csDampingId        = Shader.PropertyToID("damping");
    m_csSpeedId          = Shader.PropertyToID("speed");
    m_csLifetimeId       = Shader.PropertyToID("lifetime");
    m_csNumParticlesId   = Shader.PropertyToID("numParticles");
    m_csTimeId           = Shader.PropertyToID("time");

    m_materialProperties = new MaterialPropertyBlock();
    m_materialProperties.SetBuffer(m_csParticleBufferId, m_computeBuffer);

    m_shader.SetFloats(m_csScaleId, new float[] { 0.15f, 0.3f });
    m_shader.SetFloat(m_csDampingId, 6.0f);
    m_shader.SetFloats(m_csSpeedId, new float[] { 3.0f, 4.0f, 1.0f, 6.0f });
    m_shader.SetFloats(m_csLifetimeId, new float[] { 0.1f, 0.5f, 0.5f, 0.1f });
    m_shader.SetInt(m_csNumParticlesId, kNumParticles);

    m_shader.SetBuffer(m_csInitKernelId, m_csParticleBufferId, m_computeBuffer);
    m_shader.SetBuffer(m_csStepKernelId, m_csParticleBufferId, m_computeBuffer);

    m_shader.Dispatch(m_csInitKernelId, kNumParticles, 1, 1);

    m_computeBuffer.GetData(m_debugBuffer);
  }
  
  void Update()
  {
    m_shader.SetFloats(m_csTimeId, new float[] { Time.time, 0.016f });
    m_shader.Dispatch(m_csStepKernelId, kNumParticles, 1, 1);

    m_computeBuffer.GetData(m_debugBuffer);

    Graphics.DrawMesh(m_mesh, Matrix4x4.identity, m_material, 0, null, 0, m_materialProperties);
  }
}
