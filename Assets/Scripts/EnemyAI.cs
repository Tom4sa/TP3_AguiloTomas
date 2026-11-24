using UnityEngine;
using UnityEngine.AI;

// Enemigo: patrulla entre puntos, detecta al jugador, lo persigue y ataca.
[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    public EnemyData data;
    public Transform[] patrolPoints;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip deathClip;

    NavMeshAgent agent;
    Health health;
    Transform player;
    Health playerHealth;
    int patrolIndex;
    float nextAttack;
    float lastSeenTime = -999f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.Init(data.maxHealth);
        health.OnDamaged += _ => lastSeenTime = Time.time;   // si lo disparan, se alerta
        health.OnDeath += Die;

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) { player = p.transform; playerHealth = p.GetComponent<Health>(); }

        if (GameManager.Instance != null) GameManager.Instance.RegisterEnemy();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (health.IsDead || player == null || playerHealth.IsDead) return;

        bool sees = CanSeePlayer();
        if (sees) lastSeenTime = Time.time;
        bool tracking = Time.time - lastSeenTime < data.loseSightTime;
        float dist = Vector3.Distance(transform.position, player.position);

        if (tracking && sees && dist <= data.attackRange)
        {
            // ATACAR
            agent.isStopped = true;
            FaceTarget(player.position);
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + data.attackCooldown;
                if (audioSource != null && attackClip != null) audioSource.PlayOneShot(attackClip);
                if (Random.value <= data.accuracy) playerHealth.TakeDamage(data.damage);
            }
        }
        else if (tracking)
        {
            // PERSEGUIR
            agent.isStopped = false;
            agent.speed = data.chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // PATRULLAR
            agent.isStopped = false;
            agent.speed = data.patrolSpeed;
            if (patrolPoints != null && patrolPoints.Length > 0 &&
                !agent.pathPending && agent.remainingDistance < 0.6f)
                GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    bool CanSeePlayer()
    {
        Vector3 eyes = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.2f;
        Vector3 toPlayer = target - eyes;
        if (toPlayer.magnitude > data.sightRange) return false;
        if (Vector3.Angle(transform.forward, toPlayer) > data.fieldOfView * 0.5f) return false;

        if (Physics.Raycast(eyes, toPlayer.normalized, out RaycastHit hit, data.sightRange, ~0, QueryTriggerInteraction.Ignore))
            return hit.transform == player || hit.transform.IsChildOf(player);
        return false;
    }

    void FaceTarget(Vector3 pos)
    {
        Vector3 d = pos - transform.position; d.y = 0f;
        if (d.sqrMagnitude < 0.01f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(d), Time.deltaTime * 8f);
    }

    void Die()
    {
        agent.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
        if (audioSource != null && deathClip != null) AudioSource.PlayClipAtPoint(deathClip, transform.position);
        if (GameManager.Instance != null) GameManager.Instance.EnemyDied();
        transform.Rotate(-90f, 0f, 0f);   // "cae" al piso
        Destroy(gameObject, 2f);
    }
}
