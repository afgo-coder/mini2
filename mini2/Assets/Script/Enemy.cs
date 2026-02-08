using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float repathInterval = 0.1f;
    SpriteRenderer sr;
    NavMeshAgent agent;
    float t;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // 2D 필수 옵션
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (target == null) return;

        t += Time.deltaTime;
        if (t >= repathInterval)
        {
            t = 0f;
            agent.SetDestination(target.position);
        }

        Flip();
    }

    void Flip()
    {
        if (sr == null || agent == null) return;

        if (agent.velocity.x > 0.05f) sr.flipX = true;
        else if (agent.velocity.x < -0.05f) sr.flipX = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("이걸 죽어");
            Destroy(gameObject);
            Destroy(other.gameObject);
        }


    }
}