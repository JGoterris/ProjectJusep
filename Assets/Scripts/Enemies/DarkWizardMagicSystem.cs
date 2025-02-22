using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizardMagicSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Spell fireBall;
    [SerializeField] private Transform castPoint;

    public void CastSpell(Quaternion rotation)
    {
        Instantiate(fireBall, castPoint.position, rotation);
    }

    public Spell GetSpell()
    {
        return fireBall;
    }

    public Transform GetCastPoint()
    {
        return castPoint;
    }
}
