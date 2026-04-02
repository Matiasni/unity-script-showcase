using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, IMover
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;

    public Vector2 Velocity => rb.linearVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        currentVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = currentVelocity;
    }
}