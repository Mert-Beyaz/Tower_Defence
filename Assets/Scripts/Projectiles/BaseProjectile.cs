using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    public float speed = 10f;
    public float damage = 20f;

    public virtual void Shoot()
    {
        SoundManager.Instance.Play("Player_Shoot");
    }
}
