using UnityEngine;

// Objetivo final: trigger que da la victoria si se cumplieron los objetivos.
[RequireComponent(typeof(Collider))]
public class ExitZone : MonoBehaviour
{
    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.CanExit()) GameManager.Instance.Win();
        else HUD.Instance.ShowMessage("Aun no cumpliste todos los objetivos");
    }
}
