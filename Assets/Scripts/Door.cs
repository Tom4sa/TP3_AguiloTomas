using UnityEngine;

// Puerta que se desliza. Puede requerir llave o abrirse solo desde un Switch.
public class Door : Interactable
{
    public Vector3 openOffset = new Vector3(0, 3.5f, 0);
    public float speed = 2f;
    public bool requiresKey;
    public string keyId = "roja";
    public bool onlyBySwitch;

    Vector3 closedPos, openPos;
    bool isOpen;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    protected override void Interact()
    {
        if (isOpen) return;
        if (onlyBySwitch) { HUD.Instance.ShowMessage("Esta puerta se abre desde otro lugar"); return; }
        if (requiresKey && !GameManager.Instance.HasKey(keyId))
        {
            HUD.Instance.ShowMessage("Necesitas la llave " + keyId);
            return;
        }
        Open();
    }

    public void Open() { isOpen = true; prompt = ""; }
}
