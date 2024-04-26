using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmScript : MonoBehaviour
{
    public GameObject RythmBox;
    public int CorrectTiming;
    public Animator Anim;
    public int CorrectHits;
    public GameObject PlayerScript;

    void Start()
    {
        Anim.GetComponent<Animator>();
        Anim.Play("DefaultField");
        CorrectHits = 0;
        PlayerScript = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        if (Input.GetButtonDown("Enter") && CorrectTiming == 1)
        {
            Anim.Play("CorrectField");
            PlayerScript = GameObject.FindWithTag("Player");
            PlayerScript.GetComponent<Skills>().PerformAttack();
            CorrectHits++;
            float CorrectMultiplier = Mathf.Sqrt(CorrectHits);
            PlayerScript.GetComponent<Animator>().speed = 1f * CorrectMultiplier;
            Anim.GetComponent<Animator>().speed = 1f * CorrectMultiplier;
            PlayerScript.GetComponent<Skills>().bulletPrefab.GetComponent<Animator>().speed = 1f * CorrectMultiplier;
        }
        if (Input.GetButtonDown("Enter") && CorrectTiming == 0)
        {
            CancelAttack();
        }
    }

    public void TimingCorrect(){
            CorrectTiming = 1;
        }

    public void TimingFail()
    {
        CorrectTiming = 0;
    }

    public void DestroyRythmus()
    {
        Destroy(RythmBox);
    }

    public void CancelAttack()
    {
        Anim.Play("CancelField");
        CorrectHits = 0;
        PlayerScript.GetComponent<Animator>().speed = 1f;
        Anim.GetComponent<Animator>().speed = 1f;
        PlayerScript.GetComponent<Skills>().bulletPrefab.GetComponent<Animator>().speed = 1f;
        CorrectHits = 0;
        PlayerScript.GetComponent<Skills>().CancelAttack();
    }
}
