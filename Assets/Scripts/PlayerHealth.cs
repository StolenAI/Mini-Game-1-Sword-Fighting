using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    private int health = 100;
    private Animator CharacterAnimator;
    public PlayerHealthBar HealthBar;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponent<Animator>();
        HealthBar.SetMaxHealth(health);
    }
    public void PlayerGotHit()
    {
        health -= 5;
        HealthBar.SetHealth(health);
        if (health <= 0)
            player.SendMessage("PlayerDied");
    }
}

