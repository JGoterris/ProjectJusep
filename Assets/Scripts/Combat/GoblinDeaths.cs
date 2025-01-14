using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDeaths : MonoBehaviour
{
    public int numGoblins;
    public GameObject[] puertas;
    void Update()
    {
        if(numGoblins == 0){
            for(int i = 0; i < puertas.Length; i++)
                Destroy(puertas[i]);
        }
    }

    public void goblinMuerto(){
        numGoblins--;
    }
}
