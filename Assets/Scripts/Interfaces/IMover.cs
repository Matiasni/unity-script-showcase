using UnityEngine;

public interface IMover
{
    void Move(Vector2 direction);
    Vector2 Velocity { get; }
}