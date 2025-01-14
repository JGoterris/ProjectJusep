using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossShield : MonoBehaviour, IDamageable
{
    private SphereCollider shieldCollider;
    private MeshRenderer sphereRenderer;
    public AudioSource audioSourceShieldDown;
    public AudioSource audioSourceShieldUp;
    public AudioSource audioSourceShieldBlock;


    // Start is called before the first frame update
    void Start()
    {
        shieldCollider = GetComponent<SphereCollider>();
        sphereRenderer = GetComponent<MeshRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // This is an undestructible shield, only when there are 0 enemies on the arena goes down
        audioSourceShieldBlock.Play();
    }

    public void Down()
    {
        shieldCollider.enabled = false;
        sphereRenderer.enabled = false;
        audioSourceShieldDown.Play();
    }

    public void Up()
    {
        shieldCollider.enabled = true;
        sphereRenderer.enabled = true;
        audioSourceShieldUp.Play();
    }
}
