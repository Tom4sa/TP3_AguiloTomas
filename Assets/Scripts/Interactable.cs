using UnityEngine;

// Base para todo lo que el jugador activa con la tecla E.
public abstract class Interactable : MonoBehaviour
{
    public string prompt = "Presiona E";
    public AudioClip interactClip;

    public void Use()
    {
        if (interactClip != null) AudioSource.PlayClipAtPoint(interactClip, transform.position);
        Interact();
    }

    protected abstract void Interact();
}
