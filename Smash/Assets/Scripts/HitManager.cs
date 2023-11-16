using UnityEngine;

public class HitManager : MonoBehaviour
{
    private DamageManager _damageManager;
    private PlayerController _playerController;

    private void Awake()
    {
        _damageManager = GetComponent<DamageManager>();
        _playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Hit"))
        {
            if(other.GetComponentInParent<PlayerController>().lastHit == PlayerController.Hit.heavy)
            {
                HeavyAttack HA = new();
                _damageManager.TakeDamage(HA.getDamage());
                _playerController.Bump(HA.getPower(), other.transform.parent.localScale.x == _playerController.playerScale);
            }
            else if (other.GetComponentInParent<PlayerController>().lastHit == PlayerController.Hit.ligth)
            {
                LightAttack LA = new();
                _damageManager.TakeDamage(LA.getDamage());
                _playerController.Bump(LA.getPower(), other.transform.parent.localScale.x == _playerController.playerScale);
            }
        }
    }
}
