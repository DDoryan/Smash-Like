using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private float damageReceived;
    void Start()
    {
        damageReceived = 0.0f;
    }

    public void TakeDamage(float damage)
    {
        damageReceived += damage;
    }

    public float GetDamageReceived() {  return damageReceived; }
}
