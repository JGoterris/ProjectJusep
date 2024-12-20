using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    // This health system does nothing
    private ShieldHealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GetComponent<ShieldHealthSystem>();
    }

    public void Disable()
    {
        this.enabled = false;
    }

    public void Enable()
    {
        this.enabled = true;
    }
}
