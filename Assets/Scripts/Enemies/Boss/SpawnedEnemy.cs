using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEnemy : MonoBehaviour, ITriggers
{
    // Start is called before the first frame update
    public BossEnemy boss;

    public void Trigger(BossEnemy boss)
    {
        this.boss = boss;
    }

    private void OnDestroy()
    {
        boss.Decrease();
    }
}
