using UnityEngine;
using UnityEngine.UI;

public class DragonStats : MonoBehaviour
{
    [Header("Settings")]
    public string dragonName = "Dragon";
    public float maxHealth = 100f;
    public Slider healthSlider;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;
    public Transform damagePopupPoint;

    private float currentHealth;
    private Animator anim;
    private bool isDead = false;
    private Camera mainCam;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        mainCam = Camera.main;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        ShowDamagePopup(damage);

        if (currentHealth <= 0)
            Die();
    }

    void ShowDamagePopup(float damage)
    {
        if (damagePopupPrefab == null || damagePopupPoint == null)
        {
            Debug.LogWarning("Damage popup prefab or spawn point not assigned!", this);
            return;
        }

        GameObject popup = Instantiate(
            damagePopupPrefab,
            damagePopupPoint.position,
            Quaternion.identity
        );

        if (mainCam != null)
        {
            popup.transform.LookAt(mainCam.transform);
            popup.transform.Rotate(0, 180f, 0);
        }

        DamagePopup popupScript = popup.GetComponent<DamagePopup>();
        if (popupScript != null)
        {
            popupScript.Setup(damage);
        }
        else
        {
            Debug.LogError("DamagePopup script missing on prefab!", popup);
        }
    }

    void Die()
    {
        isDead = true;
        isAttacking = false;
        if (anim != null)
            anim.SetTrigger("Die");

        var gm = Object.FindFirstObjectByType<GameManager>();
        if (gm != null)
            gm.OnDragonDeath(dragonName);

        if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().enabled = false;

        if (GetComponent<AIController>())
            GetComponent<AIController>().enabled = false;
    }
}