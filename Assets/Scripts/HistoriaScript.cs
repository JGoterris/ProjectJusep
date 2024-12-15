using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HistoriaScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public void Start()
    {
        videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
    }

    public void Update()
    {
        // Detecta si el video ha terminado
        if (videoPlayer.frame >= (long)(videoPlayer.frameCount - 1))
        {
            volver();
        }
    }

    public void volver()
    {
        Application.LoadLevel("Menu");
    }
}
