using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeHandler : MonoBehaviour
{
    public AudioMixer master;
    public void SetMaster(float sliderValue)
    {
        master.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
    public void SetAmbient(float sliderValue)
    {
        master.SetFloat("Ambient", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusic(float sliderValue)
    {
        master.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFX(float sliderValue)
    {
        master.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
}
