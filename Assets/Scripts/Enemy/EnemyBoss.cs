using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

[RequireComponent(typeof(RangeEnemyAttack))]
public class EnemyBoss : Enemy
{
    [Header(" Elements ")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    private Animator anim;

    enum State { Non, Idle, Moving, Attack}

    [Header(" State Machine ")]
    private State state;
    private float timer;

    [Header(" Idle State ")]
    [SerializeField] private float maxIdleDuration;
    private float idleDuration;

    [Header(" Moving State ")]
    [SerializeField] private float moveSpeed;
    private Vector2 targetPos;

    [Header(" Attack State ")]
    private int attackCounter;
    private RangeEnemyAttack attack;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        state = State.Non;

        healthBar.transform.parent.gameObject.SetActive(false);

        OnSpawnSequenceCompleted += SpawnCompletedCallback;
        OnDamageTaken += DamageTakenCallback;
    }


    private void OnDestroy()
    {
        OnSpawnSequenceCompleted -= SpawnCompletedCallback;
        OnDamageTaken -= DamageTakenCallback;
    }


    protected override void Start()
    {
        base.Start();

        attack = GetComponent<RangeEnemyAttack>();
    }

    protected override void Update()
    {
        ManageStates();
    }

    private void ManageStates()
    {
        switch (state)
        {
            case State.Idle:
                ManageIdleState();
                break;

            case State.Moving:
                ManageMovingState();
                break;

            case State.Attack:
                ManageAttackState();
                break;

            default:
                break;
        }
    }

    private void SetIdleState()
    {
        state = State.Idle;

        idleDuration = Random.Range(1f, maxIdleDuration);

        anim.Play("Idle");
    }

    private void ManageIdleState()
    {
        timer += Time.deltaTime;

        if(timer >= idleDuration)
        {
            timer = 0;
            StartMovingState();
        }
    }

    private void StartMovingState()
    {
        state = State.Moving;

        targetPos = GetRandomPos();

        anim.Play("Move");
    }

    private void ManageMovingState()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < .01f)
            StartAttackState();
    }

    private void StartAttackState()
    {
        state = State.Attack;

        attackCounter = 0;

        anim.Play("Attack");
    }

    private void ManageAttackState()
    {
        
    }

    //Animation Event
    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.up;
        attack.InstantShoot(direction);
        attackCounter++;
    }

    private Vector2 GetRandomPos()
    {
        Vector2 targetPos = Vector2.zero;

        targetPos.x = Mathf.Clamp(targetPos.x, -Constants.arenaSize.x / 3, Constants.arenaSize.x / 3);
        targetPos.y = Mathf.Clamp(targetPos.y, -Constants.arenaSize.y / 3, Constants.arenaSize.y / 3);

        return targetPos;
    }

    private void SpawnCompletedCallback()
    {
        healthBar.transform.parent.gameObject.SetActive(true);
        UpdateHealthBar();

        SetIdleState();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }

    private void DamageTakenCallback(int damage, Vector2 pos, bool isCritical)
    {
        UpdateHealthBar();
    }

    public override void Die()
    {
        OnBossDeath?.Invoke(transform.position);

        DieAfterWave();
    }
}
