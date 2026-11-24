using System.Collections;
using UnityEngine;

// Arma: disparo (Raycast o proyectil fisico), municion, recarga y feedback.
public class Weapon : MonoBehaviour
{
    public WeaponData data;
    public Camera cam;
    public Transform muzzle;
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public LayerMask hitMask = ~0;

    public int AmmoInMag { get; private set; }
    public int Reserve { get; private set; }
    public bool IsReloading { get; private set; }

    float nextFire;
    Vector3 startLocalPos;

    void Awake()
    {
        AmmoInMag = data.magazineSize;
        Reserve = data.reserveAmmo;
        startLocalPos = transform.localPosition;
    }

    void OnEnable() { IsReloading = false; }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        bool fire = data.automatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
        if (fire) TryShoot();
        if (Input.GetKeyDown(KeyCode.R)) StartReload();

        // Retroceso visual
        transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * 14f);
    }

    void TryShoot()
    {
        if (IsReloading || Time.time < nextFire) return;
        if (AmmoInMag <= 0) { StartReload(); return; }

        AmmoInMag--;
        nextFire = Time.time + 1f / data.fireRate;

        if (data.type == WeaponType.Hitscan) ShootRay();
        else ShootProjectile();

        // Feedback visual y sonoro
        if (muzzleFlash != null) muzzleFlash.Play();
        if (audioSource != null && data.shootClip != null) audioSource.PlayOneShot(data.shootClip);
        transform.localPosition -= Vector3.forward * data.recoilKick;
    }

    void ShootRay()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, data.range, hitMask, QueryTriggerInteraction.Ignore))
        {
            var h = hit.collider.GetComponentInParent<Health>();
            if (h != null) h.TakeDamage(data.damage);

            if (data.impactPrefab != null)
            {
                var fx = Instantiate(data.impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(fx, 2f);
            }
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition(ray.direction * 5f, hit.point, ForceMode.Impulse);
        }
    }

    void ShootProjectile()
    {
        // Apunta al centro de la pantalla
        Vector3 target = cam.transform.position + cam.transform.forward * 100f;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 200f, hitMask, QueryTriggerInteraction.Ignore))
            target = hit.point;

        Vector3 dir = (target - muzzle.position).normalized;
        Projectile p = Instantiate(data.projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
        p.Init(data.damage, dir * data.projectileSpeed, transform.root);
    }

    public void StartReload()
    {
        if (IsReloading || AmmoInMag >= data.magazineSize || Reserve <= 0) return;
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        if (audioSource != null && data.reloadClip != null) audioSource.PlayOneShot(data.reloadClip);
        yield return new WaitForSeconds(data.reloadTime);

        int needed = data.magazineSize - AmmoInMag;
        int taken = Mathf.Min(needed, Reserve);
        AmmoInMag += taken;
        Reserve -= taken;
        IsReloading = false;
    }
}
