using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    [SerializeField] public float Health;
    [SerializeField] float timeBeforeRegen;
    public bool isPlayerDead = false;

    private void Start()
    {
        Health = 100;
    }

    private void Update()
    {
        RegenerateHealth();
    }

    public void hurtPlayer(float damageTaken)
    {
        Health -= damageTaken;
        if(Health <= 0)
        {
            Destroy(gameObject);
            isPlayerDead = true;
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.enabled = false;
        }
    }

    void RegenerateHealth()
    {
        if(Health < 100)
        {
            timeBeforeRegen -= Time.deltaTime;
            if(timeBeforeRegen <= 0)
            {
                Health++;
                if(Health >= 100)
                {
                    Health = 100;
                    timeBeforeRegen = 3;
                }
            }
        }
    }

}
