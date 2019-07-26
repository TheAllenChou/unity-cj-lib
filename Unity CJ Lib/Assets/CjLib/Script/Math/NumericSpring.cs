/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou

  Numeric Springs
    Intro     - http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
    Examples  - http://allenchou.net/2015/04/game-math-numeric-springing-examples/
    More Info - http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
*/
/******************************************************************************/

using System.Runtime.InteropServices;

using UnityEngine;

namespace CjLib
{
  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct FloatSpring
  {
    public static readonly int Stride = 2 * sizeof(float);

    public float Value;
    public float Velocity;

    public void Reset()
    {
      Value = 0.0f;
      Velocity = 0.0f;
    }

    public void Reset(float initValue)
    {
      Value = initValue;
      Velocity = 0.0f;
    }

    public void Reset(float initValue, float initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public float TrackDampingRatio(float targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        return Value;
      }

      float delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      float detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      float detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity < MathUtil.Epsilon && delta < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
      }

      return Value;
    }

    public float TrackHalfLife(float targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = 0.6931472f / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public float TrackExponential(float targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = 0.6931472f / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector2Spring
  {
    public static readonly int Stride = 4 * sizeof(float);

    public Vector2 Value;
    public Vector2 Velocity;

    public void Reset()
    {
      Value = Vector2.zero;
      Velocity = Vector2.zero;
    }

    public void Reset(Vector2 initValue)
    {
      Value = initValue;
      Velocity = Vector2.zero;
    }

    public void Reset(Vector2 initValue, Vector2 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector2 TrackDampingRatio(Vector2 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector2.zero;
        return Value;
      }

      Vector2 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector2 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector2 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.magnitude < MathUtil.Epsilon && delta.magnitude < MathUtil.Epsilon)
      {
        Velocity = Vector2.zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector2 TrackHalfLife(Vector2 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector2.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = 0.6931472f / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector2 TrackExponential(Vector2 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector2.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = 0.6931472f / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector3Spring
  {
    public static readonly int Stride = 8 * sizeof(float);

    public Vector3 Value;
    private float m_padding0;
    public Vector3 Velocity;
    private float m_padding1;

    public void Reset()
    {
      Value = Vector3.zero;
      Velocity = Vector3.zero;
    }

    public void Reset(Vector3 initValue)
    {
      Value = initValue;
      Velocity = Vector3.zero;
    }

    public void Reset(Vector3 initValue, Vector3 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector3 TrackDampingRatio(Vector3 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector3.zero;
        return Value;
      }

      Vector3 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector3 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector3 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.magnitude < MathUtil.Epsilon && delta.magnitude < MathUtil.Epsilon)
      {
        Velocity = Vector3.zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector3 TrackHalfLife(Vector3 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector3.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = 0.6931472f / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector3 TrackExponential(Vector3 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector3.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = 0.6931472f / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector4Spring
  {
    public static readonly int Stride = 8 * sizeof(float);

    public Vector4 Value;
    public Vector4 Velocity;

    public void Reset()
    {
      Value = Vector4.zero;
      Velocity = Vector4.zero;
    }

    public void Reset(Vector4 initValue)
    {
      Value = initValue;
      Velocity = Vector4.zero;
    }

    public void Reset(Vector4 initValue, Vector4 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector4 TrackDampingRatio(Vector4 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector4.zero;
        return Value;
      }

      Vector4 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector4 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector4 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.magnitude < MathUtil.Epsilon && delta.magnitude < MathUtil.Epsilon)
      {
        Velocity = Vector4.zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector4 TrackHalfLife(Vector4 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector4.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = 0.6931472f / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector4 TrackExponential(Vector4 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector4.zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = 0.6931472f / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct QuaternionSpring
  {
    public static readonly int Stride = 8 * sizeof(float);

    public Vector4 ValueVec;
    public Vector4 VelocityVec;

    public Quaternion ValueQuat
    {
      get { return QuaternionUtil.FromVector4(ValueVec); }
      set { ValueVec = QuaternionUtil.ToVector4(value); }
    }

    public Quaternion VelocityQuat
    {
      get { return QuaternionUtil.FromVector4(VelocityVec, false); }
      set { VelocityVec = QuaternionUtil.ToVector4(value); }
    }

    public void Reset()
    {
      ValueVec = QuaternionUtil.ToVector4(Quaternion.identity);
      VelocityVec = Vector4.zero;
    }

    public void Reset(Vector4 initValue)
    {
      ValueVec = initValue;
      VelocityVec = Vector4.zero;
    }

    public void Reset(Vector4 initValue, Vector4 initVelocity)
    {
      ValueVec = initValue;
      VelocityVec = initVelocity;
    }

    public void Reset(Quaternion initValue)
    {
      ValueVec = QuaternionUtil.ToVector4(initValue);
      VelocityVec = Vector4.zero;
    }

    public void Reset(Quaternion initValue, Quaternion initVelocity)
    {
      ValueVec = QuaternionUtil.ToVector4(initValue);
      VelocityVec = QuaternionUtil.ToVector4(initVelocity);
    }

    public Quaternion TrackDampingRatio(Quaternion targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
        return QuaternionUtil.FromVector4(ValueVec);
      }

      Vector4 targetValueVec = QuaternionUtil.ToVector4(targetValue);

      // keep in same hemisphere for shorter track delta
      if (Vector4.Dot(ValueVec, targetValueVec) < 0.0f)
      {
        targetValueVec = -targetValueVec;
      }

      Vector4 delta = targetValueVec - ValueVec;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector4 detX = f * ValueVec + deltaTime * VelocityVec + hhoo * targetValueVec;
      Vector4 detV = VelocityVec + hoo * delta;

      VelocityVec = detV * detInv;
      ValueVec = detX * detInv;

      if (VelocityVec.magnitude < MathUtil.Epsilon && delta.magnitude < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);;
        ValueVec = targetValueVec;
      }

      return QuaternionUtil.FromVector4(ValueVec);
    }

    public Quaternion TrackHalfLife(Quaternion targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
        ValueVec = QuaternionUtil.ToVector4(targetValue);
        return targetValue;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = 0.6931472f / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Quaternion TrackExponential(Quaternion targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.identity);
        ValueVec = QuaternionUtil.ToVector4(targetValue);
        return targetValue;
      }

      float angularFrequency = 0.6931472f / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }
}
