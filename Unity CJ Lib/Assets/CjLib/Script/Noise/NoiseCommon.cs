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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace CjLib
{
  internal class NoiseCommon
  {
    // common
    //-------------------------------------------------------------------------

    private enum Io
    {
      kInput, 
      kOutput, 
    };

    private static Dictionary<int, ComputeBuffer> s_csBufferPool;
    private static ComputeBuffer GetFloatBuffer(int count, int stride, Io io)
    {
      // round up to the next highest power of 2
      // https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
      --count;
      count |= count >> 1;
      count |= count >> 2;
      count |= count >> 4;
      count |= count >> 8;
      count |= count >> 16;
      ++count;

      if (s_csBufferPool == null)
        s_csBufferPool = new Dictionary<int, ComputeBuffer>();

      int key = (io == Io.kInput ? +1 : -1) * stride;

      ComputeBuffer buffer;
      if (!s_csBufferPool.TryGetValue(key, out buffer))
      {
        buffer = new ComputeBuffer(count, stride);
        s_csBufferPool[key] = buffer;

      }
      else if (buffer.count < count)
      {
        buffer.Dispose();
        buffer = new ComputeBuffer(count, stride);
        s_csBufferPool[key] = buffer;
      }

      return buffer;
    }

    private static bool s_csIdInit = false;
    private static int s_csSeedId;
    private static int s_csDimensionId;
    private static int s_csScaleId;
    private static int s_csOffsetId;
    private static int s_csNumOctavesId;
    private static int s_csOctaveOffsetFactorId;
    private static int s_csPeriodId;
    private static int s_csInputId;
    private static int s_csInput2Id;
    private static int s_csInput3Id;
    private static int s_csOutputId;
    private static int s_csOutput2Id;
    private static int s_csOutput3Id;
    private static void InitCsId()
    {
      if (s_csIdInit)
        return;

      s_csSeedId               = Shader.PropertyToID("seed");
      s_csDimensionId          = Shader.PropertyToID("dimension");
      s_csScaleId              = Shader.PropertyToID("scale");
      s_csOffsetId             = Shader.PropertyToID("offset");
      s_csNumOctavesId         = Shader.PropertyToID("numOctaves");
      s_csOctaveOffsetFactorId = Shader.PropertyToID("octaveOffsetFactor");
      s_csPeriodId             = Shader.PropertyToID("period");
      s_csInputId              = Shader.PropertyToID("input");
      s_csInput2Id             = Shader.PropertyToID("input2");
      s_csInput3Id             = Shader.PropertyToID("input3");
      s_csOutputId             = Shader.PropertyToID("output");
      s_csOutput2Id            = Shader.PropertyToID("output2");
      s_csOutput3Id            = Shader.PropertyToID("output3");

      s_csIdInit = true;
    }

    // so zero seed doesn't easily get passed into sine and get zero
    internal static float JumbleSeed(float seed)
    {
      return (seed + 1.2345689f) * 1.23456789f;
    }

    private static int[] GetDimension(Array array)
    {
      return 
        new int[]
        {
          (array.Rank >= 1) ? array.GetLength(0) : 1,
          (array.Rank >= 2) ? array.GetLength(1) : 1,
          (array.Rank >= 3) ? array.GetLength(2) : 1
        };
    }

    private static ComputeBuffer SetInputOutputBuffers(ComputeShader shader, int kernelId, Array input, Array output)
    {
      int csInputId = -1;
      int inputStride = Marshal.SizeOf(input.GetType().GetElementType());
      switch (inputStride / sizeof(float))
      {
        case 1: csInputId = s_csInputId; break;
        case 2: csInputId = s_csInput2Id; break;
        case 3: csInputId = s_csInput3Id; break;
      }
      ComputeBuffer inputBuffer = GetFloatBuffer(input.Length, inputStride, Io.kInput);
      inputBuffer.SetData(input, 0, 0, input.Length);
      shader.SetBuffer(kernelId, csInputId, inputBuffer);

      int csOutputId = -1;
      int outputStride = Marshal.SizeOf(output.GetType().GetElementType());
      switch (outputStride / sizeof(float))
      {
        case 1: csOutputId = s_csOutputId; break;
        case 2: csOutputId = s_csOutput2Id; break;
        case 3: csOutputId = s_csOutput3Id; break;
      }
      ComputeBuffer outputBuffer = GetFloatBuffer(input.Length, outputStride, Io.kOutput);
      shader.SetBuffer(kernelId, csOutputId, outputBuffer);

      return outputBuffer;
    }

    private static ComputeBuffer SetOutputBuffer(ComputeShader shader, int kernelId, Array output, int[] dimension)
    {
      int csOutputId = -1;
      int outputStride = Marshal.SizeOf(output.GetType().GetElementType());
      switch (outputStride / sizeof(float))
      {
        case 1: csOutputId = s_csOutputId;  break;
        case 2: csOutputId = s_csOutput2Id; break;
        case 3: csOutputId = s_csOutput3Id; break;
      }
      ComputeBuffer outputBuffer = GetFloatBuffer(dimension[0] * dimension[1] * dimension[2], outputStride, Io.kOutput);
      shader.SetBuffer(kernelId, csOutputId, outputBuffer);

      return outputBuffer;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / grid samples
    //-------------------------------------------------------------------------

    // scale-agnostic noise
    internal static void Compute
    (
      INoiseDataArray output,
      ComputeShader shader,
      int kernelId, 
      float seed
    )
    {
      InitCsId();

      Array aOutput = output.GetArray();

      int[] dimension = GetDimension(aOutput);

      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, aOutput, dimension);

      seed = JumbleSeed(seed);
      shader.SetFloat(s_csSeedId, seed);
      shader.SetInts(s_csDimensionId, dimension);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(aOutput, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    // scaled noise
    internal static void Compute
    (
      INoiseDataArray output,
      ComputeShader shader, 
      int kernelId, 
      NoiseScale scale,
      NoiseOffset offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      InitCsId();

      Array aOutput = output.GetArray();

      int[] dimension = GetDimension(aOutput);

      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, aOutput, dimension);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale.GetArray());
      shader.SetFloats(s_csOffsetId, offset.GetArray());
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(aOutput, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    // scaled periodic noise
    internal static void Compute
    (
      INoiseDataArray output, 
      ComputeShader shader, 
      int kernelId, 
      NoiseScale scale, 
      NoiseOffset offset,
      NoisePeriod period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      InitCsId();

      Array aOutput = output.GetArray();

      int[] dimension = GetDimension(aOutput);

      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, aOutput, dimension);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale.GetArray());
      shader.SetFloats(s_csOffsetId, offset.GetArray());
      shader.SetFloats(s_csPeriodId, period.GetArray());
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(aOutput, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    // scaled noise
    internal static void Compute
    (
      INoiseDataArray input,
      INoiseDataArray output, 
      ComputeShader shader, 
      int kernelId, 
      NoiseScale scale, 
      NoiseOffset offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      InitCsId();

      Array aInput = input.GetArray();
      Array aOutput = output.GetArray();

      ComputeBuffer outputBuffer = SetInputOutputBuffers(shader, kernelId, aInput, aOutput);

      shader.SetFloats(s_csScaleId, scale.GetArray());
      shader.SetFloats(s_csOffsetId, offset.GetArray());
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, aInput.Length, 1, 1);

      outputBuffer.GetData(aOutput, 0, 0, aOutput.Length);
    }

    // scaled periodic noise
    internal static void Compute
    (
      INoiseDataArray input,
      INoiseDataArray output,
      ComputeShader shader, 
      int kernelId, 
      NoiseScale scale, 
      NoiseOffset offset, 
      NoisePeriod period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      InitCsId();

      Array aInput = input.GetArray();
      Array aOutput = output.GetArray();

      ComputeBuffer outputBuffer = SetInputOutputBuffers(shader, kernelId, aInput, aOutput);

      shader.SetFloats(s_csOffsetId, offset.GetArray());
      shader.SetFloats(s_csScaleId, scale.GetArray());
      shader.SetFloats(s_csPeriodId, period.GetArray());
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, aInput.Length, 1, 1);

      outputBuffer.GetData(aOutput, 0, 0, aOutput.Length);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }


  // type-merging utilities
  //---------------------------------------------------------------------------

  public class NoiseParams
  {
    private float[] m_array;
    public float[] GetArray() { return m_array; }

    public NoiseParams(float value)
    {
      m_array = new float[] { value, value, value };
    }

    public NoiseParams(float[] array, float defaultValue)
    {
      m_array =
        new float[]
        {
          (array.Length >= 1) ? array[0] : defaultValue,
          (array.Length >= 2) ? array[1] : defaultValue,
          (array.Length >= 3) ? array[2] : defaultValue
        };
    }

    public NoiseParams(Vector2 vector, float defaultValue)
    {
      m_array = new float[] { vector.x, vector.y, defaultValue };
    }

    public NoiseParams(Vector3 vector)
    {
      m_array = new float[] { vector.x, vector.y, vector.z };
    }
  }

  public class NoiseScale : NoiseParams
  {
    private static float kDefault = 1.0f;

    public static implicit operator NoiseScale(float value) { return new NoiseScale(value); }
    public NoiseScale(float value) : base(value) { }

    public static implicit operator NoiseScale(float[] array) { return new NoiseScale(array); }
    public NoiseScale(float[] array) : base(array, kDefault) { }

    public static implicit operator NoiseScale(Vector2 vector) { return new NoiseScale(vector); }
    public NoiseScale(Vector2 vector) : base(vector, kDefault) { }

    public static implicit operator NoiseScale(Vector3 vector) { return new NoiseScale(vector); }
    public NoiseScale(Vector3 vector) : base(vector) { }
  }

  public class NoiseOffset : NoiseParams
  {
    private static float kDefault = 0.0f;

    public static implicit operator NoiseOffset(float value) { return new NoiseOffset(value); }
    public NoiseOffset(float value) : base(value) { }

    public static implicit operator NoiseOffset(float[] array) { return new NoiseOffset(array); }
    public NoiseOffset(float[] array) : base(array, kDefault) { }

    public static implicit operator NoiseOffset(Vector2 vector) { return new NoiseOffset(vector); }
    public NoiseOffset(Vector2 vector) : base(vector, kDefault) { }

    public static implicit operator NoiseOffset(Vector3 vector) { return new NoiseOffset(vector); }
    public NoiseOffset(Vector3 vector) : base(vector) { }
  }

  public class NoisePeriod : NoiseParams
  {
    private static float kDefault = 1.0f;

    public static implicit operator NoisePeriod(float value) { return new NoisePeriod(value); }
    public NoisePeriod(float value) : base(value) { }

    public static implicit operator NoisePeriod(float[] array) { return new NoisePeriod(array); }
    public NoisePeriod(float[] array) : base(array, kDefault) { }

    public static implicit operator NoisePeriod(Vector2 vector) { return new NoisePeriod(vector); }
    public NoisePeriod(Vector2 vector) : base(vector, kDefault) { }

    public static implicit operator NoisePeriod(Vector3 vector) { return new NoisePeriod(vector); }
    public NoisePeriod(Vector3 vector) : base(vector) { }
  }

  internal interface INoiseDataArray
  {
    Array GetArray();
  }

  public class FloatArray : INoiseDataArray
  {
    private Array m_array;
    public Array GetArray() { return m_array; }

    public static implicit operator FloatArray(float[] array) { return new FloatArray(array); }
    public FloatArray(float[] array) { m_array = array; } 

    public static implicit operator FloatArray(float[,] array) { return new FloatArray(array); }
    public FloatArray(float[,] array) { m_array = array; }

    public static implicit operator FloatArray(float[,,] array) { return new FloatArray(array); }
    public FloatArray(float[,,] array) { m_array = array; }
  }

  public class Vector2Array : INoiseDataArray
  {
    private Array m_array;
    public Array GetArray() { return m_array; }

    public static implicit operator Vector2Array(Vector2[] array) { return new Vector2Array(array); }
    public Vector2Array(Vector2[] array) { m_array = array; }

    public static implicit operator Vector2Array(Vector2[,] array) { return new Vector2Array(array); }
    public Vector2Array(Vector2[,] array) { m_array = array; }

    public static implicit operator Vector2Array(Vector2[,,] array) { return new Vector2Array(array); }
    public Vector2Array(Vector2[,,] array) { m_array = array; }
  }

  public class Vector3Array : INoiseDataArray
  {
    private Array m_array;
    public Array GetArray() { return m_array; }

    public static implicit operator Vector3Array(Vector3[] array) { return new Vector3Array(array); }
    public Vector3Array(Vector3[] array) { m_array = array; }

    public static implicit operator Vector3Array(Vector3[,] array) { return new Vector3Array(array); }
    public Vector3Array(Vector3[,] array) { m_array = array; }

    public static implicit operator Vector3Array(Vector3[,,] array) { return new Vector3Array(array); }
    public Vector3Array(Vector3[,,] array) { m_array = array; }
  }

  //---------------------------------------------------------------------------
  // end: type-merging utilities
}
