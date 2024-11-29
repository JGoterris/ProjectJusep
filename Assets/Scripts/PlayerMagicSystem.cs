using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicSystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana;
    [SerializeField] private float manaRechargeRate = 2f;
    [SerializeField] private float timeBetweenCasts = 0.25f;
    private float currentCastTimer;
    [SerializeField] private Transform castPoint;
    private bool castingMagic = false;
    public Image manaBar = null;

    private void Awake(){
        currentMana = maxMana;
    }
    
    private void Update()
    {
        bool hasEnoughMana = currentMana >= spellToCast.SpellToCast.ManaCost;

        if(Input.GetMouseButtonDown(0) && !castingMagic && hasEnoughMana){
            castingMagic = true;
            // currentMana -= spellToCast.SpellToCast.ManaCost;
            UpdateMana(-spellToCast.SpellToCast.ManaCost);
            currentCastTimer = 0;
            CastSpell();
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

    void CastSpell(){
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }

    void UpdateMana(float amount)
    {   
        if (currentMana + amount >= 0) {
            currentMana += amount;

            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }

            if (manaBar != null)
            {
                manaBar.fillAmount = currentMana / maxMana;
            }
        }
    }
}
