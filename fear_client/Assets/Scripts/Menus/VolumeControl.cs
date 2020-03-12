using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    public enum channel {
        MasterVolume,
        BGMVolume,
        SFXVolume
    };

    public channel dropdown = channel.MasterVolume;

    //public var dropdown = (int) channel.MasterVolume;

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat(dropdown.ToString(), Mathf.Log10(sliderValue) * 20);
    }
}
