using UnityEngine;

public enum WeaponType { Hitscan, Projectile }

// ScriptableObject con las variables de cada arma.
[CreateAssetMenu(fileName = "WeaponData", menuName = "Shooter/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Pistola";
    public WeaponType type = WeaponType.Hitscan;
    public bool automatic = false;

    [Header("Stats")]
    public float damage = 20f;
    public float fireRate = 4f;          // disparos por segundo
    public float range = 100f;           // solo hitscan
    public int magazineSize = 12;
    public int reserveAmmo = 48;
    public float reloadTime = 1.5f;

    [Header("Proyectil (solo tipo Projectile)")]
    public Projectile projectilePrefab;
    public float projectileSpeed = 30f;

    [Header("Feedback")]
    public AudioClip shootClip;
    public AudioClip reloadClip;
    public GameObject impactPrefab;      // particulas al impactar
    public float recoilKick = 0.05f;
}
