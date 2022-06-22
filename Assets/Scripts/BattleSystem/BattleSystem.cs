using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateResult(BaseUnit attacker, BaseUnit defender)
    {
        int expectedDamage = Mathf.Clamp(attacker.Attack - defender.Armor, 0, 999);
        int expectedRemainingHealth = defender.Health - expectedDamage;
        //First, we calculate the amount of damage we expect to do, based on the attack of the attacker and the armor of the defender.
        //Then, we calculate how much health the defender will be left with in this scenario. We can then update the interface.
    }

    public void ApplyBattleCalculation(BaseUnit attacker, BaseUnit defender)
    {
        int damage = Mathf.Clamp(attacker.Attack - defender.Armor, 0, 999);
        defender.ChangeHealth(-damage);
    }

}
