using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedEnemy : MonoBehaviour
{

    public CombatManager combatManager;

    private void OnDestroy()
    {
        if (combatManager != null)
        {
            combatManager.Decrease();
        }
    }
}
