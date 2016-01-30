﻿using UnityEngine;
using System.Collections;

public class SpawnBehaviour : MonoBehaviour {
    [SerializeField]
    float currentNewWaveTimer = 0.0f;
    [SerializeField]
    float currentWaveEnemySpawnTimer = 0.0f;
    Level currentLevel;
    [SerializeField]
    int currentWave = 0;
    bool waveSpawnedCompletely = false;
    public GameObject player;
	// Use this for initialization
	void Start () {
        LoadLevelData("level1.txt");
	}
	
	// Update is called once per frame
	void Update () { 
        currentNewWaveTimer += Time.deltaTime;
        // spawn a new wave when we have spawned all enemies in a wave but no sooner than wavetimer
        if(currentNewWaveTimer > currentLevel.waveSpawnTimer && waveSpawnedCompletely) {
                SpawnWave();
        } else {
            // could be invoked, but dunno if it is better
            // decrease the amount of enemies in a wave by one and spawn a new one and reset spawntimer
            if (currentWaveEnemySpawnTimer > currentLevel.waveLength)
            {
                currentWaveEnemySpawnTimer = 0.0f;
                if (currentLevel.enemies[currentWave].amount > 0)
                {
                    GameObject spawnedEnemy;
                    spawnedEnemy = (GameObject)Instantiate(Resources.Load("Prefabs/" + currentLevel.enemies[currentWave].enemyPrefab), Vector3.zero, Quaternion.identity);  // prefabs need to go to Resources/Prefabs or otherwise everything is terrible
                    spawnedEnemy.GetComponent<Enemy>().SetData(currentLevel.enemies[currentWave]);
                    currentLevel.enemies[currentWave].amount -= 1;
                } else
                {
                    waveSpawnedCompletely = true;
                }
            } else {
                currentWaveEnemySpawnTimer += Time.deltaTime;
            }
        }
	}

    void SpawnWave() {
        currentWave += 1;
        currentNewWaveTimer = 0.0f;
    }

    public void LoadLevelData(string levelName) {
        string path = "LevelData/" + levelName.Replace(".txt", "");  
        string jsonData = Resources.Load<TextAsset>(path).text;
        currentLevel = JsonUtility.FromJson<Level>(jsonData);
        currentNewWaveTimer = 0.0f;
    }
}