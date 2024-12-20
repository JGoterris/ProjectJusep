using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Toggle toggleVolume;

    void Start(){
        Time.timeScale = 1;
        sliderVolume.value = PlayerPrefs.GetFloat("volume", 1);
        if(sliderVolume.value == 1)
            PlayerPrefs.SetFloat("volume", 1);
        toggleVolume.isOn = PlayerPrefs.GetInt("isMuted", 0) == 1 ? true : false;
        if(toggleVolume.isOn == false)
            PlayerPrefs.SetInt("isMuted", 0);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        AudioListener.pause = PlayerPrefs.GetInt("isMuted") == 1 ? true : false;
    }

    public void doExitGame()
    {
        Application.Quit();
    }

    public void startGame()
    {
        Application.LoadLevel("InitialRoom");
    }

    public void story()
    {
        Application.LoadLevel("Historia");
    }
    public void openSettings(){
        menu.SetActive(false);
        settings.SetActive(true);
    }
    public void closeSettings(){
        menu.SetActive(true);
        settings.SetActive(false);
    }
    public void openControls(){
        settings.SetActive(false);
        controls.SetActive(true);
    }
    public void closeControls(){
        settings.SetActive(true);
        controls.SetActive(false);
    }
    public void setVolume(){
        PlayerPrefs.SetFloat("volume", sliderVolume.value);
    }
    public void mute(){
        PlayerPrefs.SetInt("isMuted", toggleVolume.isOn ? 1 : 0);
    }
}

