using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{

    [SerializeField] protected int _cost;
    [Min(1)]
    [SerializeField] protected int _maxLevel;
    [SerializeField] protected bool _unlocked;

    [SerializeField] protected BaseSkill _nextUnlockedSkill;
    [SerializeField] protected int _nextUnlockLevel;

    protected int _upgradeLevel = 0; 

    // Start is called before the first frame update
    void Start()
    {
        if (_nextUnlockLevel > _maxLevel && _nextUnlockedSkill != null)
        {
            Debug.LogError($"A skill of type {GetType()} has an unlock level larger than the max level. The next skill can't be unlocked like this!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockSkill()
    {
        _unlocked = true;
        //Add code to make the corresponding button interactable.
    }

    public void UpgradeSkill()
    {
        if(_upgradeLevel < _maxLevel)
        {
            ApplySkill();
            if(_upgradeLevel == _nextUnlockLevel && _nextUnlockedSkill != null)
            {
                _nextUnlockedSkill.UnlockSkill();
            }
            return;
        }
        Debug.LogWarning("Trying to upgrade a fully upgraded skill! This is impossible!");
    }

    protected abstract void ApplySkill();

}
