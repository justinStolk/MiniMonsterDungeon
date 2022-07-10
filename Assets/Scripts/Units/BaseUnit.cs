using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Attack;
    public int Armor;
    public int MoveRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
    }
}
