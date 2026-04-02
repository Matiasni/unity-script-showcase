using UnityEngine;

public class CharacterMovementVisuals : MonoBehaviour, ICharacterAnimation
{
    [SerializeField] private Animator animator;

    public void UpdateAnimation(Vector2 velocity)
    {
        animator.SetFloat("MoveX", velocity.x);
        animator.SetFloat("MoveY", velocity.y);
    }
}