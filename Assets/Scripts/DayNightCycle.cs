using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Simple day/night cycle:
/// - time01 goes from 0..1 for a full day
/// - Uses a UI Image as a dark overlay to simulate night
/// - Exposes IsNight for other systems
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("How long (in real minutes) one full in-game day lasts.")]
    public float dayLengthInMinutes = 10f;

    [Tooltip("Normalized time of day, 0..1 (0 = sunrise, 0.5 = sunset, etc.)")]
    [Range(0f, 1f)]
    public float time01 = 0.25f; // start around morning

    [Header("Visuals")]
    [Tooltip("Full-screen UI Image used as a darkness overlay.")]
    public Image darknessOverlay;

    [Tooltip("Color at full night (alpha controls darkness).")]
    public Color nightOverlayColor = new Color(0f, 0f, 0f, 0.7f);

    [Tooltip("Color at full day (usually transparent).")]
    public Color dayOverlayColor = new Color(0f, 0f, 0f, 0f);

    [Header("Debug (optional)")]
    public TMP_Text clockText;

    public bool IsNight { get; private set; }

    void Update()
    {
        if (dayLengthInMinutes <= 0f)
            dayLengthInMinutes = 0.1f;

        // Advance time
        float daySeconds = dayLengthInMinutes * 60f;
        time01 += Time.deltaTime / daySeconds;

        if (time01 > 1f)
            time01 -= 1f;

        // Light level curve (bright at midday, dark at night)
        float angle = time01 * Mathf.PI * 2f;
        float light01 = Mathf.Clamp01(Mathf.Sin(angle) * 0.5f + 0.5f);

        // Decide if it's night (roughly evening to early morning)
        IsNight = (time01 <= 0.25f || time01 >= 0.75f);

        // Apply overlay
        if (darknessOverlay != null)
        {
            Color c = Color.Lerp(nightOverlayColor, dayOverlayColor, light01);
            darknessOverlay.color = c;
        }

        // Optional clock debug
        if (clockText != null)
        {
            float hours = time01 * 24f;
            int h = Mathf.FloorToInt(hours);
            int m = Mathf.FloorToInt((hours - h) * 60f);
            clockText.text = $"{h:00}:{m:00}";
        }
    }
}
