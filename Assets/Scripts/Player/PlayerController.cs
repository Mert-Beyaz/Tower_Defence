using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float hp = 100;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootingTime;

    [Header("I Frame")]
    [SerializeField] private Color i_FrameColor;
    [SerializeField] private float i_FrameTime;
    [SerializeField] private GameObject i_FrameParticle;

    private PlayerMovement _playerMovement;
    private Transform _target = null;
    private float _targetDistance = Mathf.Infinity;
    private float _dist = Mathf.Infinity;
    private bool _inIFrame = false;
    private SpriteRenderer _sr;
    private GameObject projectile;

    private void OnEnable()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _sr = GetComponent<SpriteRenderer>();

        Reset();
        _playerMovement.Reset();
        StartCoroutine(StartSoohting());
    }

    IEnumerator StartSoohting()
    {
        yield return null;
        _target = null;
        SelectTarget();
        if (_target != null) Shoot(_target);
        yield return Helper.GetWait(shootingTime);
        StartCoroutine(StartSoohting());
    }

    private void SelectTarget()
    {
        _targetDistance = Mathf.Infinity;
        foreach (Monster monster in WaveController.Instance.MonsterList)
        {
            if (monster.InsideArea)
            {
                _dist = Vector3.Distance(transform.position, monster.transform.position);
                if (_dist < _targetDistance)
                {
                    _targetDistance = _dist;
                    _target = monster.transform;
                }
            }
        }
    }

    private void Shoot(Transform target)
    {
        _playerMovement.Target = target;
        gun.LookAt(target);
        switch (GameManager.Instance.Skill)
        {
            case SkillType.BlueProjectile:
                projectile = PoolManager.Instance.GetObject(PoolType.BlueProjectile);
                break;
            case SkillType.YellowProjectile:
                projectile = PoolManager.Instance.GetObject(PoolType.YellowProjectile);
                break;
        }
        projectile.transform.position = shootPoint.position;
        projectile.GetComponent<Projectile>().Shoot(target);
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0 || _inIFrame) return;
        hp -= damage;
        UIManager.Instance.SetPlayerHPUI(maxHp, hp);
        if (hp <= 0)
        {
            _playerMovement.IsDead = true;
            _playerMovement.Animator.SetBool("Dead", true);
        }
        else EntryIFrame();
    }

    private void EntryIFrame()
    {
        _inIFrame = true;
        i_FrameParticle.SetActive(true);
        _sr.DOColor(i_FrameColor, i_FrameTime/8).SetLoops(8, LoopType.Yoyo).OnComplete(() =>
        {
            _inIFrame = false;
            i_FrameParticle.SetActive(false);
        });
    }

    private void Reset()
    {
        hp = maxHp;
        UIManager.Instance.SetPlayerHPUI(maxHp, hp);
        _inIFrame = false;
        _dist = Mathf.Infinity;
        _targetDistance = Mathf.Infinity;
        _target = null;
        gun.rotation = Quaternion.identity;
    }
}
