using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterSO monsterSO;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject deadParticle;
    private float _hp;
    private float _speed;
    private float _power;
    private List<Transform> _path = new();
    private int _pathIndex = 0;

    private Quaternion _lookLeft = new(0, 0, 0, 0);
    private Quaternion _lookRight = new(0, 180, 0, 0);
    private int _sortingOrderBase = 5000;
    private bool _insideArea = false;
    private bool _isDead = false;

    private Tween _tween;

    public bool InsideArea { get => _insideArea; set => _insideArea = value; }

    public void Init(List<Transform> path) 
    {
        Reset();
        _hp = monsterSO.HP;
        _speed = monsterSO.Speed;
        _power = monsterSO.Power;
        sr.color = monsterSO.Color;
        _path = path;
        transform.position = _path[_pathIndex].position;
        _pathIndex++;
        WalkOnThePath();
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            StartCoroutine(Destroy());
        }
    }

    private void GiveDamage()
    {
        SoundManager.Instance.Play("Enemy_Shoot");
        animator.SetTrigger("Hit");
    }

    private void WalkOnThePath()
    {
        animator.SetBool("Dead", false);
        if (transform.position.x < _path[_pathIndex].position.x) transform.rotation = _lookLeft;
        else transform.rotation = _lookRight;
        sr.sortingOrder = (int)(_sortingOrderBase - transform.position.y * 10);

        _tween = transform.DOMove(_path[_pathIndex].position, CalculateTime()).SetEase(Ease.Linear).OnComplete(() =>
        {
            _pathIndex++;
            if (_pathIndex >= _path.Count) 
            {
                PoolManager.Instance.ReturnObject(this.gameObject);
                return;
            }
            WalkOnThePath();
        });
    }

    private float CalculateTime()
    {
        return Vector3.Distance(_path[_pathIndex].position, transform.position) / _speed;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovementArea"))
        {
            _insideArea = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovementArea"))
        {
            _insideArea = true;
        }
        if (other.CompareTag("Finish"))
        {
            UIManager.Instance.SetHPText();
        }
        if (other.CompareTag("Player") && !_isDead)
        {
            GiveDamage();
            other.GetComponent<PlayerController>().TakeDamage(_power);
        }
    }

    IEnumerator Destroy()
    {
        _isDead = true;
        _tween?.Kill();
        deadParticle.SetActive(true);
        animator.SetBool("Dead", true);
        WaveController.Instance.MonsterList.Remove(this);
        WaveController.Instance.CheckWave();
        yield return Helper.GetWait(0.5f);
        PoolManager.Instance.ReturnObject(gameObject);
    }

    private void Reset()
    {
        _tween?.Kill();
        _pathIndex = 0;
        _insideArea = false;
        deadParticle.SetActive(false);
        _isDead = false;
    }
}


