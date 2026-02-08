using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 720f;
    public float gravity = 9.81f;

    [Header("Abilities")]
    public GameObject fireParticlePrefab;
    public Transform mouthPoint;
    public DragonStats enemyStats;

    [Header("Cooldown Settings")]
    public float globalAttackDelay = 1.0f;

    [Header("UI Reference")]
    public UnityEngine.UI.Button[] attackButtons;

    private CharacterController controller;
    private Animator anim;
    private bool isPerformingFly = false;
    private bool canAttack = true;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPerformingFly) return;

        HandleGravity();
        HandleMovement();
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
            verticalVelocity = -0.5f;
        else
            verticalVelocity -= gravity * Time.deltaTime;

        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            controller.Move(direction * moveSpeed * Time.deltaTime);
            anim.SetFloat("Speed", 1f);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    public void OnFireButtonPressed()
    {
        if (canAttack && !isPerformingFly)
        {
            StartCoroutine(FireAttackRoutine());
        }
    }

    public void OnTailButtonPressed()
    {
        if (canAttack && !isPerformingFly)
        {
            StartCoroutine(TailAttackRoutine());
        }
    }

    public void OnFlyButtonPressed()
    {
        if (canAttack && !isPerformingFly)
        {
            StartCoroutine(FlyAttackRoutine());
        }
    }

    IEnumerator FireAttackRoutine()
    {
        SetButtonsInteractable(false);
        canAttack = false;
        anim.SetTrigger("AttackFire");
        yield return new WaitForSeconds(0.4f);
        SoundManager.instance.PlaySound(SoundManager.instance.fireBreath);
        if (fireParticlePrefab && mouthPoint)
        {
            GameObject fire = Instantiate(fireParticlePrefab, mouthPoint.position, mouthPoint.rotation);
            Destroy(fire, 1f);
        }

        CheckDamage(10f, 10, "Fire");

        yield return new WaitForSeconds(globalAttackDelay);
        canAttack = true;
        SetButtonsInteractable(true);
    }

    IEnumerator TailAttackRoutine()
    {
        SetButtonsInteractable(false);
        canAttack = false;
        anim.SetTrigger("AttackTail");
        SoundManager.instance.PlaySound(SoundManager.instance.tailSlam);
        yield return new WaitForSeconds(0.5f);

        
        CheckDamage(10f, 15, "Tail");

        yield return new WaitForSeconds(globalAttackDelay);
        canAttack = true;
        SetButtonsInteractable(true);
    }

    IEnumerator FlyAttackRoutine()
    {
        SetButtonsInteractable(false);
        canAttack = false;
        isPerformingFly = true;
        anim.SetTrigger("AttackFly");

        float startY = transform.position.y;
        float flyHeight = 6f;

        // Ascent (Dragon jumps up)
        float timer = 0;
        while (timer < 0.6f)
        {
            transform.position += Vector3.up * (flyHeight * Time.deltaTime / 0.6f);
            timer += Time.deltaTime;
            yield return null;
        }


        SoundManager.instance.PlaySound(SoundManager.instance.flyLaunch);

        // Spawn fire while at the peak
        if (fireParticlePrefab && mouthPoint)
        {
            GameObject fire = Instantiate(fireParticlePrefab, mouthPoint.position, mouthPoint.rotation);

            // Use SetParent for better stability during movement
            fire.transform.SetParent(mouthPoint);

            // Reset local transform to ensure it doesn't spawn "downside"
            fire.transform.localPosition = Vector3.zero;
            fire.transform.localRotation = Quaternion.identity;

            Destroy(fire, 1f);
        }

        // Wait a little longer while breathing fire before coming down
        yield return new WaitForSeconds(0.3f);

        // Descent (Dragon comes back down)
        timer = 0;
        while (timer < 0.3f)
        {
            transform.position -= Vector3.up * (flyHeight * Time.deltaTime / 0.3f);
            timer += Time.deltaTime;
            yield return null;
        }

        // Landing & Reset
        Vector3 finalPos = transform.position;
        finalPos.y = startY;
        transform.position = finalPos;
        verticalVelocity = 0;
        controller.Move(Vector3.zero);

        CheckDamage(10f, 25, "Fly Landing");

        isPerformingFly = false;
        yield return new WaitForSeconds(globalAttackDelay);
        canAttack = true;
        SetButtonsInteractable(true);
    }

    void SetButtonsInteractable(bool state)
    {
        foreach (var btn in attackButtons)
        {
            if (btn != null) btn.interactable = state;
        }
    }

    // Handle damage and debugging
    void CheckDamage(float range, float damage, string attackName)
    {
        if (enemyStats != null)
        {
            float dist = Vector3.Distance(transform.position, enemyStats.transform.position);
            if (dist < range)
            {
                enemyStats.TakeDamage(damage);
                Debug.Log(attackName + " HIT! Distance: " + dist);
            }
            else
            {
                Debug.Log(attackName + " MISSED. Distance: " + dist + " (Needs to be < " + range + ")");
            }
        }
    }
}