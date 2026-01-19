using System;
using UnityEngine;

public class MathUtils
{
    public static float GetT(float a, float b, float value)
    {
        return Mathf.Clamp01((value - a) / (b - a));
    }

    public static float GetTWithOffset(float t, float tOffset)
    {
        return t * (1 - tOffset) + tOffset;
    }

    public static float Remap(float value, float inMin, float inMax, float outMin, float outMax, bool clamp = false)
    {
        if (inMin == inMax)
            return outMin;

        float t = GetT(inMin, inMax, value);
        if (clamp)
            t = MathF.Max(0f, MathF.Min(1f, t));
        return t * (outMax - outMin) + outMin;
    }
}
