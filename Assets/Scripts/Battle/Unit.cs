using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int maxHP;
    public int currentHP;

    public int maxTP;
    public int currentTP;

    public UnitType type;

    public int CritChance;
    public int attack;
    public int defense;
    public int skillAttack1;
    public int skillAttack2;
    public int skillAttack3;

    public string skill1;
    public string skill2;
    public string skill3;

    public enum UnitType
    {
        Fire,
        Water,
        Electric,
        Techno,
        Normal,
        Dark,
        Light,
        Magic
    }

    private static Dictionary<(UnitType, UnitType), float> typeEffectiveness = new Dictionary<(UnitType, UnitType), float>()
{
    {(UnitType.Electric, UnitType.Water), 1.5f},
    {(UnitType.Electric, UnitType.Techno), 1.2f},
    {(UnitType.Electric, UnitType.Dark), 0.5f},
    {(UnitType.Electric, UnitType.Electric), 0.5f},
    {(UnitType.Electric, UnitType.Fire), 1.0f},
    {(UnitType.Electric, UnitType.Normal), 1.0f},
    {(UnitType.Electric, UnitType.Light), 1.0f},
    {(UnitType.Electric, UnitType.Magic), 1.0f},

    {(UnitType.Fire, UnitType.Water), 0.5f},
    {(UnitType.Fire, UnitType.Techno), 1.0f},
    {(UnitType.Fire, UnitType.Electric), 1.0f},
    {(UnitType.Fire, UnitType.Normal), 1.0f},
    {(UnitType.Fire, UnitType.Dark), 1.0f},
    {(UnitType.Fire, UnitType.Light), 1.0f},
    {(UnitType.Fire, UnitType.Magic), 1.5f},

    {(UnitType.Water, UnitType.Fire), 1.5f},
    {(UnitType.Water, UnitType.Techno), 1.0f},
    {(UnitType.Water, UnitType.Electric), 0.5f},
    {(UnitType.Water, UnitType.Normal), 1.0f},
    {(UnitType.Water, UnitType.Dark), 1.0f},
    {(UnitType.Water, UnitType.Light), 1.0f},
    {(UnitType.Water, UnitType.Magic), 1.0f},

    {(UnitType.Techno, UnitType.Water), 1.0f},
    {(UnitType.Techno, UnitType.Fire), 1.0f},
    {(UnitType.Techno, UnitType.Electric), 0.8f},
    {(UnitType.Techno, UnitType.Normal), 1.5f},
    {(UnitType.Techno, UnitType.Dark), 1.5f},
    {(UnitType.Techno, UnitType.Light), 0.5f},
    {(UnitType.Techno, UnitType.Magic), 0.5f},

    {(UnitType.Normal, UnitType.Water), 1.0f},
    {(UnitType.Normal, UnitType.Fire), 1.0f},
    {(UnitType.Normal, UnitType.Electric), 1.0f},
    {(UnitType.Normal, UnitType.Techno), 0.5f},
    {(UnitType.Normal, UnitType.Dark), 1.0f},
    {(UnitType.Normal, UnitType.Light), 1.0f},
    {(UnitType.Normal, UnitType.Magic), 1.0f},

    {(UnitType.Dark, UnitType.Water), 1.0f},
    {(UnitType.Dark, UnitType.Fire), 1.0f},
    {(UnitType.Dark, UnitType.Electric), 1.5f},
    {(UnitType.Dark, UnitType.Techno), 0.5f},
    {(UnitType.Dark, UnitType.Normal), 1.0f},
    {(UnitType.Dark, UnitType.Light), 0.5f},
    {(UnitType.Dark, UnitType.Magic), 1.5f},

    {(UnitType.Light, UnitType.Water), 1.0f},
    {(UnitType.Light, UnitType.Fire), 1.0f},
    {(UnitType.Light, UnitType.Electric), 1.0f},
    {(UnitType.Light, UnitType.Techno), 1.5f},
    {(UnitType.Light, UnitType.Normal), 1.0f},
    {(UnitType.Light, UnitType.Dark), 1.5f},
    {(UnitType.Light, UnitType.Magic), 0.5f},

    {(UnitType.Magic, UnitType.Water), 1.0f},
    {(UnitType.Magic, UnitType.Fire), 0.5f},
    {(UnitType.Magic, UnitType.Electric), 1.0f},
    {(UnitType.Magic, UnitType.Techno), 1.5f},
    {(UnitType.Magic, UnitType.Normal), 1.0f},
    {(UnitType.Magic, UnitType.Dark), 0.5f},
    {(UnitType.Magic, UnitType.Light), 1.5f},
};



    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0) { 
            return true;
        }
        else { return false; }
    }

    public static float GetDamageMultiplier(UnitType attackerType, UnitType defenderType)
    {
        return typeEffectiveness.TryGetValue((attackerType, defenderType), out float multiplier) ? multiplier : 1.0f;
    }
}
