using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private IInputSource input;
    private IMover movement;
    private ICharacterAnimation charAnimation;

    private void Awake()
    {
        input = new PlayerInputHandler();
        movement = GetComponent<IMover>();
        charAnimation = GetComponent<ICharacterAnimation>();
    }

    private void Update()
    {
        var dir = input.GetMovement();
        movement.Move(dir);
        charAnimation.UpdateAnimation(movement.Velocity);
    }
}