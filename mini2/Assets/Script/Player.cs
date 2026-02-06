using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 이동
        Vector3 move = new Vector3(moveX, moveY, 0).normalized;
        transform.Translate(move * speed * Time.deltaTime);

        // 애니메이터 전달
        anim.SetFloat("MoveX", moveX);
        anim.SetFloat("MoveY", moveY);
    }
}
