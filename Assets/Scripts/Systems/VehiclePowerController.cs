using System.Collections;
using UnityEngine;

public class VehiclePowerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SpriteRenderer playerVisual;

    [Header("Posiciones")]
    [SerializeField] private float middleY = 0f;
    [SerializeField] private float groundY = -3.5f;
    [SerializeField] private float ceilingY = 3.5f;

    [Header("Invulnerabilidad al terminar")]
    [SerializeField] private float invulnerabilityAfterVehicle = 2f;

    private GameObject currentVehicleVisual;
    private bool isUsingVehicle;
    private Rigidbody2D rb;

    // Guarda las restricciones originales del Rigidbody2D
    // para restaurarlas cuando termine el vehicle.
    private RigidbodyConstraints2D originalConstraints;

    public bool IsUsingVehicle => isUsingVehicle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateVehicle(VehiclePickup.VehicleTier tier, GameObject vehicleVisualPrefab)
    {
        if (isUsingVehicle) return;

        float vehicleSpeed = 35f;
        float distance = 500f;
        float speedReduction = -3f;
        float targetY = middleY;

        bool goingToCeiling = false;

        if (playerController != null)
        {
            goingToCeiling = playerController.IsGravityInverted();
        }

        switch (tier)
        {
            case VehiclePickup.VehicleTier.Gama1:
                vehicleSpeed = 35f;
                distance = 200f;
                speedReduction = -3f;
                targetY = middleY;
                break;

            case VehiclePickup.VehicleTier.Gama2:
                vehicleSpeed = 35f;
                distance = 250f;
                speedReduction = -4f;
                targetY = goingToCeiling ? ceilingY : groundY;
                break;

            case VehiclePickup.VehicleTier.Gama3:
                vehicleSpeed = 40f;
                distance = 300f;
                speedReduction = -5f;
                targetY = goingToCeiling ? ceilingY : groundY;
                break;

            case VehiclePickup.VehicleTier.Gama4:
                vehicleSpeed = 50f;
                distance = 400f;
                speedReduction = -7f;
                targetY = middleY;
                break;
        }

        StartCoroutine(VehicleRoutine(
            tier,
            vehicleSpeed,
            distance,
            speedReduction,
            targetY,
            goingToCeiling,
            vehicleVisualPrefab
        ));
    }

    private IEnumerator VehicleRoutine(
        VehiclePickup.VehicleTier tier,
        float vehicleSpeed,
        float distance,
        float speedReduction,
        float targetY,
        bool goingToCeiling,
        GameObject vehicleVisualPrefab
    )
    {
        isUsingVehicle = true;

        float speedBeforeVehicle = 0f;

        if (playerController != null)
            speedBeforeVehicle = playerController.GetMoveSpeed();

        if (playerHealth != null)
            playerHealth.SetExternalInvulnerable(true);

        if (playerController != null)
            playerController.SetVehicleMode(true, vehicleSpeed);

        if (rb != null)
        {
            originalConstraints = rb.constraints;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.gravityScale = 0f;

            // Congela solo la posición Y para que no suba ni baje,
            // pero permite que siga avanzando en X.
            rb.constraints = RigidbodyConstraints2D.FreezePositionY |
                             RigidbodyConstraints2D.FreezeRotation;
        }

        transform.position = new Vector3(
            transform.position.x,
            targetY,
            transform.position.z
        );

        if (playerVisual != null)
            playerVisual.enabled = false;

        if (vehicleVisualPrefab != null)
        {
            currentVehicleVisual = Instantiate(
                vehicleVisualPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
        }

        if (currentVehicleVisual != null)
        {
            Vector3 vehicleScale = currentVehicleVisual.transform.localScale;

            if (tier == VehiclePickup.VehicleTier.Gama1 ||
                tier == VehiclePickup.VehicleTier.Gama4)
            {
                vehicleScale.y = Mathf.Abs(vehicleScale.y) * Mathf.Sign(transform.localScale.y);
            }
            else if (tier == VehiclePickup.VehicleTier.Gama2 ||
                     tier == VehiclePickup.VehicleTier.Gama3)
            {
                if (goingToCeiling)
                    vehicleScale.y = -Mathf.Abs(vehicleScale.y) * Mathf.Sign(transform.localScale.y);
                else
                    vehicleScale.y = Mathf.Abs(vehicleScale.y) * Mathf.Sign(transform.localScale.y);
            }

            currentVehicleVisual.transform.localScale = vehicleScale;
        }

        float startX = transform.position.x;

        while (transform.position.x < startX + distance)
        {
            yield return null;
        }

        if (currentVehicleVisual != null)
            Destroy(currentVehicleVisual);

        if (playerVisual != null)
            playerVisual.enabled = true;

        transform.position = new Vector3(
            transform.position.x,
            groundY,
            transform.position.z
        );

        if (rb != null)
        {
            rb.constraints = originalConstraints;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        if (playerController != null)
        {
            playerController.SetVehicleMode(false, 0f);
            playerController.ForceNormalGravity();
            playerController.SetSpeedAfterVehicle(speedBeforeVehicle + speedReduction);
        }

        if (playerHealth != null)
        {
            playerHealth.SetExternalInvulnerable(false);
            playerHealth.ActivateInvulnerability(invulnerabilityAfterVehicle);
        }

        isUsingVehicle = false;
    }
}