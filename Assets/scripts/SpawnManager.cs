using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public int randomSpawnIndex;

    [SerializeField] int enemiesCurrentlySpawned;
    [SerializeField] int maxEnemiesPerWave;
    public int currentRound;

    [SerializeField] int enemiesRemaning;
    [SerializeField] int enemiesPerRound;
    [SerializeField] int enemiesLeftToBeSpawned;

    [SerializeField] float timeBetweenSpawnsMin;
    [SerializeField] float timeBetweenSpawnsMax;
    [SerializeField] float spawnTime;
    [SerializeField] float freeTimeBetweenRounds;

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [SerializeField] bool maxPerWave;
    [SerializeField] public bool isRoundChanging = false;

    public float healthMultiplier;

    public bool doDamage = false;

    private void Start()
    {
        spawnTime = Random.Range(timeBetweenSpawnsMin, timeBetweenSpawnsMax);
        currentRound = 1;
        enemiesRemaning = enemiesPerRound;
        enemiesLeftToBeSpawned = enemiesRemaning;
        enemyPrefab.GetComponent<Enemy>().EnemyHealth = 150;

    }
    private void Update()
    {           
        spawnEnemies();        
    }

    void spawnEnemies()
    {    
        if(spawnTime > 0 && !isRoundChanging)
        {
            spawnTime -= Time.deltaTime;
        }
        
        
        if(enemiesRemaning > 0)
        {
            if (spawnTime <= 0.0f && enemiesCurrentlySpawned <= maxEnemiesPerWave && !maxPerWave && enemiesLeftToBeSpawned > 0)
            {
                randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefab, spawnPoints[randomSpawnIndex].transform.position, Quaternion.Euler(0, 0, 0));
                enemiesCurrentlySpawned++;
                enemiesLeftToBeSpawned--;

                spawnTime = Random.Range(timeBetweenSpawnsMin, timeBetweenSpawnsMax);           
            }

            if (enemiesCurrentlySpawned == maxEnemiesPerWave)
            {
                maxPerWave = true;
            }
            else if (enemiesCurrentlySpawned < maxEnemiesPerWave)
            {
                maxPerWave = false;
            }
        }
        else
        {
            spawnTime = -1.0f;
        }
        
    }

    public void deductEnemiesInRound(int zombiesKilled)
    {
        enemiesRemaning -= zombiesKilled;
        enemiesCurrentlySpawned--;
        checkRound();

    }

    void checkRound()
    {
        
        if (enemiesRemaning == 0)
        {
            isRoundChanging = true;

            StartCoroutine(waveTransition());              
        }
    }

    IEnumerator waveTransition()
    {
        yield return new WaitForSeconds(freeTimeBetweenRounds);
        currentRound++;
        enemiesPerRound += 4;
        enemiesRemaning = enemiesPerRound;
        enemiesLeftToBeSpawned = enemiesRemaning;
        spawnTime = Random.Range(timeBetweenSpawnsMin, timeBetweenSpawnsMax);

        if (currentRound > 1 && currentRound < 9)
        {
            healthMultiplier += 100;
            enemyPrefab.GetComponent<Enemy>().IncreaseEnemyHP(healthMultiplier);
        }
        isRoundChanging = false;
    }
}
