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

using UnityEngine;

namespace CjLib
{
  internal class NoiseCommon
  {
    // common
    //-------------------------------------------------------------------------

    internal static Dictionary<int, ComputeBuffer> s_csBufferPool;
    internal static ComputeBuffer GetFloatBuffer(int count, int stride)
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

      ComputeBuffer buffer;
      if (!s_csBufferPool.TryGetValue(stride, out buffer))
      {
        buffer = new ComputeBuffer(count, stride);
        s_csBufferPool[stride] = buffer;

      }
      else if (buffer.count < count)
      {
        buffer.Dispose();
        buffer = new ComputeBuffer(count, stride);
        s_csBufferPool[stride] = buffer;
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
    internal static int s_csBufferId;
    internal static int s_csBuffer2Id;
    internal static int s_csBuffer3Id;
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
      s_csBufferId             = Shader.PropertyToID("buffer");
      s_csBuffer2Id            = Shader.PropertyToID("buffer2");
      s_csBuffer3Id            = Shader.PropertyToID("buffer3");

      s_csIdInit = true;
    }

    // so zero seed doesn't easily get passed into sine and get zero
    internal static float JumbleSeed(float seed)
    {
      return (seed + 1.2345689f) * 1.23456789f;
    }

    //-------------------------------------------------------------------------
    // end: common


    // GPU compute / custom sample points
    //-------------------------------------------------------------------------

    // scale-agnostic noise
    internal static void Compute
    (
      Array input, 
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      float seed, 
      int bufferStride
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(input.Length, bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      seed = JumbleSeed(seed);
      shader.SetFloat(s_csSeedId, seed);

      shader.Dispatch(kernelId, input.Length, 1, 1);

      buffer.GetData(output);
    }

    // scaled noise
    internal static void Compute
    (
      Array input, 
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int bufferStride, 
      float[] offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(input.Length, bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, input.Length, 1, 1);

      buffer.GetData(output);
    }

    // scaled periodic noise
    internal static void Compute
    (
      Array input, 
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int bufferStride, 
      float[] offset, 
      float[] period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(input.Length, bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);
      shader.SetFloats(s_csPeriodId, period);

      shader.Dispatch(kernelId, input.Length, 1, 1);

      buffer.GetData(output);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / custom sampel points


    // GPU compute / grid sample points
    //-------------------------------------------------------------------------

    // scale-agnostic noise
    internal static void Compute
    (
      Array output,
      ComputeShader shader,
      int kernelId,
      float seed,
      int[] dimension,
      int bufferStride
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(dimension[0] * dimension[1] * dimension[2], bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      seed = JumbleSeed(seed);
      shader.SetFloat(s_csSeedId, seed);
      shader.SetInts(s_csDimensionId, dimension);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      buffer.GetData(output);
    }

    // scaled noise
    internal static void Compute
    (
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int[] dimension, 
      int bufferStride, 
      float[] scale, 
      float[] offset, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(dimension[0] * dimension[1] * dimension[2], bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      buffer.GetData(output);
    }

    // scaled periodic noise
    internal static void Compute
    (
      Array output, 
      ComputeShader shader, 
      int kernelId, 
      int[] dimension, 
      int bufferStride, 
      float[] scale, 
      float[] offset, 
      float[] period, 
      int numOctaves, 
      float octaveOffsetFactor
    )
    {
      int csBufferId = s_csBufferId;
      switch (bufferStride / sizeof(float))
      {
        case 2: csBufferId = s_csBuffer2Id; break;
        case 3: csBufferId = s_csBuffer3Id; break;
      }

      ComputeBuffer buffer = GetFloatBuffer(dimension[0] * dimension[1] * dimension[2], bufferStride);
      shader.SetBuffer(kernelId, csBufferId, buffer);

      shader.SetInts(s_csDimensionId, dimension);
      shader.SetFloats(s_csScaleId, scale);
      shader.SetFloats(s_csOffsetId, offset);
      shader.SetInt(s_csNumOctavesId, numOctaves);
      shader.SetFloat(s_csOctaveOffsetFactorId, octaveOffsetFactor);
      shader.SetFloats(s_csPeriodId, period);

      shader.Dispatch(kernelId, dimension[0], dimension[1], dimension[2]);

      buffer.GetData(output);
    }

    //-------------------------------------------------------------------------
    // end: GPU compute / grid sample points
  }
}
