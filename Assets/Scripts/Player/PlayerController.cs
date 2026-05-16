using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Gravedad")]
    [SerializeField] private float gravityStrength = 4f;

    [Header("Detección")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float checkOffset = 0.05f;

    [Header("Límites")]
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    [Header("Velocidad progresiva")]
    [SerializeField] private float baseMoveSpeed = 6f;
    [SerializeField] private float speedIncreaseEveryMeters = 100f;
    [SerializeField] private float speedIncreaseAmount = 0.2f;
    [SerializeField] private float maxMoveSpeed = 20f;

    [Header("Modificadores de velocidad por zonas")]
    [SerializeField] private float zoneSpeedModifier = 0f;
    [SerializeField] private float minZoneSpeedModifier = -3f;
    [SerializeField] private float maxZoneSpeedModifier = 5f;

    private float startX;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isGrounded;
    private bool gravityInverted = false;

    private bool isVehicleMode;
    private float vehicleSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ApplyGravityState();
        startX = transform.position.x;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        CheckGrounded();
        HandleInput();
        CheckVerticalBounds();
        UpdateProgressiveSpeed();
    }

    private void FixedUpdate()
    {
        ApplyAutoRun();
    }

    private void HandleInput()
    {
        bool inputPressed = false;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            inputPressed = true;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            inputPressed = true;
        }

        if (isVehicleMode)
            return;

        if (inputPressed && isGrounded)
        {
            FlipGravity();
        }
    }

    private void ApplyAutoRun()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    private void FlipGravity()
    {
        gravityInverted = !gravityInverted;
        isGrounded = false;
        ApplyGravityState();
    }

    private void ApplyGravityState()
    {
        rb.gravityScale = gravityInverted ? -gravityStrength : gravityStrength;

        Vector3 scale = transform.localScale;
        scale.y = gravityInverted ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    private void CheckGrounded()
    {
        if (playerCollider == null)
        {
            isGrounded = false;
            return;
        }

        Bounds bounds = playerCollider.bounds;
        Vector2 checkPosition;

        if (!gravityInverted)
            checkPosition = new Vector2(bounds.center.x, bounds.min.y - checkOffset);
        else
            checkPosition = new Vector2(bounds.center.x, bounds.max.y + checkOffset);

        isGrounded = Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
    }

    private void CheckVerticalBounds()
    {
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player murió");
    }

    private void UpdateProgressiveSpeed()
    {
        if (isVehicleMode)
        {
            moveSpeed = vehicleSpeed;
            return;
        }

        float distanceTravelled = transform.position.x - startX;

        int increases = Mathf.FloorToInt(distanceTravelled / speedIncreaseEveryMeters);
        float progressiveSpeed = baseMoveSpeed + (increases * speedIncreaseAmount);
        float finalSpeed = progressiveSpeed + zoneSpeedModifier;

        moveSpeed = Mathf.Clamp(finalSpeed, baseMoveSpeed, maxMoveSpeed);
    }

    public void AddZoneSpeedModifier(float amount)
    {
        zoneSpeedModifier += amount;
        zoneSpeedModifier = Mathf.Clamp(zoneSpeedModifier, minZoneSpeedModifier, maxZoneSpeedModifier);
    }

    public void SetVehicleMode(bool active, float speed)
    {
        isVehicleMode = active;
        vehicleSpeed = speed;
    }

    public bool IsVehicleMode()
    {
        return isVehicleMode;
    }

    public void ForceNormalGravity()
    {
        gravityInverted = false;
        ApplyGravityState();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeedAfterVehicle(float targetSpeed)
    {
        targetSpeed = Mathf.Clamp(targetSpeed, baseMoveSpeed, maxMoveSpeed);

        moveSpeed = targetSpeed;

        // Reinicia la progresión desde esta nueva velocidad.
        startX = transform.position.x;
        baseMoveSpeed = targetSpeed;

        // Limpia modificadores para evitar valores raros acumulados.
        zoneSpeedModifier = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Bounds bounds = col.bounds;
        Vector2 checkPosition;

        bool invertedPreview = Application.isPlaying && gravityInverted;

        if (!invertedPreview)
            checkPosition = new Vector2(bounds.center.x, bounds.min.y - checkOffset);
        else
            checkPosition = new Vector2(bounds.center.x, bounds.max.y + checkOffset);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }

    public bool IsGravityInverted()
    {
        return gravityInverted;
    }
}