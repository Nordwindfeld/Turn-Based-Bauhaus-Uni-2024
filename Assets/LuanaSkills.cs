using System;
using UnityEngine;
using UnityEngine.UI;

enum SkillState { Ready, Charging, Attacking, Cancelled }
public class LuanaSkills : MonoBehaviour
{
    public Animator animator; 
    public GameObject chargingUI; 
    public Image shrinkingCircle;
    public Transform Skill1FirePoint;
    public GameObject bulletPrefab;

    private SkillState currentState = SkillState.Ready;
    private float circleShrinkSpeed = 1.0f;
    private float currentCircleScale = 1.0f;

    void Start()
    {
        
    }


    void Update()
    {
        if (currentState == SkillState.Charging)
        {
            currentCircleScale -= Time.deltaTime * circleShrinkSpeed;
            shrinkingCircle.transform.localScale = new Vector3(currentCircleScale, currentCircleScale, 1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                    PerformAttack();

                    currentState = SkillState.Cancelled;
            }

            if (currentCircleScale <= 0)
            {
                currentState = SkillState.Cancelled;
            }
        }
    }

    public void SkillCharging()
    {
        currentState = SkillState.Charging;
        Instantiate(bulletPrefab, Skill1FirePoint.position, Skill1FirePoint.rotation);
        animator.Play("Luana Stand Left Skill Attack Weapon Charge");
        chargingUI.SetActive(true);
    }

    private void PerformAttack()
    {
        // Attacken-Logik, Animation spielen
        animator.Play("Luana Stand Left Skill Attack Weapon Shoot");
        // Kreis für den nächsten Angriff vorbereiten oder Zustand ändern
        // ...
    }
}
