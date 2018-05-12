using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthCondition : MonoBehaviour
{
    public GameObject slowMotionTrigger;
    public RectTransform healthBar;
    public float currentHealth = 100;
    public float maxHealth = 100f;
    float maxHealthBarLength;

    Player playerScript;


    public float SM_CoolDown;
    float lastSM_Time;


    // Use this for initialization
    void Start()
    {
        playerScript = FindObjectOfType<Player>();
        //slowMotionTrigger = GameObject.FindGameObjectWithTag("SlowMotionTrigger");

        currentHealth = maxHealth;
        maxHealthBarLength = healthBar.rect.width;
        //playerScript.anim.SetBool("Die", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine("SlowMotionRoutine");
        }

        if (playerScript.isDashing)
        {
            slowMotionTrigger.SetActive(true);
        }
        else
        {
            slowMotionTrigger.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "EnemySpawnTrigger")
        {
            playerScript.isInAttackMode = true;
            playerScript.isSwinging = false;
            playerScript.rb.velocity = Vector3.zero;

        }

        if (col.gameObject.tag == "EnergyBall")
        {
            if ((Time.time - lastSM_Time) > SM_CoolDown && playerScript.isDashing)
            {
                lastSM_Time = Time.time;
                StartCoroutine("SlowMotionRoutine");
            }
            else
            {
                Destroy(col.gameObject);
                if (currentHealth > 0)
                {
                    Damaged(GameManager.GHOST_ENERGYBALL_DMG);

                    if (currentHealth <= 0)
                    {
                        print("die");
                        playerScript.anim.SetBool("isDead", true);
                        currentHealth = 0;
                    }
                }
            }
        }
    }

    IEnumerator SlowMotionRoutine()
    {
        SlowMotion();
        yield return new WaitForSeconds(.5f);
        NormalMotion();
    }

    void Damaged(float dmgType)
    {
        currentHealth -= dmgType;
        playerScript.anim.SetTrigger("Damaged");
        healthBar.sizeDelta = new Vector2((currentHealth / maxHealth) * maxHealthBarLength, healthBar.rect.height);
    }

    void SlowMotion()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.001f;
    }

    void NormalMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
