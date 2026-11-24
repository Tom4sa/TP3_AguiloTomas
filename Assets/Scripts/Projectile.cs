using UnityEngine;

// Proyectil fisico: Rigidbody + Collider, interactua por colisiones.
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float explosionForce = 0f;     // opcional: empuja Rigidbodies
    public GameObject impactPrefab;

    float damage;
    bool hit;

    public void Init(float dmg, Vector3 velocity, Transform owner)
    {
        damage = dmg;
        var rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = velocity;   // en Unity 2022 o anterior usar rb.velocity

        // No colisionar con quien disparo
        foreach (var a in GetComponentsInChildren<Collider>())
            foreach (var b in owner.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(a, b);

        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision c)
    {
        if (hit) return;
        hit = true;

        var h = c.collider.GetComponentInParent<Health>();
        if (h != null) h.TakeDamage(damage);

        if (explosionForce > 0f && c.rigidbody != null)
            c.rigidbody.AddForceAtPosition(transform.forward * explosionForce, c.GetContact(0).point, ForceMode.Impulse);

        if (impactPrefab != null)
        {
            var fx = Instantiate(impactPrefab, c.GetContact(0).point, Quaternion.LookRotation(c.GetContact(0).normal));
            Destroy(fx, 2f);
        }
        Destroy(gameObject);
    }
}
