using UnityEngine;

// Interruptor: abre puertas y cuenta como objetivo intermedio.
public class Switch : Interactable
{
    public Door[] doors;
    public bool countsAsObjective = true;
    bool used;

    protected override void Interact()
    {
        if (used) return;
        used = true;
        prompt = "";
        foreach (var d in doors) d.Open();
        if (countsAsObjective) GameManager.Instance.CompleteObjective();
        HUD.Instance.ShowMessage("Mecanismo activado");
    }
}
