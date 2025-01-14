using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEnemy : MonoBehaviour, ITriggers
{
    // Start is called before the first frame update
     ITriggerReceiver receiver;

    public void Trigger(ITriggerReceiver receiver)
    {
        this.receiver = receiver;
    }

    private void OnDestroy()
    {
        if (receiver != null)
        {
            receiver.Decrease();
        }
    }
}
