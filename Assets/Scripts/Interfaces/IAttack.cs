using UnityEngine; // Ensure this is present

public interface IAttack
{
    void ExecuteAttack(Vector2 direction);
    int Damage { get; }
    float Range { get; }
}
