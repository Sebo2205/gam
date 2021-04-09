using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 3;
    public float health = 3;
    public float invincibility = 1;
    protected bool isDead = false;
    [HideInInspector] public float invincibilityLeft = 0;
    protected virtual void Start() {
        health = maxHealth;
    }
    protected virtual void Update() {
        if (invincibilityLeft > 0) invincibilityLeft -= Time.deltaTime;
    }
    public virtual void TakeDamage(float dmg, bool ignoreInvincibility = false) {
        if (!ignoreInvincibility && invincibilityLeft > 0) return;
        health -= dmg;
        if (health <= 0) {
            Kill();
            return;
        }
        invincibilityLeft = invincibility;
    }
    public virtual void Kill() {
        if (isDead) return;
        isDead = true;
        health = 0;
        Destroy(gameObject);
    }
}
