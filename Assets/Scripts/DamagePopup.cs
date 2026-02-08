using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float lifeTime = 1.2f;

    private TextMeshProUGUI text;
    private Color startColor;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI missing in DamagePopup prefab!", this);
            return;
        }

        startColor = text.color;
    }

    public void Setup(float damage)
    {
        if (text == null) return;

        text.text = damage.ToString("0");
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (text == null) return;

        float fade = Time.deltaTime / lifeTime;
        text.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            text.color.a - fade
        );
    }
}