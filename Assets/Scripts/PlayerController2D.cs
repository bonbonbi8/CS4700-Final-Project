using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mouseScreenPos;
    private Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        if (moveInput.sqrMagnitude > 1f) moveInput = moveInput.normalized;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        if (cam)
        {
            var mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -cam.transform.position.z));
            var dir = (mouseWorld - transform.position);
            if (dir.sqrMagnitude > 0.0001f)
            {
                float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, ang - 90f); // adjust -90 if your sprite’s forward is +X
            }
        }
    }

    // Called by PlayerInput (Send Messages)
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("Move input: " + moveInput);
    }

    private void OnLook(InputValue value)
    {
        mouseScreenPos = value.Get<Vector2>();
        Debug.Log("Mouse pos: " + mouseScreenPos);
    }

}
