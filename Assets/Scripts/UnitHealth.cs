using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Slider component in Unity UI supports easy health bar creation. Slider has a public value that you set. Value = health and the component does the rest.
public class UnitHealth : MonoBehaviour
{
    public Slider slider;
    public int health = 3;

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int _damage)
    {
        health -= _damage;
    }

    void Update()
    {
        slider.value = health;
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
