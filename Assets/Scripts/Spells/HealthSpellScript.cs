using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpellScript : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        Destroy(this.gameObject, 3);
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
    }
}
