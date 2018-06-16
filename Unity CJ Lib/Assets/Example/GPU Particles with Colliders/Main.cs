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

namespace GpuParticlesWithColliders
{
  public class Main : MonoBehaviour
  {
    public ComputeShader m_shader;
    public Material m_material;

    private ComputeShader m_prevShader;
    private Material m_prevMaterial;

    [Range(0.0f, 2.0f)]
    public float m_timeScale = 1.0f;

    [Range(5.0f, 20.0f)]
    public float m_gravity = 9.8f;

    [Range(0.2f, 1.0f)]
    public float m_sphereRadius = 0.8f;

    [Range(-0.1f, 0.1f)]
    public float m_sphereSpeed = 0.05f;

    [Range(-1.0f, 1.0f)]
    public float m_floorTilt = 0.0f;

    [Range(-5.0f, 0.0f)]
    public float m_floorHeight = -2.0f;

    [Range(0.0f, 1.0f)]
    public float m_restitution = 0.3f;

    [Range(0.0f, 1.0f)]
    public float m_friction = 0.7f;

    public GameObject m_spherePrefab;
    public GameObject m_floorPrefab;

    private float m_spherePhase;
    private GameObject[] m_aSphere;
    private Vector3[] m_aSpherePrevPos;
    private Vector4[] m_aSphereVel;
    private const int kNumSpheres = 3;
    private GameObject m_floor;

    private const int kNumParticles = 1000;

    private ComputeBuffer m_computeBuffer;
    private ComputeBuffer m_instanceArgsBuffer;

    private Mesh m_mesh;
    private MaterialPropertyBlock m_materialProperties;

    private int m_csInitKernelId;
    private int m_csStepKernelId;

    private int m_csParticleBufferId;
    private int m_csScaleId;
    private int m_csSpeedId;
    private int m_csLifetimeId;
    private int m_csNumParticlesId;
    private int m_csTimeId;
    private int m_csDynamics;
    private int m_csASphere;
    private int m_csASphereVel;
    private int m_csPlane;

    void OnEnable()
    {
      m_mesh = new Mesh();
      m_mesh = PrimitiveMeshFactory.BoxFlatShaded();

      m_spherePhase = 0.0f;
      m_aSphere = new GameObject[kNumSpheres];
      m_aSpherePrevPos = new Vector3[kNumSpheres];
      m_aSphereVel = new Vector4[kNumSpheres];
      for (int i = 0; i < kNumSpheres; ++i)
      {
        m_aSphere[i] = Instantiate(m_spherePrefab);
        m_aSpherePrevPos[i] = Vector3.zero;
      }

      m_floor = Instantiate(m_floorPrefab);

      int particleStride = sizeof(float) * 24;
      m_computeBuffer = new ComputeBuffer(kNumParticles, particleStride);

      uint[] instanceArgs = new uint[] { 0, 0, 0, 0, 0 };
      m_instanceArgsBuffer = new ComputeBuffer(1, instanceArgs.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      instanceArgs[0] = (uint)m_mesh.GetIndexCount(0);
      instanceArgs[1] = (uint)kNumParticles;
      instanceArgs[2] = (uint)m_mesh.GetIndexStart(0);
      instanceArgs[3] = (uint)m_mesh.GetBaseVertex(0);
      m_instanceArgsBuffer.SetData(instanceArgs);

      m_csInitKernelId = m_shader.FindKernel("Init");
      m_csStepKernelId = m_shader.FindKernel("Step");

      m_csParticleBufferId = Shader.PropertyToID("particleBuffer");
      m_csScaleId = Shader.PropertyToID("scale");
      m_csSpeedId = Shader.PropertyToID("speed");
      m_csLifetimeId = Shader.PropertyToID("lifetime");
      m_csNumParticlesId = Shader.PropertyToID("numParticles");
      m_csTimeId = Shader.PropertyToID("time");
      m_csDynamics = Shader.PropertyToID("dynamics");
      m_csASphere = Shader.PropertyToID("aSphere");
      m_csASphereVel = Shader.PropertyToID("aSphereVel");
      m_csPlane = Shader.PropertyToID("plane");

      m_materialProperties = new MaterialPropertyBlock();

      InitParticles();
    }

    void Update()
    {
      if (!isActiveAndEnabled)
        return;
      
      UpdateParticles();
      RenderParticles();
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

    private void SetUpMaterial()
    {
      m_material.enableInstancing = true;
      m_material.SetBuffer(m_csParticleBufferId, m_computeBuffer);
    }

    private void SetUpShader()
    {
      m_shader.SetFloats(m_csScaleId, new float[] { 0.1f, 0.15f });
      m_shader.SetFloats(m_csSpeedId, new float[] { 1.0f, 1.5f, 1.0f, 6.0f });
      m_shader.SetFloats(m_csLifetimeId, new float[] { 0.1f, 3.0f, 3.0f, 0.1f });
      m_shader.SetInt(m_csNumParticlesId, kNumParticles);

      m_shader.SetBuffer(m_csInitKernelId, m_csParticleBufferId, m_computeBuffer);
      m_shader.SetBuffer(m_csStepKernelId, m_csParticleBufferId, m_computeBuffer);
    }

    private void InitParticles()
    {
      SetUpMaterial();
      SetUpShader();

      m_shader.Dispatch(m_csInitKernelId, kNumParticles, 1, 1);
    }

    private void UpdateParticles()
    {
      SetUpMaterial();
      SetUpShader();

      m_shader.SetFloats(m_csTimeId, new float[] { Time.time, m_timeScale * Time.fixedDeltaTime });
      m_shader.SetFloats(m_csDynamics, new float[] { m_gravity, m_restitution, m_friction });

      Vector4[] aSphere = new Vector4[kNumSpheres];
      for (int i = 0; i < kNumSpheres; ++i)
      {
        m_aSpherePrevPos[i] = m_aSphere[i].transform.position;
        m_spherePhase -= MathUtil.TwoPi * (Time.fixedDeltaTime * m_sphereSpeed) * m_timeScale;
        float phase = m_spherePhase + MathUtil.TwoPi * (((float)i) / kNumSpheres);
        Vector3 pos = new Vector3(1.5f * Mathf.Cos(phase), 1.5f * Mathf.Sin(phase) - 0.5f, 0.0f);
        m_aSphere[i].transform.position = pos;
        m_aSphere[i].transform.localScale = 2.0f * new Vector3(m_sphereRadius, m_sphereRadius, m_sphereRadius);
        aSphere[i].Set(pos.x, pos.y, pos.z, m_sphereRadius);
        m_aSphereVel[i] = (pos - m_aSpherePrevPos[i]) / Time.fixedDeltaTime;
      }
      m_shader.SetVectorArray(m_csASphere, aSphere);
      m_shader.SetVectorArray(m_csASphereVel, m_aSphereVel);

      Quaternion floorRot = Quaternion.AngleAxis(20.0f * m_floorTilt, Vector3.left);
      m_floor.transform.position = new Vector3(0.0f, m_floorHeight, 0.0f);
      m_floor.transform.rotation = floorRot;
      Vector3 floorNormal = floorRot * Vector3.up;
      float floorD = -floorNormal.y * m_floorHeight;
      m_shader.SetVector(m_csPlane, new Vector4(floorNormal.x, floorNormal.y, floorNormal.z, floorD));

      m_shader.Dispatch(m_csStepKernelId, kNumParticles, 1, 1);
    }

    private void RenderParticles()
    {
      Graphics.DrawMeshInstancedIndirect(m_mesh, 0, m_material, new Bounds(Vector3.zero, 20.0f * Vector3.one), m_instanceArgsBuffer, 0, m_materialProperties, UnityEngine.Rendering.ShadowCastingMode.On);
    }
  }
}
