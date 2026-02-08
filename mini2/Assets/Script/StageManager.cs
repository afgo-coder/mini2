using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class StageManager : MonoBehaviour
{
    [Header("Refs")]
    public ScreenFader fader;
    public Transform player;

    [Header("Prefabs")]
    public GameObject chestPrefab;
    public GameObject enemyPrefab;

    [Header("Spawn")]
    public int startChestCount = 1;
    int remainingChests;
    public int startEnemyCount = 1;
    public float spawnCheckRadius = 0.25f;
    public LayerMask blockedMask; 
    public Bounds spawnBounds;

    [Header("UI")]
    public TMP_Text stageText;
    public TMP_Text chestText;

    [Header("Stage Rule")]
    public int maxStage = 5;

    [Header("NavMesh")]
    public NavMeshSurface surface; 

    [Header("Player Reset")]
    public Vector2 playerStartPos = new Vector2(-2.17f, -3.93f);


    int stage = 1;
    int chestCount;
    int enemyCount;

    readonly List<GameObject> spawned = new();

    void Start()
    {
        chestCount = startChestCount;
        enemyCount = startEnemyCount;

        StartStage();
    }

    void StartStage()
    {
        ClearSpawned();

        if (player != null)
        {
            player.position = new Vector3(playerStartPos.x, playerStartPos.y, player.position.z);
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
        remainingChests = chestCount;
        UpdateUI();
        SpawnMany(chestPrefab, chestCount);
        SpawnMany(enemyPrefab, enemyCount);

        
        if (surface != null) surface.BuildNavMesh();
    }
    public void OnChestCollected()
    {
        remainingChests--;
        UpdateUI();
        if (remainingChests <= 0)
        {
            StartCoroutine(NextStageRoutine());
        }
    }

    IEnumerator NextStageRoutine()
    {
        bool isFinalClear = (stage >= maxStage);

        if (fader != null)
            yield return fader.PlayStageClearSequence(isFinalClear ? "Game Clear!" : "Stage Clear!");

        if (isFinalClear)
        {
            
            Time.timeScale = 0f;
            yield break;
        }

        stage++;
        chestCount += 1;
        enemyCount += 1;

        StartStage();
    }

    void SpawnMany(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (TryGetFreePos(out Vector2 pos))
            {
                var go = Instantiate(prefab, pos, Quaternion.identity);
                var chaser = go.GetComponent<Enemy>();
                if (chaser != null)
                    chaser.target = player;   
                spawned.Add(go);
            }
            else
            {
                Debug.LogWarning("X");
            }
        }
    }

    void UpdateUI()
    {
        if (stageText != null) stageText.text = $"STAGE: {stage}";
        if (chestText != null) chestText.text = $"Chest: {remainingChests}";
    }
    bool TryGetFreePos(out Vector2 pos)
    {
        
        for (int i = 0; i < 200; i++)
        {
            float x = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
            float y = Random.Range(spawnBounds.min.y, spawnBounds.max.y);
            Vector2 p = new Vector2(x, y);

            
            if (!Physics2D.OverlapCircle(p, spawnCheckRadius, blockedMask))
            {
                pos = p;
                return true;
            }
        }
        pos = default;
        return false;
    }

    void ClearSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null) Destroy(spawned[i]);
        }
        spawned.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnBounds.center, spawnBounds.size);
    }
}
