using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    [SerializeField] float flapForce = 8f, gravity = 3f, tiltFactor = 6f, maxTilt = 30f, minTilt = -90f;

    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
    }

    void Update()
    {
        var game = GameManager.Instance;
        if ((game.Current == GameManager.State.Ready || game.Current == GameManager.State.Playing) && FlapPressed())
        {
            body.gravityScale = gravity;
            body.linearVelocity = Vector2.up * flapForce;
            game.OnFlap();
        }
        if (game.Current == GameManager.State.Playing || game.Current == GameManager.State.GameOver)
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(body.linearVelocity.y * tiltFactor, minTilt, maxTilt));
    }

    static bool FlapPressed() =>
        (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
        (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

    void OnTriggerEnter2D(Collider2D other) => GameManager.Instance.AddScore();

    void OnCollisionEnter2D(Collision2D collision) => GameManager.Instance.GameOver();
}
