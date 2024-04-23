using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{

    public Animator animator;
    public GameObject chargingUI;
    public Image shrinkingCircle;
    public Transform Skill1FirePoint;
    public GameObject bulletPrefab;
    private GameObject currentBullet;

    enum SkillState { Ready, Charging, Attacking, Cancelled }
    public enum PlayerState { Nobody, Luana }
    public PlayerState characterName;

    private SkillState currentState = SkillState.Ready;
    private float circleShrinkSpeed = 1.0f;
    private float currentCircleScale = 1.0f;

    private BattleSystem battleSystem;
    private Vector3 enemyPosition;
    private int shoot;

    void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        shoot = 0;
    }


    void Update()
    {
        if(currentState == SkillState.Charging && shoot == 1)
        {
            if (Input.GetButtonDown("Timing"))
            {
                shoot = 0;
                PerformAttack();
            }
        }
    }

    public void SkillCharging()
    {
            currentState = SkillState.Charging;
            currentBullet = Instantiate(bulletPrefab, Skill1FirePoint.position, Skill1FirePoint.rotation);
            animator.Play("Luana Stand Left Skill Attack Weapon Charge");
    }

    private void PerformAttack()
    {
        if (currentState == SkillState.Charging)
        {
            currentState = SkillState.Attacking;
            animator.Play("Luana Stand Left Skill Attack Weapon Shoot");
            StartCoroutine(MoveBulletToEnemy(currentBullet, battleSystem.EnemySelectionPointerPosition, 1f));
        }
    }


    public void CancelAttack()
    {
        Destroy(currentBullet);
        animator.Play("Luana Stand Left Skill Attack Weapon Fail");
    }

    private IEnumerator MoveBulletToEnemy(GameObject bullet, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = bullet.transform.position;

        while (time < duration)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bullet.transform.position = targetPosition;

    }

    public void PrepareForSkillCharging()
    {
        if (characterName == PlayerState.Luana && currentState != SkillState.Charging)
        {
            currentState = SkillState.Ready;
        }
    }
}
