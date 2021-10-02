using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsWindow : MonoBehaviour
{
    public event EventHandler OnVolumeChaned;
    private float musicVolume;
    private float soundEffectsVolume;
    private Slider musicSlider;
    private Slider soundEffectsSlider;
    [SerializeField] private AudioMixer musicAudioMixer;
    [SerializeField] private AudioMixer soundEffectsAudioMixer;

    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }
    public void Setup(float musicVolume, float soundEffectsVolume)
    {
        musicSlider = transform.Find("MusicSlider").GetComponent<Slider>();
        soundEffectsSlider = transform.Find("SoundEffectsSlider").GetComponent<Slider>();

        musicSlider.value = musicVolume;
        soundEffectsSlider.value = soundEffectsVolume;

        SetMusicVolume(musicVolume);
        SetSoundEffectsVolume(soundEffectsVolume);
    }

    public void SetMusicVolume(float volume)
    {
        this.musicVolume = volume;
        musicAudioMixer.SetFloat("volume", volume);
        OnVolumeChaned?.Invoke(this, EventArgs.Empty);
    }
    public void SetSoundEffectsVolume(float volume)
    {
        this.soundEffectsVolume = volume;
        soundEffectsAudioMixer.SetFloat("volume", volume);
        OnVolumeChaned?.Invoke(this, EventArgs.Empty);
    }
}
