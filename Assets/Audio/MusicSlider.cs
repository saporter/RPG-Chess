using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour {

    public AudioMixer mixer;
    public Slider slider;

    public void MusicVolumeAdjust()
    {
        mixer.SetFloat("musicVol", slider.value);
    }

    public void SFXVolumeAdjust()
    {
        mixer.SetFloat("SFXVol", slider.value);
    }
}
