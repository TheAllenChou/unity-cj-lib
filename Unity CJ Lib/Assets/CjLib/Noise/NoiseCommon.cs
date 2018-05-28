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

    internal enum Io
    {
      kInput, 
      kOutput, 
    };

    internal static Dictionary<int, ComputeBuffer> s_csBufferPool;
    internal static ComputeBuffer GetFloatBuffer(int count, int stride, Io io)
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

    internal static bool s_csIdInit = false;
    internal static int s_csSeedId;
    internal static int s_csDimensionId;
    internal static int s_csScaleId;
    internal static int s_csOffsetId;
    internal static int s_csNumOctavesId;
    internal static int s_csOctaveOffsetFactorId;
    internal static int s_csPeriodId;
    internal static int s_csInputId;
    internal static int s_csInput2Id;
    internal static int s_csInput3Id;
    internal static int s_csOutputId;
    internal static int s_csOutput2Id;
    internal static int s_csOutput3Id;
    internal static void InitCsId()
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

    internal static ComputeBuffer SetInputOutputBuffers(ComputeShader shader, int kernelId, Array input, Array output)
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

    internal static ComputeBuffer SetOutputBuffer(ComputeShader shader, int kernelId, Array output, int[] dimension)
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
      Array output,
      ComputeShader shader,
      int kernelId,
      float seed,
      int[] dimension
    )
    {
      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, output, dimension);

      seed = JumbleSeed(seed);
      shader.SetFloat(s_csSeedId, seed);
      shader.SetInts(s_csDimensionId, dimension);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(output, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    // scaled noise
    internal static void Compute
    (
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int[] dimension, 
      float[] scale, 
      float[] offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, output, dimension);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(output, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    // scaled periodic noise
    internal static void Compute
    (
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int[] dimension, 
      float[] scale, 
      float[] offset, 
      float[] period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      ComputeBuffer outputBuffer = SetOutputBuffer(shader, kernelId, output, dimension);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csOffsetId, offset);
      shader.SetFloats(s_csPeriodId, period);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      outputBuffer.GetData(output, 0, 0, dimension[0] * dimension[1] * dimension[2]);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid samples


    // GPU compute / custom samples
    //-------------------------------------------------------------------------

    // scaled noise
    internal static void Compute
    (
      Array input, 
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      float[] scale, 
      float[] offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      ComputeBuffer outputBuffer = SetInputOutputBuffers(shader, kernelId, input, output);

      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, input.Length, 1, 1);

      outputBuffer.GetData(output, 0, 0, output.Length);
    }

    // scaled periodic noise
    internal static void Compute
    (
      Array input, 
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      float[] scale, 
      float[] offset, 
      float[] period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      ComputeBuffer outputBuffer = SetInputOutputBuffers(shader, kernelId, input, output);

      shader.SetFloats(s_csOffsetId, offset);
      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csPeriodId, period);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, input.Length, 1, 1);

      outputBuffer.GetData(output, 0, 0, output.Length);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom samples
  }
}
