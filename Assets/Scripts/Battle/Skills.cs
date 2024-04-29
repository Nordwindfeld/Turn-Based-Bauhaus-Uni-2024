using System.Collections;
using UnityEditor.Experimental.GraphView;
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
                Attack();
            }
        }


    }

    public void SkillCharging()
    {
            currentState = SkillState.Charging;
            animator.Play("Luana Stand Left Skill Attack Weapon Charge");
    }

    public void Attack()
    {
        if (currentState == SkillState.Charging)
        {
            animator.Play("Luana Stand Left Skill Attack Weapon Shoot");
            StartCoroutine(MoveBulletToEnemy(currentBullet, battleSystem.GetComponent<BattleSystem>().enemyPosition.position, 1f));
            battleSystem.GetComponent<BattleSystem>().PlayerAttack();
        }
    }


    public void CancelAttack()
    {
        Destroy(currentBullet);
        animator.Play("Luana Stand Left Skill Attack Weapon Fail");
        currentState = SkillState.Ready;
        battleSystem.GetComponent<BattleSystem>().CheckEnemyStatusAndContinue();
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

        yield return StartCoroutine(battleSystem.GetComponent<BattleSystem>().PerformAttack(battleSystem.GetComponent<BattleSystem>().playerUnit, battleSystem.GetComponent<BattleSystem>().SelectedEnemyUnit, battleSystem.GetComponent<BattleSystem>().SelectedEnemyHPSlider));


        yield return new WaitForSeconds(0.5f);
        Animator bulletAnimator = bullet.GetComponent<Animator>();
        if (bulletAnimator != null)
        {
            bulletAnimator.Play("ShootExplode");
        }

        Destroy(bullet, 0.5f);
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
