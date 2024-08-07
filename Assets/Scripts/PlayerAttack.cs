using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private const float comboDelay = 1.8f;
    private float numClicks = 0f;
    private float lastClickTime = 0.75f;
    private Animator characterAnimator;
    [SerializeField] private GameObject swordTrigger;
    [SerializeField] private GameObject Enemy;
    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Time.time - lastClickTime > comboDelay)
        {
            numClicks = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            StartCoroutine(SwordTriggerCoroutine());
            lastClickTime = Time.time;
            numClicks++;

            switch (numClicks)
            {
                case 1:
                    characterAnimator.SetTrigger("SlashAttack1");
                    break;
                case 2:
                    characterAnimator.SetTrigger("SlashAttack2");
                    break;
                case 3:
                    characterAnimator.SetTrigger("SlashAttack3");
                    break;
            }

            numClicks = Mathf.Clamp(numClicks, 0, 3);
        }
    }

    private IEnumerator SwordTriggerCoroutine()
    {
        swordTrigger.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        swordTrigger.SetActive(false);
    }
}
