using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObjectAtlas enemyScriptableObjectAtlas = null;
    private static List<Enemy> enemyList = new List<Enemy>();

    private int enemyIndex = 1;
    private int counter = 0;
    private int maxEnemyIdex = 1;
    private int multiplierValue = 1;

    private int numberOfEnemiesPerWave = 25;

    private float[] spawnsY = new float[] {0.5f, -0.5f, -1.5f};

    private float timeBtwSpawn = 0;
    private float startTileBtwSpawn = 1.5f;
    private float currentStartTileBtwSpawn = 0;
    private float decreseTime = 0.02f;
    private float minTime = 0.65f;
    private bool inGame = false;

    private void Update()
    {
        if (inGame)
        {
            if (timeBtwSpawn <= 0)
            {
                float enemySpawnY = spawnsY[Random.Range(0, spawnsY.Length)];
                if (UnityEngine.Random.Range(0, 100) < 10) {
                    float potionSpawnY = spawnsY[Random.Range(0, spawnsY.Length)];
                    Potion potion = Potion.Create(new Vector3(transform.position.x, potionSpawnY, transform.position.z), (Potion.PotionType)Random.Range(0, System.Enum.GetValues(typeof(Potion.PotionType)).Length));
                    Debug.Log(potion);
                    while (potionSpawnY == enemySpawnY)
                        enemySpawnY = spawnsY[Random.Range(0, spawnsY.Length)];
                }
                Enemy enemyHandle = Enemy.Create(new Vector3(transform.position.x, enemySpawnY, transform.position.z), enemyScriptableObjectAtlas.GetCharacterAtIndex(Random.Range(enemyIndex - 1, enemyIndex + 2)), multiplierValue);
                enemyList.Add(enemyHandle);
                counter++;
                if(counter >= numberOfEnemiesPerWave)
                {
                    counter = 0;
                    if(enemyIndex >= maxEnemyIdex - 2)
                    {
                        enemyIndex = 1;
                        multiplierValue++;
                    }
                    else
                        enemyIndex++;
                    if (currentStartTileBtwSpawn > minTime)
                        currentStartTileBtwSpawn -= decreseTime;
                }
                timeBtwSpawn = currentStartTileBtwSpawn;
            }
            else
                timeBtwSpawn -= Time.deltaTime;
        }
    }

    public void StartSpawn()
    {
        enemyIndex = 1;
        counter = 0;
        currentStartTileBtwSpawn = startTileBtwSpawn;
        maxEnemyIdex = enemyScriptableObjectAtlas.GetNumberOfEnemies();
        inGame = true;
    }

    public void UnPauseSpawn()
    {
        inGame = true;
    }

    public void StopSpawn()
    {
        inGame = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
