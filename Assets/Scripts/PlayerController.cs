using UnityEngine;

// Movimiento en primera persona + camara con el mouse.
[RequireComponent(typeof(CharacterController), typeof(Health))]
public class PlayerController : MonoBehaviour
{
    public PlayerData data;
    public Transform cameraPivot;   // la Camera (hija del jugador)

    CharacterController cc;
    float pitch;
    float verticalVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        GetComponent<Health>().Init(data.maxHealth);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // Mirar
        float mx = Input.GetAxis("Mouse X") * data.mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * data.mouseSensitivity;
        transform.Rotate(0f, mx, 0f);
        pitch = Mathf.Clamp(pitch - my, -85f, 85f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Mover
        Vector3 dir = transform.right * Input.GetAxisRaw("Horizontal")
                    + transform.forward * Input.GetAxisRaw("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? data.runSpeed : data.walkSpeed;

        if (cc.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = Mathf.Sqrt(data.jumpHeight * -2f * data.gravity);
        verticalVelocity += data.gravity * Time.deltaTime;

        cc.Move((dir.normalized * speed + Vector3.up * verticalVelocity) * Time.deltaTime);
    }
}
