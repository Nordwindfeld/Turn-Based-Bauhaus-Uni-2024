using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{

    public Animator animator;
    public Transform Skill1FirePoint;
    public GameObject bulletPrefab;
    private GameObject currentBullet;

    public enum SkillState { Ready, Charging, Attacking, Cancelled }
    public enum PlayerState { Nobody, Luana }
    public PlayerState characterName;

    private SkillState currentState = SkillState.Ready;

    private GameObject battleSystem;
    private Vector3 enemyPosition;
    public int Rythm;

    private GameObject rythmScript;


    [System.Obsolete]
    void Start()
    {
        battleSystem = GameObject.FindWithTag("battlesystem");
        rythmScript = GameObject.FindWithTag("RythmusScript");
    }


    void Update()
    {
        if(currentState == SkillState.Charging)
        {
            if (Input.GetButtonDown("Timing") && rythmScript.GetComponent<RythmScript>().CorrectTiming == 1)
            {
                PerformAttack();
            }
        }


    }

    public void SkillCharging()
    {
            currentState = SkillState.Charging;
            animator.Play("Luana Stand Left Skill Attack Weapon Charge");
    }

    public void PerformAttack()
    {
        if (currentState == SkillState.Charging)
        {
            animator.Play("Luana Stand Left Skill Attack Weapon Shoot");
            StartCoroutine(MoveBulletToEnemy(currentBullet, battleSystem.GetComponent<BattleSystem>().EnemySelectionPointerPosition*2, 1f));
        }
    }


    public void CancelAttack()
    {
        Destroy(currentBullet);
        animator.Play("Luana Stand Left Skill Attack Weapon Fail");
        currentState = SkillState.Ready;
        StartCoroutine(battleSystem.GetComponent<BattleSystem>().EnemyTurn());
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

    public void BulletAppears()
    {
        currentBullet = Instantiate(bulletPrefab, Skill1FirePoint.position, Skill1FirePoint.rotation);
    }

    public void Ready()
    {
        currentState = SkillState.Ready;
    }
}
