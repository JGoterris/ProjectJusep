using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private bool paused = false;
    private bool sePuedePausar = true;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Toggle toggleVolume;
    [SerializeField] private AudioSource[] pauseSounds;

    void Start(){
        paused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        sliderVolume.value = PlayerPrefs.GetFloat("volume");
        toggleVolume.isOn = PlayerPrefs.GetInt("isMuted") == 1 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && sePuedePausar){
			pauseUnpause();
        }
    }

    public void pauseUnpause(){
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        
        pauseUI.SetActive(paused);
        
        if(paused){
            for(int i = 0; i < pauseSounds.Length; i++)
                pauseSounds[i].Stop();
        }

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public void exit(){
        Application.LoadLevel("Menu");
    }

    public void setVolume(){
        PlayerPrefs.SetFloat("volume", sliderVolume.value);
    }
    public void mute(){
        PlayerPrefs.SetInt("isMuted", toggleVolume.isOn ? 1 : 0);
    }

    public void restart(){
        Application.LoadLevel("InitialRoom");
    }

    public void yaNoSePuedePausar(){
        sePuedePausar = false;
    }
}
