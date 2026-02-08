using UnityEngine;

public class Chest : MonoBehaviour
{
    StageManager stageManager;

    void Awake()
    {
        stageManager = FindAnyObjectByType<StageManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("≈‰≥¢Ωß ªÛ¿⁄∏‘¿Ω");
        Destroy(gameObject);
        stageManager.OnChestCollected();
    }
}

