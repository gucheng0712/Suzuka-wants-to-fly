using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MentalState
{
    IDLE,
    PATROL,
    ATTACK,
    CHASE,
    DIE
}
public class Enemy : MonoBehaviour
{

    GameManager_Audio audioManager;

    public RectTransform healthBar;
    public float maxHealth = 100f;
    public float currentHealth;
    float maxHealthBarLength;


    Animator anim;
    Transform firePoint;
    GameObject energyBall;
    string energyBallPath = "Prefabs/EnergyBall";
    EnemySpawner enemySpawner;
    Transform player;
    Rigidbody rb;

    Transform building;
    Light spotLight;
    Vector3[] waypoints;
    public float viewDistance = 8;
    float viewAngle;
    float moveSpeed = 3f;
    float waitTime = 2f;
    int targetWaypointIndex;
    bool isAttacking;

    float lastAttackTime;
    float attackCooldown = 4;


    MentalState mentalState;

    Player playerScript;

    void Start()
    {
        audioManager = FindObjectOfType<GameManager_Audio>();
        anim = GetComponentInChildren<Animator>();
        spotLight = GetComponentInChildren<Light>();
        viewAngle = spotLight.spotAngle;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        energyBall = (GameObject)Resources.Load(energyBallPath, typeof(GameObject));
        firePoint = transform.Find("FirePoint");

        currentHealth = maxHealth;
        maxHealthBarLength = healthBar.rect.width;

        audioManager.ChangeBGM(audioManager.battle_bgm);
    }
    void Update()
    {
        //   print(mentalState);

        if (Input.GetKey(KeyCode.P))
        {
            mentalState = MentalState.DIE;
        }

        switch (mentalState)
        {
            case MentalState.CHASE:
                Chase();
                ChaseOrAttack();
                anim.SetBool("IsMoving", true);
                break;
            case MentalState.ATTACK:
                Attack();
                ChaseOrAttack();
                anim.SetBool("IsMoving", false);
                break;
            case MentalState.PATROL:
                Partol(waypoints);
                ChaseOrAttack();
                anim.SetBool("IsMoving", true);
                break;
            case MentalState.DIE:
                Die();
                break;
            case MentalState.IDLE:
                StartCoroutine("TransitionToPartol");
                anim.SetBool("IsMoving", false);
                ChaseOrAttack();
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            building = collision.gameObject.transform;
            Transform pathHolder = building.Find("PathHolder");
            waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
            }
            rb.isKinematic = true;
            mentalState = MentalState.IDLE;
        }
    }

    IEnumerator TransitionToPartol()
    {
        yield return new WaitForSeconds(waitTime);
        if (mentalState == MentalState.IDLE)
            mentalState = MentalState.PATROL;
    }

    void Partol(Vector3[] _waypoints)
    {
        spotLight.enabled = true;
        spotLight.color = Color.yellow;

        transform.LookAt(_waypoints[targetWaypointIndex]);
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[targetWaypointIndex], moveSpeed * Time.deltaTime);
        if (transform.position == _waypoints[targetWaypointIndex])
        {
            targetWaypointIndex = (targetWaypointIndex + 1) % _waypoints.Length;
            mentalState = MentalState.IDLE;
            StartCoroutine("TransitionToPartol");
        }
    }

    void ChaseOrAttack()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (!Physics.Linecast(transform.position + Vector3.up * 0.5f, player.position + Vector3.up * 0.5f, LayerMask.GetMask("Groundable")))
            {
                if (angle < viewAngle / 2)
                {
                    Debug.DrawLine(transform.position + Vector3.up * 0.5f, player.position, Color.blue);
                    mentalState = (Vector3.Distance(transform.position, player.position) >= viewDistance / 2) ? MentalState.CHASE : MentalState.ATTACK;
                }
            }
            else
            {
                mentalState = MentalState.PATROL;
            }

        }
        else if (mentalState != MentalState.IDLE && Vector3.Distance(transform.position, player.position) >= viewDistance * 1.5f)
        {
            mentalState = MentalState.PATROL;
        }
    }

    void LookAtTarget()
    {
        Vector3 lookDir = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, lookDir, 0.5f));
    }

    void Chase()
    {
        spotLight.enabled = true;
        spotLight.color = Color.red;
        LookAtTarget();
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        spotLight.enabled = false;
        LookAtTarget();
        if ((Time.time - lastAttackTime) > attackCooldown)
        {
            attackCooldown = Random.Range(2, 7);
            lastAttackTime = Time.time;
            GameObject newEnergyBall = Instantiate(energyBall, firePoint.position, transform.rotation);
            Destroy(newEnergyBall, 5);
        }
    }

    void Die()
    {
        StartCoroutine("EnemyRespawnDelay");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    IEnumerator EnemyRespawnDelay()
    {
        spotLight.enabled = false;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(5);
        building.Find("EnemySpawnTrigger").gameObject.SetActive(true);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "BladeCollider")
        {
            Damaged(GameManager.PLAYER_HIDDENBLADE_DMG);
        }
        if (col.gameObject.tag == "BulletCollider")
        {
            Damaged(GameManager.PLAYER_Bullet_DM);
        }
        if (currentHealth <= 0)
        {
            mentalState = MentalState.DIE;
            audioManager.ChangeBGM(audioManager.noraml_bgm);
            FindObjectOfType<Player>().isInAttackMode = false;
        }
    }

    void Damaged(float dmgType)
    {
        currentHealth -= dmgType;
        anim.SetTrigger("Damaged");
        healthBar.sizeDelta = new Vector2((currentHealth / maxHealth) * maxHealthBarLength, healthBar.rect.height);
    }
}
