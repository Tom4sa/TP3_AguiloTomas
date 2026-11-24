using UnityEngine;

// ScriptableObject con las variables del jugador (editable desde el Inspector).
[CreateAssetMenu(fileName = "PlayerData", menuName = "Shooter/Player Data")]
public class PlayerData : ScriptableObject
{
    public float maxHealth = 100f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -19.62f;
    public float mouseSensitivity = 2f;
}
