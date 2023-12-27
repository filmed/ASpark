using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    public bool isDead = false;

    public void InitHealth(int _healthValue) 
    {
        currentHealth = _healthValue;
        maxHealth = _healthValue;
        isDead = false;
    }

    public void GetHit(float _amount, GameObject _sender) 
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        if (currentHealth > 0) 
        {
            OnHitWithReference?.Invoke(_sender);



        }
        else 
        {
            OnDeathWithReference?.Invoke(_sender);
            isDead = true;
            Destroy(gameObject);
        }
    }
}
