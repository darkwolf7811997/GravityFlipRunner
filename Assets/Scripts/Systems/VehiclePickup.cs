using UnityEngine;

public class VehiclePickup : MonoBehaviour
{
    public enum VehicleTier
    {
        Gama1,
        Gama2,
        Gama3,
        Gama4
    }

    [SerializeField] private VehicleTier tier;
    [SerializeField] private GameObject vehicleVisualPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        VehiclePowerController controller = collision.GetComponent<VehiclePowerController>();

        if (controller == null) return;

        controller.ActivateVehicle(tier, vehicleVisualPrefab);
        Destroy(gameObject);
    }
}