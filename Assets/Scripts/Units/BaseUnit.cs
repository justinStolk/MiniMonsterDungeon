using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUnit : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Attack;
    public int Armor;
    public int MoveRange;

    public Slider HPSlider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeAttack(int power)
    {
        int expectedDamage = Mathf.Clamp(power - Armor, 0, 999);
        Health = Mathf.Clamp(Health - expectedDamage, 0, MaxHealth);
        HPSlider.value = (float)Health / MaxHealth;
        if(Health == 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        HPSlider.value = (float)Health / MaxHealth;
    }

    protected abstract void Die();

}
