using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 1.5f;
    public KeyCode interactKey = KeyCode.E;

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("PlayerInteract: No Inventory found on Player.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (inventory == null)
            return;

        // Find any campfire within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (Collider2D hit in hits)
        {
            Campfire campfire = hit.GetComponent<Campfire>();
            if (campfire != null)
            {
                bool added = campfire.TryAddFuel(inventory);
                if (added)
                {
                    Debug.Log("PlayerInteract: fed the campfire.");
                }
                else
                {
                    Debug.Log("PlayerInteract: no fuel to add.");
                }
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
