using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUp
{
   
    public float speedMod;
    public float damageMod;
    public float fireRateMod;
    public bool shield;
    public bool inviz;

    
    public float healHealth;
    public float maxHealthMod;

    
    public float durration;
    public bool perm;

    
    public PowerUp()
    {
        
        speedMod = damageMod = fireRateMod = healHealth = maxHealthMod = 0;

        
        shield = inviz = perm = false;
    }

    #region get and set methods of all variables
    public void SetSpeed() { }
    public float GetSpeed() { return speedMod; }
    public void SetDamage() { }
    public float GetDamage() { return damageMod; }
    public void SetFireRate() { }
    public float GetFirerate() { return fireRateMod; }
    public void SetHealHealth() { }
    public float GetHealHealth() { return healHealth; }
    public void SetMaxHealthMod() { }
    public float GetMaxHealthMod() { return maxHealthMod; }

    public void setShield()
    {
        shield = !shield; 
    }

    public void SetInviz()
    {
        inviz = !inviz; 
    }

    public void SetPermanent()
    {
        perm = !perm; 
    }

    
    public bool IsShielded()
    {
        if (shield)
        {
            return true;
        }

        return false;
    }

    public bool IsInvisible()
    {
        if (inviz)
        {
            return true;
        }

        return false;
    }

    public bool IsPermanent()
    {
        if (perm)
        {
            return true;
        }

        return false;
    }

    #endregion

    public void OnActivate(TankData target)
    {
        target.frontSpeed += speedMod;
        target.backSpeed += speedMod;
        target.damageDone += damageMod;
        target.fireRate += fireRateMod;
        target.maxHealth += maxHealthMod;
        target.health += healHealth;

        if (IsShielded())
        {
            target.gameObject.AddComponent<SphereCollider>();

            SphereCollider shield = target.gameObject.GetComponent<SphereCollider>();
            shield.isTrigger = true;
            shield.radius = 1.0f;

        }
    }

    public void OnDeactivate(TankData target)
    {
        target.frontSpeed -= speedMod;
        target.backSpeed -= speedMod;
        target.damageDone -= damageMod;
        target.fireRate -= fireRateMod;
        target.maxHealth -= maxHealthMod;
        target.health -= healHealth;
        setShield();
        SetInviz();
    }

    
}
