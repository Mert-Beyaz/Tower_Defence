using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private Quaternion _lookLeft = new(0, 0, 0, 0);
    private Quaternion _lookRight = new(0,180,0,0);
    private bool _insideArea = true;
    private Transform _target = null;
    private bool _isDead = false;

    public Animator Animator { get => animator; set => animator = value; }
    public Transform Target { get => _target; set => _target = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isDead) return;

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput = _moveInput.normalized;

        if (_moveInput.x == 0 && _moveInput.y == 0) 
        {
            animator.SetInteger("AnimState", 0);
            SoundManager.Instance.StopLoop("Walk");
        } 
        else
        {
            animator.SetInteger("AnimState", 1);
            SoundManager.Instance.PlayLoop("Walk");

        }

        if (Target != null)
        {
            if (transform.position.x > Target.position.x) transform.rotation = _lookLeft;
            else if (transform.position.x < Target.position.x) transform.rotation = _lookRight;
        }
        else
        {
            if (_moveInput.x > 0)
                transform.rotation = _lookRight;
            else if (_moveInput.x < 0)
                transform.rotation = _lookLeft;
        }
       
    }

    void FixedUpdate()
    {
        if (_isDead) return;
        if (_insideArea)
        {
            _rb.MovePosition(_rb.position + _moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovementArea"))
        {
            _insideArea = false;
            _rb.MovePosition(_rb.position - _moveInput * 0.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovementArea"))
        {
            _insideArea = true;
        }
    }

    public void Reset()
    {
        _isDead = false;
        _target = null;
        _insideArea = true;
        transform.position = Vector3.zero;
        Animator.SetBool("Dead", false);
    }
}
