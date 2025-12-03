using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Header("Required Item To Win")]
    public ItemData requiredItem;
    public int requiredQuantity = 1;

    [Header("Prompt")]
    public string debugMessageIfMissing = "You feel like you are missing something important...";
    public string debugMessageIfWin = "You activate the device and escape!";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory == null)
            return;

        if (requiredItem == null)
        {
            Debug.LogWarning("WinTrigger: No requiredItem set. Triggering win anyway.");
            TriggerWin();
            return;
        }

        int count = inventory.GetItemCount(requiredItem);
        if (count >= requiredQuantity)
        {
            Debug.Log(debugMessageIfWin);
            TriggerWin();
        }
        else
        {
            Debug.Log(debugMessageIfMissing);
        }
    }

    void TriggerWin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerWin();
        }
        else
        {
            Debug.LogWarning("WinTrigger: No GameManager instance found, cannot show win screen.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
