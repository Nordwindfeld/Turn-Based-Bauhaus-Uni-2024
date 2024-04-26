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

    public EnemySelectionState EnemyState;
    public enum EnemySelectionState { NOTHING, ENEMY};

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

    public GameObject EnemySelectionPrefab;
    private GameObject EnemySelectionPointer;

    public GameObject StandardMenu;
    [SerializeField] public GameObject StandardMenuFirstButton;
    public GameObject SkillMenu;
    [SerializeField] public GameObject SKillMenuFirstButton;
    public GameObject ComboMenu;
    public GameObject ItemMenu;
    public EventSystem eventSystem;

    private GameObject playerVariables;
    public GameObject enemyVariables;
    public Vector3 EnemySelectionPointerPosition { get; private set; }

    public int SkillSelection;

    public GameObject RythmPrefab;
    public GameObject currentRythmPrefab;
    public Vector3 RythmPrefabPosition { get; private set; }

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        BattleMenu.SetActive(false);
    }

    private void Update()
    {
        SpriteRenderer playerSpriteRenderer = playerVariables.GetComponent<SpriteRenderer>();
        Vector3 PlayerSpriteSize = playerSpriteRenderer.bounds.size;
        Vector3 RythmPrefabPosition = playerPosition.position + new Vector3(PlayerSpriteSize.y, PlayerSpriteSize.y/2, 0);
        if (EnemyState == EnemySelectionState.ENEMY)
        {
            Skills skills = playerVariables.GetComponent<Skills>();

            if (Input.GetButtonDown("Enter"))
            {
                if (EnemyState == EnemySelectionState.ENEMY && !BattleMenu.activeSelf)
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
                    EnemyState = EnemySelectionState.NOTHING;
                }
            }
        }
    }

    IEnumerator SetupBattle()
    {
        playerVariables = Instantiate(playerPrefab, playerPosition);
        playerUnit = playerVariables.GetComponent<Unit>();

        enemyVariables = Instantiate(enemyPrefab, enemyPosition);
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

    IEnumerator PlayerAttack()
    {
        CloseBattleMenu();
        yield return PerformAttack(playerUnit, enemyUnit, EnemyHPSlider);
    }

    public IEnumerator EnemyTurn()
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
            state = (defender == playerUnit) ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
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
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
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
            EnemySelection(1);
            Skills skills = playerVariables.GetComponent<Skills>();
        }
    }

    public void OnSkill2()
    {
        EnemySelection(2);
    }

    public void OnSkill3()
    {
        EnemySelection(3);
    }

    public void OnSkillBack()
    {
        SkillMenu.SetActive(false);
        StandardMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(StandardMenuFirstButton);
    }

    public void EnemySelection(int skill)
    {
        BattleMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        SpriteRenderer enemySpriteRenderer = enemyVariables.GetComponent<SpriteRenderer>();
        Vector3 spriteSize = enemySpriteRenderer.bounds.size;
        Vector3 enemySelectionPointerPosition = enemyPosition.position + new Vector3(0, spriteSize.y/2 + spriteSize.y / 8, 0);
        if (EnemySelectionPointer != null)
        {
            Destroy(EnemySelectionPointer);
        }
        EnemySelectionPointer = Instantiate(EnemySelectionPrefab, enemySelectionPointerPosition, Quaternion.identity);

        EnemyState = EnemySelectionState.ENEMY;
        SkillSelection = skill;
    }
}
