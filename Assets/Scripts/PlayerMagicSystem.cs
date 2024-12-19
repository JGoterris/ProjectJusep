using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicSystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCastLeft;
    [SerializeField] private Spell spellToCastRight;
    [SerializeField] private GameObject healthSpell;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana;
    [SerializeField] private float manaRechargeRate = 2f;
    [SerializeField] private float timeBetweenCasts = 0.25f;
    private float currentCastTimer;
    [SerializeField] private Transform castPoint;
    private bool castingMagic = false;
    public Image manaBar = null;
    private HealthComponent myHealth;

    private void Awake(){
        currentMana = maxMana;
        myHealth = GetComponent<HealthComponent>();
    }
    
    private void Update()
    {
        bool hasEnoughMana = currentMana >= spellToCastLeft.SpellToCast.ManaCost;

        if(Time.timeScale != 0 && Input.GetMouseButtonDown(0) && !castingMagic && hasEnoughMana){
            castingMagic = true;
            // currentMana -= spellToCastLeft.SpellToCast.ManaCost;
            UpdateMana(-spellToCastLeft.SpellToCast.ManaCost);
            currentCastTimer = 0;
            CastSpell(spellToCastLeft);
        }

        if(Time.timeScale != 0 && Input.GetMouseButtonDown(1) && !castingMagic && hasEnoughMana){
            castingMagic = true;
            // currentMana -= spellToCastLeft.SpellToCast.ManaCost;
            UpdateMana(-spellToCastRight.SpellToCast.ManaCost);
            currentCastTimer = 0;
            CastSpell(spellToCastRight);
        }

        if(Time.timeScale != 0 && Input.GetKeyDown("q") && !castingMagic && hasEnoughMana){
            castingMagic = true;
            UpdateMana(-30);
            currentCastTimer = 0;
            HealthSpell();
        }

        if(castingMagic){
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > timeBetweenCasts){
                castingMagic = false;
            }
        }

        if(currentMana < maxMana && !castingMagic){
            // currentMana += manaRechargeRate * Time.deltaTime;
            UpdateMana(manaRechargeRate * Time.deltaTime);
            //if(currentMana > maxMana){
            //    currentMana = maxMana;
            //}
        }
    }

    void CastSpell(Spell spellToCast){
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }

    void HealthSpell(){
        myHealth.Health(10);
        Vector3 point = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(healthSpell, point, castPoint.rotation);
    }

    void UpdateMana(float amount)
    {   
        if (currentMana + amount >= 0) {
            currentMana += amount;

            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }

            
        } else if (currentMana + amount < 0) {
            currentMana = 0;
        }
        if (manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
        }
    }
}
