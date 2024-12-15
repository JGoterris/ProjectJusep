using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
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
}

