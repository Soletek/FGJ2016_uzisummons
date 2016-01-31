	using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public List<GameObject> enemiesInScene;
    int levelNumber = 1;
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
                try {
                    if (currentLevel.enemies[currentWave].amount > 0)
                    {
                        if (currentLevel.enemies[currentWave].enemyPrefab != "<pause>")
                        {
                            GameObject spawnedEnemy;
                            spawnedEnemy = (GameObject)Instantiate(Resources.Load("Prefabs/" + currentLevel.enemies[currentWave].enemyPrefab), Vector3.zero, Quaternion.identity);  // prefabs need to go to Resources/Prefabs or otherwise everything is terrible
                            spawnedEnemy.GetComponent<Enemy>().SetData(currentLevel.enemies[currentWave]);
                            enemiesInScene.Add(spawnedEnemy);
                        }

                        currentLevel.enemies[currentWave].amount -= 1;
                    } else
                    {
                        waveSpawnedCompletely = true;
                    }
                } catch(IndexOutOfRangeException e)
                {
                    Debug.Log(e);
                }
            } else {
                currentWaveEnemySpawnTimer += Time.deltaTime;
            }
        }
    }

    // will not spawn a new wave unless all enemies are dead
    void SpawnWave() {
        enemiesInScene.Remove(null);
        //Debug.Log(currentLevel.enemies[currentWave].spawnNextImmediatly);
        if(enemiesInScene.Count > 0 && currentLevel.enemies[currentWave].spawnNextImmediatly == "no")
        {
            return;
        }
        
        if (currentLevel.enemies.Length <= currentWave + 1)
        {
            // get next level

            levelNumber += 1;
            try
            {
                string path = "level" + levelNumber.ToString() + ".txt";
                LoadLevelData(path);
            }
            catch
            {
                
            }

            if (levelNumber >= 5)
            {
                SceneManager.LoadScene(2);
            }
        }
        else {
            currentWave += 1;
            currentNewWaveTimer = 0.0f;
            waveSpawnedCompletely = false;
        }
    }

    public void LoadLevelData(string levelName) {
        string path = "LevelData/" + levelName.Replace(".txt", "");  
        string jsonData = Resources.Load<TextAsset>(path).text;
        currentLevel = JsonUtility.FromJson<Level>(jsonData);
        currentWave = 0;
        currentNewWaveTimer = 0.0f;
        currentWaveEnemySpawnTimer = 0.0f;
        waveSpawnedCompletely = false;
    }
}
