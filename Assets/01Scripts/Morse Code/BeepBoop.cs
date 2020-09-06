using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeepBoop
{
    public static AudioClip GetTone(int sampleCount, int sampleFreq, float frequency)
    {
        float[] samples = new float[sampleCount];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(Mathf.PI*2*i*frequency/sampleFreq);
        }
        AudioClip ac = AudioClip.Create("BeepBoop", samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);
        return ac;
    }
}
