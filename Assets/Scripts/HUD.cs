using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Interfaz: vida, municion, arma actual, objetivo, mensajes y flash de dano.
public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    public Health player;
    public WeaponManager weapons;

    public Slider healthBar;
    public TMP_Text healthText;
    public TMP_Text ammoText;
    public TMP_Text weaponText;
    public TMP_Text objectiveText;
    public TMP_Text promptText;
    public TMP_Text messageText;
    public Image damageFlash;
    public AudioSource uiAudio;
    public AudioClip hurtClip;

    float messageTimer;

    void Awake() { Instance = this; }

    void Start()
    {
        player.OnDamaged += OnPlayerDamaged;
        if (promptText != null) promptText.text = "";
        if (messageText != null) messageText.text = "";
    }

    void Update()
    {
        healthBar.maxValue = player.max;
        healthBar.value = player.Current;
        healthText.text = Mathf.CeilToInt(player.Current) + " / " + Mathf.CeilToInt(player.max);

        var w = weapons.Current;
        weaponText.text = w.data.weaponName;
        ammoText.text = w.IsReloading ? "Recargando..." : w.AmmoInMag + " / " + w.Reserve;

        if (GameManager.Instance != null) objectiveText.text = GameManager.Instance.ObjectiveText();

        // Flash de dano que se desvanece
        if (damageFlash != null)
        {
            var c = damageFlash.color;
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime * 1.5f);
            damageFlash.color = c;
        }

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f) messageText.text = "";
        }
    }

    void OnPlayerDamaged(float amount)
    {
        if (damageFlash != null) { var c = damageFlash.color; c.a = 0.45f; damageFlash.color = c; }
        if (uiAudio != null && hurtClip != null) uiAudio.PlayOneShot(hurtClip);
    }

    public void SetPrompt(string text) { if (promptText != null) promptText.text = text; }

    public void ShowMessage(string text, float seconds = 2.5f)
    {
        messageText.text = text;
        messageTimer = seconds;
    }
}
