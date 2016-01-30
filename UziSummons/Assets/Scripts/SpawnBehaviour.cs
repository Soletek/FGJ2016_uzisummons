using UnityEngine;
using System.Collections;

public class SpawnBehaviour : MonoBehaviour {
    [SerializeField]
    float currentNewWaveTimer = 0.0f;
    [SerializeField]
    float currentWaveEnemySpawnTimer = 0.0f;
    Level currentLevel;
    [SerializeField]
    int currentWave = 0;
    int maxWaves = 0;
    bool waveSpawnedCompletely = false;
    bool enemiesAreDead = false;
    public GameObject player;
    //public GameObject LevelListContainer;

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
                    if (currentLevel.enemies[currentWave].enemyPrefab != "<pause>")
                    {
                        GameObject spawnedEnemy;
                        spawnedEnemy = (GameObject)Instantiate(Resources.Load("Prefabs/" + currentLevel.enemies[currentWave].enemyPrefab), Vector3.zero, Quaternion.identity);  // prefabs need to go to Resources/Prefabs or otherwise everything is terrible
                        spawnedEnemy.GetComponent<Enemy>().SetData(currentLevel.enemies[currentWave]);
                        if (this.player != null)
                        {
                            spawnedEnemy.GetComponent<Enemy>().player = this.player;
                        }
                    }

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
        // if we at maxwaves, load next level when everyone's dead
        if (currentWave < maxWaves) {
            currentWave += 1;
            currentNewWaveTimer = 0.0f;
        } else
        {
            if(enemiesAreDead)
            {
                LoadLevelData("level1.txt");
            }
        }

        
    }

    public void LoadLevelData(string levelName) {
        string path = "LevelData/" + levelName.Replace(".txt", "");  
        string jsonData = Resources.Load<TextAsset>(path).text;
        currentLevel = JsonUtility.FromJson<Level>(jsonData);
        maxWaves = currentLevel.enemies.Length;
        currentNewWaveTimer = 0.0f;
        currentWave = 0;
    }
}
