using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Health
{
    [Header("Player: Stats")]
    public float speed = 10;
    public float homingAttackRange = 10;
    public float homingAttackSpeed = 50;
    public float jumpStrength = 5;
    public float stompSpeed = 75;
    public int coins = 0;
    public float midAirSpeedMultiplier = 0.3f;
    public float groundCheckRadius = 0.1f;
    [Header("Player: Floor detection")]
    public Transform groundCheck;
    public LayerMask groundMask;
    [Header("Player: Graphics and stuff")]
    public SpriteRenderer sr;
    public SpriteRenderer ball;
    public GameObject stompEffect;
    public TrailRenderer trail;
    [Header("Player: Other stuff")]
    public Vector2 respawnPos;
    public LayerMask homingAttackMask;
    public Collider2D normalCollider;
    public Collider2D spinCollider;
    public PlayerCamera cam;
    [HideInInspector] public int checkpoint = -1;
    bool isGrounded = false;
    [HideInInspector] public bool inputEnabled = true;
    Rigidbody2D rb;
    public bool stomp = false;
    public bool spring = false;
    public bool jump = false;
    public bool spinning = false;
    public float timer = 0;
    [HideInInspector] public float movement = 0;
    [HideInInspector] public float vertical = 0;
    [HideInInspector] public bool doJump = false;
    Transform curTarget;
    bool homingAttack = false;
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        trail.startColor = sr.color * trail.startColor;
        trail.endColor = sr.color * trail.endColor;
        trail.emitting = false;
    }
    protected virtual void UpdateTarget() {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, homingAttackRange, homingAttackMask);
        Collider2D nearest = null;
        float nearestDist = float.PositiveInfinity;
        foreach (Collider2D target in targets) {
            float d = Vector2.Distance(rb.position, target.transform.position);
            if (d < nearestDist) {
                nearestDist = d;
                nearest = target;
            }
        }
        if (nearest) curTarget = nearest.transform;
        else curTarget = null;
    }
    protected virtual void UpdateInput() {
        movement = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        doJump = Input.GetButtonDown("Jump");
    }
    protected virtual void FixedUpdate() {
        if (transform.position.y < -70) Kill();
        if (isGrounded && timer <= 0) {
            spring = false;
            if (jump || stomp || homingAttack) spinning = false;
            jump = false;
        }
        normalCollider.enabled = !spinning;
        spinCollider.enabled = spinning;
        sr.enabled = !spinning;
        ball.enabled = spinning;
        if (timer > 0) timer -= Time.deltaTime;
        bool s = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        isGrounded = s;
        if (spring) {
            sr.transform.up = rb.velocity.normalized;
            spinning = false;
            jump = false;
        } else {
            sr.transform.up = transform.up;
        }
        if (homingAttack && curTarget) {
            float d = Vector2.Distance(curTarget.position, rb.position);
            Vector2 dir = curTarget.position - transform.position;
            rb.velocity = dir.normalized * homingAttackSpeed;
            if (d < 1f) {
                homingAttack = false;
                curTarget = null;
            }
        }
        if (!inputEnabled) return;
        trail.emitting = stomp || spinning;
        if (stomp) {
            homingAttack = false;
            rb.velocity = Vector2.down * stompSpeed;
            if (isGrounded) {
                Instantiate(stompEffect, transform.position, transform.rotation);
                stomp = false;
            }
            return;
        }
        float a = speed * movement;
        if (!isGrounded) a *= midAirSpeedMultiplier;
        rb.AddForce(Vector2.right * a);
    }
    protected override void Update() {
        base.Update();
        if (!Input.touchSupported && SystemInfo.deviceType != DeviceType.Handheld) UpdateInput();
        if (!inputEnabled) return;
        if (vertical < -0.5f && !stomp && !isGrounded) {
            stomp = true;
            spinning = true;
        }
        if (doJump && !isGrounded && !stomp && !homingAttack) {
            UpdateTarget();
            if (curTarget) {
                homingAttack = true;
                spinning = true;
            }
        }
        if (doJump && isGrounded && !stomp) {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            isGrounded = false;
            spinning = true;
            timer = .2f;
            jump = true;
        }
        if (!curTarget) homingAttack = false;
        doJump = false;
    }
    public override void TakeDamage(float dmg, bool ignoreInvincibility = false) {
        base.TakeDamage(dmg, ignoreInvincibility);
        if (isDead) return;
        if (invincibilityLeft > 0) return;
        StartCoroutine(_SlowMo());
    }
    IEnumerator _SlowMo() {
        RigidbodyConstraints2D c = rb.constraints;
        Vector2 v = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSecondsRealtime(.1f);
        rb.constraints = c;
        rb.velocity = v;
        
    }
    public override void Kill() {
        if (isDead) return;
        isDead = true;
        health = 0;
        rb.constraints = RigidbodyConstraints2D.None;
        
        inputEnabled = false;
        StageLoader.main.PlayerRespawn(this);
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
