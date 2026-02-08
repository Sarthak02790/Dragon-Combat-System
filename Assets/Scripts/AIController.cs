using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIController : MonoBehaviour
{
    public Transform player;
    public DragonStats playerStats;

    [Header("Distance Settings")]
    public float attackRange = 10f;
    public float attackCooldown = 3f;

    [Header("Abilities")]
    public GameObject fireParticlePrefab;
    public Transform mouthPoint;

    private NavMeshAgent agent;
    private Animator anim;
    private float nextAttackTime;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetFloat("Speed", 1f);
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", 0f);

            // Turn to face the player while attacking
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }

            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(ChooseAttackRoutine());
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    IEnumerator ChooseAttackRoutine()
    {
        isAttacking = true;
        int choice = Random.Range(0, 3);

        if (choice == 0) yield return StartCoroutine(AIFireAttack());
        else if (choice == 1) yield return StartCoroutine(AITailAttack());
        else yield return StartCoroutine(AIFlyAttack());

        isAttacking = false;
    }

    IEnumerator AIFireAttack()
    {
        anim.SetTrigger("AttackFire");
        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySound(SoundManager.instance.fireBreath);
        if (fireParticlePrefab && mouthPoint)
        {
            GameObject fire = Instantiate(fireParticlePrefab, mouthPoint.position, mouthPoint.rotation);
            Destroy(fire, 1f);
        }

        // Hit distance for fire is long
        if (Vector3.Distance(transform.position, player.position) < 12f)
            playerStats.TakeDamage(10);

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator AITailAttack()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.tailSlam);
        anim.SetTrigger("AttackTail");
        yield return new WaitForSeconds(0.5f);

        // Hit distance for tail
        if (Vector3.Distance(transform.position, player.position) < 10f)
            playerStats.TakeDamage(12);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator AIFlyAttack()
    {
        
        anim.SetTrigger("AttackFly");

        // Disable agent so it doesn't fight the vertical movement
        agent.enabled = false;

        float startY = transform.position.y;
        float flyHeight = 6f;

        float timer = 0;
        while (timer < 0.6f)
        {
            transform.position += Vector3.up * (flyHeight * Time.deltaTime / 0.6f);
            timer += Time.deltaTime;
            yield return null;
        }

        SoundManager.instance.PlaySound(SoundManager.instance.flyLaunch);

        // Fire from the sky!
        if (fireParticlePrefab && mouthPoint)
        {
            GameObject fire = Instantiate(fireParticlePrefab, mouthPoint.position, mouthPoint.rotation);
            fire.transform.parent = mouthPoint;
            Destroy(fire, 1f);
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0;
        while (timer < 0.3f)
        {
            transform.position -= Vector3.up * (flyHeight * Time.deltaTime / 0.3f);
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        agent.enabled = true;

        if (Vector3.Distance(transform.position, player.position) < 10f)
            playerStats.TakeDamage(20);
    }
}