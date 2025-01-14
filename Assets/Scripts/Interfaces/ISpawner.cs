using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner : ITriggerReceiver
{
    GameObject Spawn<T>(Transform swp, GameObject type) where T : Component, ITriggers;
}
