using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))] 
public class enemyHealth : MonoBehaviour
{
    [SerializeField] int MaxHitpoints = 5;

    [Tooltip("Adds amount to MaxHitPoints when enemy dies")]

    [SerializeField] int DifficultyRamp = 1;
    int CurrentHitpoints = 0;

    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();    
    }
    void OnEnable()
    {
        CurrentHitpoints = MaxHitpoints;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

     void ProcessHit()
    {
        CurrentHitpoints--;
        if (CurrentHitpoints <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            MaxHitpoints += DifficultyRamp;
        }
    }
}
