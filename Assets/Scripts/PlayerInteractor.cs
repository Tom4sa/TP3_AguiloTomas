using UnityEngine;

// Detecta objetos interactuables frente a la camara y los activa con E.
public class PlayerInteractor : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        Interactable target = null;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distance, ~0, QueryTriggerInteraction.Collide))
            target = hit.collider.GetComponentInParent<Interactable>();

        if (HUD.Instance != null) HUD.Instance.SetPrompt(target != null ? target.prompt : "");
        if (target != null && Input.GetKeyDown(KeyCode.E)) target.Use();
    }
}
