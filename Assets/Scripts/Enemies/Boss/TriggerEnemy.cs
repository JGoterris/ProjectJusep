using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour, ITriggers
{
    public BossEnemy boss;

    public void Trigger(BossEnemy boss)
    {
        this.boss = boss;
    }

    private void OnDestroy()
    {
        boss.Trigger();
    }
}
