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

using CjLib;

namespace TurbulentRainbowGpuParticles
{
  public class Main : MonoBehaviour
  {
    public ComputeShader m_shader;

    private const int kNumParticles = 10000;

    /*
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
    */

    private ComputeBuffer m_computeBuffer;
    private ComputeBuffer m_instanceArgsBuffer;
    //private Particle[] m_debugBuffer;

    private Mesh m_mesh;
    private Material m_material;
    private MaterialPropertyBlock m_materialProperties;

    private int m_csInitKernelId;
    private int m_csStepKernelId;

    private int m_csParticleBufferId;
    private int m_csScaleId;
    private int m_csDampingId;
    private int m_csSpeedId;
    private int m_csLifetimeId;
    private int m_csNumParticlesId;
    private int m_csTimeId;

    void OnEnable()
    {
      m_mesh = new Mesh();
      m_mesh = PrimitiveMeshFactory.BoxFlatShaded();

      int particleStride = sizeof(float) * 24;
      m_computeBuffer = new ComputeBuffer(kNumParticles, particleStride);

      uint[] instanceArgs = new uint[] { 0, 0, 0, 0, 0 };
      m_instanceArgsBuffer = new ComputeBuffer(1, instanceArgs.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      instanceArgs[0] = (uint) m_mesh.GetIndexCount(0);
      instanceArgs[1] = (uint) kNumParticles;
      instanceArgs[2] = (uint) m_mesh.GetIndexStart(0);
      instanceArgs[3] = (uint) m_mesh.GetBaseVertex(0);
      m_instanceArgsBuffer.SetData(instanceArgs);

      //m_debugBuffer = new Particle[kNumParticles];

      m_csInitKernelId = m_shader.FindKernel("Init");
      m_csStepKernelId = m_shader.FindKernel("Step");

      m_csParticleBufferId = Shader.PropertyToID("particleBuffer");
      m_csScaleId = Shader.PropertyToID("scale");
      m_csDampingId = Shader.PropertyToID("damping");
      m_csSpeedId = Shader.PropertyToID("speed");
      m_csLifetimeId = Shader.PropertyToID("lifetime");
      m_csNumParticlesId = Shader.PropertyToID("numParticles");
      m_csTimeId = Shader.PropertyToID("time");

      m_material = new Material(Shader.Find("CjLib/Example/TurbulentRainbowGpuParticles"));
      m_material.enableInstancing = true;
      m_material.SetBuffer(m_csParticleBufferId, m_computeBuffer);
      m_materialProperties = new MaterialPropertyBlock();

      m_shader.SetFloats(m_csScaleId, new float[] { 0.15f, 0.3f });
      m_shader.SetFloat(m_csDampingId, 6.0f);
      m_shader.SetFloats(m_csSpeedId, new float[] { 3.0f, 4.0f, 1.0f, 6.0f });
      m_shader.SetFloats(m_csLifetimeId, new float[] { 0.1f, 0.5f, 0.5f, 0.1f });
      m_shader.SetInt(m_csNumParticlesId, kNumParticles);

      m_shader.SetBuffer(m_csInitKernelId, m_csParticleBufferId, m_computeBuffer);
      m_shader.SetBuffer(m_csStepKernelId, m_csParticleBufferId, m_computeBuffer);

      m_shader.Dispatch(m_csInitKernelId, kNumParticles, 1, 1);

      //m_computeBuffer.GetData(m_debugBuffer);
    }

    void Update()
    {
      m_shader.SetFloats(m_csTimeId, new float[] { Time.time, Time.fixedDeltaTime });
      m_shader.Dispatch(m_csStepKernelId, kNumParticles, 1, 1);

      //m_computeBuffer.GetData(m_debugBuffer);

      Graphics.DrawMeshInstancedIndirect(m_mesh, 0, m_material, new Bounds(Vector3.zero, 20.0f * Vector3.one), m_instanceArgsBuffer, 0, m_materialProperties, UnityEngine.Rendering.ShadowCastingMode.On);
    }

    void OnDisable()
    {
      if (m_computeBuffer != null)
      {
        m_computeBuffer.Dispose();
        m_computeBuffer = null;
      }

      if (m_instanceArgsBuffer != null)
      {
        m_instanceArgsBuffer.Dispose();
        m_instanceArgsBuffer = null;
      }
    }
  }
}
