using UnityEngine;

public class HitManager : MonoBehaviour
{
    private DamageManager _damageManager;
    private PlayerController _playerController;

    private void Awake()
    {
        _damageManager = GetComponent<DamageManager>();
        _playerController = GetComponent<PlayerController>();
        UIManager.instance.SetDamageTextPlayer1("Joueur 1 : " + _damageManager.GetDamageReceived() + " %");
        UIManager.instance.SetDamageTextPlayer2("Joueur 2 : " + _damageManager.GetDamageReceived() + " %");
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

            if (gameObject.layer == 3)
            {
                UIManager.instance.SetDamageTextPlayer1("Joueur 1 : " + _damageManager.GetDamageReceived() + " %");
            }
            if (gameObject.layer == 6)
            {
                UIManager.instance.SetDamageTextPlayer2("Joueur 2 : " + _damageManager.GetDamageReceived() + " %");
            }
        }
    }
}
