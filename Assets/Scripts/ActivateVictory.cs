using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ActivateVictory : MonoBehaviour
{
    public GameObject victoryMenu;
    [SerializeField] private PauseScript pauseScript;

    public void Victory()
    {
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        pauseScript.yaNoSePuedePausar();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
