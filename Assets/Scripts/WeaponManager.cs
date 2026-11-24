using UnityEngine;

// Cambio de armas con teclas 1..9 y rueda del mouse.
public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    int index;

    public Weapon Current => weapons[index];

    void Start() { Select(0); }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        for (int i = 0; i < weapons.Length && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Select(i);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) Select((index + 1) % weapons.Length);
        else if (scroll < 0f) Select((index - 1 + weapons.Length) % weapons.Length);
    }

    void Select(int i)
    {
        index = i;
        for (int k = 0; k < weapons.Length; k++)
            weapons[k].gameObject.SetActive(k == index);
    }
}
