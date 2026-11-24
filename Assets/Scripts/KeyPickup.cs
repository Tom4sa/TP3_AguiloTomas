using UnityEngine;

// Llave recogible: desbloquea puertas con el mismo keyId.
public class KeyPickup : Interactable
{
    public string keyId = "roja";
    public bool countsAsObjective = false;

    protected override void Interact()
    {
        GameManager.Instance.AddKey(keyId);
        if (countsAsObjective) GameManager.Instance.CompleteObjective();
        HUD.Instance.ShowMessage("Conseguiste la llave " + keyId);
        Destroy(gameObject);
    }
}
