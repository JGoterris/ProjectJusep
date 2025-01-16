using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ActivateVictory : MonoBehaviour
{
    public GameObject victoryMenu;

    public void Victory()
    {
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
