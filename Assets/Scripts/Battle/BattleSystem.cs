using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.EventSystems;


public class BattleSystem : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }


    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject player2Prefab;
    public GameObject enemy2Prefab;
    public GameObject player3Prefab;
    public GameObject enemy3Prefab;

    public Transform playerPosition;
    public Transform enemyPosition;
    public Transform player2Position;
    public Transform enemy2Position;
    public Transform player3Position;
    public Transform enemy3Position;

    public Unit playerUnit;
    public Unit enemyUnit;
    public Unit enemyUnit2;
    public Unit enemyUnit3;
    public GameObject SelectedEnemyVariables;
    public Unit SelectedEnemyUnit;
    public Slider SelectedEnemyHPSlider;

    public Text EnemyNameText;
    public Text EnemyLevelText;

    public String PlayerAttackType;

    public Text PlayerNameText;
    public Text PlayerLevelText;
    public Text PlayerTPCurrentText;
    public Text PlayerTPMaxText;
    public Text PlayerCurrentHealthText;
    public Text PlayerMaxHealthText;
    public Slider PlayerHPSlider;
    public Slider PlayerTPSlider;
    public Slider EnemyHPSlider;
    public Slider EnemyHPSlider2;
    public Slider EnemyHPSlider3;

    public TextMeshProUGUI SkillName1;
    public TextMeshProUGUI SkillName2;
    public TextMeshProUGUI SkillName3;

    public BattleState state;

    public GameObject BattleMenu;

    public int LevelDamage;
    public int RandomDamage;
    public int PlayerDamage;
    public int EnemyDamage;

    public GameObject EnemySelectionPrefab;
    private GameObject EnemySelectionPointer;

    public GameObject StandardMenu;
    [SerializeField] public GameObject StandardMenuFirstButton;
    public GameObject SkillMenu;
    [SerializeField] public GameObject SKillMenuFirstButton;
    public GameObject EnemySelectionMenu;
    [SerializeField] public GameObject EnemySelectionMenuFirstButton;
    public GameObject ComboMenu;
    public GameObject ItemMenu;
    public EventSystem eventSystem;

    private GameObject playerVariables;
    public GameObject enemyVariables;
    public GameObject enemyVariables2;
    public GameObject enemyVariables3;
    public Vector3 EnemySelectionPointerPosition { get; private set; }

    public int SkillSelection;

    public GameObject RythmPrefab;
    public GameObject currentRythmPrefab;
    public Vector3 RythmPrefabPosition { get; private set; }
    public bool EnemyDead;
    public bool PlayerDead;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        BattleMenu.SetActive(false);
        EnemySelectionMenu.SetActive(false);
    }

    private void Update()
    {
        SpriteRenderer playerSpriteRenderer = playerVariables.GetComponent<SpriteRenderer>();
        Vector3 PlayerSpriteSize = playerSpriteRenderer.bounds.size;
        Vector3 RythmPrefabPosition = playerPosition.position + new Vector3(PlayerSpriteSize.y, PlayerSpriteSize.y/2, 0);
    }

    IEnumerator SetupBattle()
    {
        playerVariables = Instantiate(playerPrefab, playerPosition);
        playerUnit = playerVariables.GetComponent<Unit>();

        enemyVariables = Instantiate(enemyPrefab, enemyPosition);
        enemyVariables2 = Instantiate(enemy2Prefab, enemy2Position);
        enemyVariables3 = Instantiate(enemy3Prefab, enemy3Position);
        enemyUnit = enemyVariables.GetComponent<Unit>();
        enemyUnit2 = enemyVariables2.GetComponent<Unit>();
        enemyUnit3 = enemyVariables3.GetComponent<Unit>();

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
        //EnemyHPSlider2.maxValue = enemyUnit2.maxHP;
        //EnemyHPSlider2.value = enemyUnit2.currentHP;
        //EnemyHPSlider3.maxValue = enemyUnit3.maxHP;
        //EnemyHPSlider3.value = enemyUnit3.currentHP;

        SkillName1.text = playerUnit.skill1.ToString();
        SkillName2.text = playerUnit.skill2.ToString();
        SkillName3.text = playerUnit.skill3.ToString();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
    }

    void SetUpBattleMenu()
    {
        BattleMenu.transform.position = playerPosition.position + new Vector3(775, 390, 0);
        BattleMenu.SetActive(true);
        SkillMenu.SetActive(false);
        StandardMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
    }

    public void CloseBattleMenu()
    {
        BattleMenu.SetActive(false);
    }

    public IEnumerator PlayerAttack()
    {
        Debug.Log($"Attacking {SelectedEnemyUnit.unitName} with standard attack");
        yield return PerformAttack(playerUnit, SelectedEnemyUnit, SelectedEnemyHPSlider);
    }

    public IEnumerator EnemyTurn()
    {
        yield return PerformAttack(enemyUnit, playerUnit, PlayerHPSlider);
    }

    public IEnumerator PerformAttack(Unit attacker, Unit defender, Slider defenderHPSlider)
    {
        Debug.Log("Performing attack logic");
        int attackDamage = SkillSelection == 1 ? attacker.skillAttack1 :
                           SkillSelection == 2 ? attacker.skillAttack2 :
                           SkillSelection == 3 ? attacker.skillAttack3 : attacker.attack;

        int totalDamage = CalculateDamage(attacker, defender, attackDamage);
        bool isDead = defender.TakeDamage(totalDamage);
        defenderHPSlider.value = defender.currentHP;
        Debug.Log($"{attacker.unitName} attacks {defender.unitName} with total damage: {totalDamage}");
        yield return new WaitForSeconds(1f);
        if ((PlayerAttackType == "Standard" && state == BattleState.PLAYERTURN) || state == BattleState.ENEMYTURN)
        {
            EnemyTurnFunction(isDead, defender);
        }
    }

    public int CalculateDamage(Unit attacker, Unit defender, int attackDamage)
    {
        float baseDamage = attackDamage;
        float levelMultiplier = Mathf.Pow(0.95f, Math.Abs(attacker.unitLevel - defender.unitLevel));
        float randomMultiplier = UnityEngine.Random.Range(0.9f, 1.1f);
        float typeMultiplier = Unit.GetDamageMultiplier(attacker.type, defender.type);
        float critMultiplier = UnityEngine.Random.Range(1, 101) <= attacker.CritChance ? 1.5f : 1f;
        float defenseEffect = Mathf.Clamp(defender.defense * 0.1f, 0, baseDamage * 0.5f);
        int finalDamage = Mathf.RoundToInt((baseDamage * levelMultiplier * randomMultiplier * typeMultiplier * critMultiplier) - defenseEffect);

        return Math.Max(1, finalDamage);
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
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            SkillSelection = 0;
            PlayerAttackType = "Standard";
            Debug.Log("Standard attack selected");
            EnemySelection();
        }
    }

    public void OnSkillButton()
    { 
        SkillMenu.SetActive(true);
        StandardMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SKillMenuFirstButton);
    }

    public void OnComboButton()
    {
        
    }

    public void OnItemButton()
    {
        
    }

    public void OnSkill1()
    {
        if (state == BattleState.PLAYERTURN)
        {
            SkillSelection = 1;
            PlayerAttackType = "Skill";
            EnemySelection();
        }
    }

    public void OnSkill2()
    {
        if (state == BattleState.PLAYERTURN)
        {
            SkillSelection = 2;
            PlayerAttackType = "Skill";
            EnemySelection();
        }
    }

    public void OnSkill3()
    {
        if (state == BattleState.PLAYERTURN)
        {
            SkillSelection = 3;
            PlayerAttackType = "Skill";
            EnemySelection();
        }
    }

    public void OnSkillBack()
    {
        SkillMenu.SetActive(false);
        StandardMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
        SkillSelection = 0;
    }

    public void EnemySelection()
    {
        BattleMenu.SetActive(false);
        EnemySelectionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(EnemySelectionMenuFirstButton);
    }

    public void EnemySelectionPointerEvent(GameObject enemyVariablesChoice, Transform EnemyPositionChoice)
    {

        SpriteRenderer enemySpriteRenderer = enemyVariablesChoice.GetComponent<SpriteRenderer>();
        Vector3 spriteSize = enemySpriteRenderer.bounds.size;
        Vector3 enemySelectionPointerPosition = EnemyPositionChoice.position + new Vector3(0, spriteSize.y / 2 + spriteSize.y / 8, 0);
        if (EnemySelectionPointer != null)
        {
            Destroy(EnemySelectionPointer);
        }
        EnemySelectionPointer = Instantiate(EnemySelectionPrefab, enemySelectionPointerPosition, Quaternion.identity);
    }

    public void EnemySelectionPointerEvent1()
    { 
        EnemySelectionPointerEvent(enemyVariables, enemyPosition);
        SelectedEnemyVariables = enemyVariables;
        SelectedEnemyUnit = enemyUnit;
        SelectedEnemyHPSlider = EnemyHPSlider;
    }

    public void EnemySelectionPointerEvent2()
    { 
        EnemySelectionPointerEvent(enemyVariables2, enemy2Position);
        SelectedEnemyVariables = enemyVariables2;
        SelectedEnemyUnit = enemyUnit2;
        SelectedEnemyHPSlider = EnemyHPSlider2;
    }

    public void EnemySelectionPointerEvent3()
    { 
        EnemySelectionPointerEvent(enemyVariables3, enemy3Position);
        SelectedEnemyVariables = enemyVariables3;
        SelectedEnemyUnit = enemyUnit3;
        SelectedEnemyHPSlider = EnemyHPSlider3;
    }

    public void EnemyButtonDownSelected()
    {
        Destroy(EnemySelectionPointer);
        Debug.Log("Enemy Button Down");
        Skills skills = playerVariables.GetComponent<Skills>();
        EnemySelectionMenu.SetActive(false);
        CloseBattleMenu();

        if (PlayerAttackType == "Standard")
        {
            Debug.Log("Executing standard attack");
            StartCoroutine(PlayerAttack());
        }
        if (PlayerAttackType == "Skill")
        {
            if (SkillSelection == 1)
            {
                currentRythmPrefab = Instantiate(RythmPrefab, RythmPrefabPosition, Quaternion.identity);
                skills.SkillCharging();
            }
            if (EnemySelectionPointer != null)
            {
                Destroy(EnemySelectionPointer);
                EnemySelectionPointer = null;
            }
            EnemySelectionMenu.SetActive(false);
        }
    }

    public void EnemyTurnFunction(bool isEnemyDead, Unit defender)
    {
        if (isEnemyDead)
        {
            state = (defender == playerUnit) ? BattleState.LOST : BattleState.WON;
            EndBattle();
        }
        else
        {
            state = (defender == playerUnit) ? BattleState.ENEMYTURN : BattleState.PLAYERTURN;
            if (state == BattleState.ENEMYTURN)
            {
                StartCoroutine(EnemyTurn());
            }
            else
            {
                PlayerTurn();
            }
        }
    }

    public void CheckEnemyStatusAndContinue()
    {
        bool isEnemyDead = SelectedEnemyUnit.currentHP <= 0;
        EnemyTurnFunction(isEnemyDead, SelectedEnemyUnit);
    }
}
