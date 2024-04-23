using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYRTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    Unit playerUnit;
    Unit enemyUnit;

    public Text EnemyNameText;
    public Text EnemyLevelText;

    public Text PlayerNameText;
    public Text PlayerLevelText;
    public Text PlayerTPCurrentText;
    public Text PlayerTPMaxText;
    public Text PlayerCurrentHealthText;
    public Text PlayerMaxHealthText;
    public Slider PlayerHPSlider;
    public Slider PlayerTPSlider;
    public Slider EnemyHPSlider;

    public TextMeshProUGUI SkillName1;
    public TextMeshProUGUI SkillName2;
    public TextMeshProUGUI SkillName3;

    public BattleState state;

    public GameObject BattleMenu;

    int LevelDamage;
    int RandomDamage;
    int PlayerDamage;
    int EnemyDamage;

    public GameObject StandardMenu;
    public GameObject SkillMenu;
    public GameObject ComboMenu;
    public GameObject ItemMenu;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        BattleMenu.SetActive(false);
    }

    IEnumerator SetupBattle()
    {
        GameObject playerVariables = Instantiate(playerPrefab, playerPosition);
        playerUnit = playerVariables.GetComponent<Unit>();

        GameObject enemyVariables = Instantiate(enemyPrefab, enemyPosition);
        enemyUnit = enemyVariables.GetComponent<Unit>();

        EnemyNameText.text = enemyUnit.unitName;
        EnemyLevelText.text = enemyUnit.unitLevel.ToString("D2");

        PlayerNameText.text = playerUnit.unitName;
        PlayerLevelText.text = playerUnit.unitLevel.ToString("D2");
        PlayerTPCurrentText.text = playerUnit.currentTP.ToString("D3");
        PlayerTPMaxText.text = playerUnit.maxTP.ToString("D3");
        PlayerCurrentHealthText.text = playerUnit.currentHP.ToString("D3");
        PlayerMaxHealthText.text = playerUnit.maxHP.ToString("D3");
        PlayerHPSlider.maxValue = playerUnit.maxHP;
        PlayerHPSlider.value = playerUnit.currentHP;
        PlayerTPSlider.maxValue = playerUnit.maxTP;
        PlayerTPSlider.value = playerUnit.currentTP;
        EnemyHPSlider.maxValue = enemyUnit.maxHP;
        EnemyHPSlider.value = enemyUnit.currentHP;

        SkillName1.text = playerUnit.skill1.ToString();
        SkillName2.text = playerUnit.skill2.ToString();
        SkillName3.text = playerUnit.skill3.ToString();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void SetUpBattleMenu()
    {
        BattleMenu.transform.position = playerPosition.position + new Vector3(775, 390, 0);
        BattleMenu.SetActive(true);
    }

    public void CloseBattleMenu()
    {
        BattleMenu.SetActive(false);
    }

    IEnumerator PlayerAttack()
    {
        CloseBattleMenu();
        yield return PerformAttack(playerUnit, enemyUnit, EnemyHPSlider);
    }

    IEnumerator EnemyTurn()
    {
        yield return PerformAttack(enemyUnit, playerUnit, PlayerHPSlider);
    }

    private IEnumerator PerformAttack(Unit attacker, Unit defender, Slider defenderHPSlider)
    {
        yield return new WaitForSeconds(2f);

        int standardAttackDamage = attacker.attack;
        int levelDamage;
        int randomDamage = UnityEngine.Random.Range(-1, 3);
        int critMultiplier = UnityEngine.Random.Range(1, 101) <= attacker.CritChance ? UnityEngine.Random.Range(1, 4) : 1;
        int defenseEffect = (int)(defender.defense * 0.2);

        float typeMultiplier = Unit.GetDamageMultiplier(attacker.type, defender.type);

        if (attacker.unitLevel > defender.unitLevel)
        {
            levelDamage = (int)(Math.Abs(attacker.unitLevel - defender.unitLevel) * 0.5);
        }
        else if (attacker.unitLevel < defender.unitLevel)
        {
            levelDamage = (int)(Math.Abs(attacker.unitLevel - defender.unitLevel) * -0.4);
        }
        else
        {
            levelDamage = 0;
        }

        int rawDamage = standardAttackDamage + levelDamage + randomDamage - defenseEffect;
        int effectiveDamage = Mathf.Max(0, rawDamage);
        int totalDamage = Mathf.FloorToInt(effectiveDamage * critMultiplier * typeMultiplier);

        bool isDead = defender.TakeDamage(totalDamage);

        Debug.Log($"{attacker.unitName} attacks {defender.unitName} with total damage: {totalDamage} (Type multiplier: {typeMultiplier})");

        defenderHPSlider.value = defender.currentHP;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = (defender == playerUnit) ? BattleState.LOST : BattleState.WON;
            EndBattle();
        }
        else
        {
            state = (defender == playerUnit) ? BattleState.PLAYERTURN : BattleState.ENEMYRTURN;
            if (state == BattleState.PLAYERTURN)
            {
                PlayerTurn();
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }
    }


    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            // Programmieren was dann passieren soll.
        } 
        else if (state == BattleState.LOST)
        {
            // Programmieren bei Lose
        }
    }

    void PlayerTurn()
    {
        SetUpBattleMenu();
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    public void OnSkillButton()
    { 
        SkillMenu.SetActive(true);
        StandardMenu.SetActive(false);
    }

    public void OnComboButton()
    {
        
    }

    public void OnItemButton()
    {
        
    }

    public void OnSkill1()
    {

    }

    public void OnSkill2()
    {
           
    }

    public void OnSkill3()
    {

    }

    public void OnSkillBack()
    {
        SkillMenu.SetActive(false);
        StandardMenu.SetActive(true);
    }
}
