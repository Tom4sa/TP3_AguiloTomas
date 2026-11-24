using UnityEngine;

// ScriptableObject con las variables de cada tipo de enemigo.
[CreateAssetMenu(fileName = "EnemyData", menuName = "Shooter/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float maxHealth = 50f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float sightRange = 18f;
    [Range(30, 360)] public float fieldOfView = 110f;
    public float attackRange = 10f;
    public float damage = 8f;
    public float attackCooldown = 1.2f;
    [Range(0f, 1f)] public float accuracy = 0.6f;
    public float loseSightTime = 4f;
}
