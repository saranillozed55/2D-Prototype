using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class BossBase : Breakable
{
    [Header("References")]
    public Rigidbody2D _rb;
    public Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Transform _playerTransform;
    protected DamageFlash _damageFlash;
    protected CameraShakeSource _cameraShakeSource;

    [Space]
    [Header("Boss Settings")]
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _contactDamage = 5;

    public int Phase { get; private set; } = 1;

    protected override void Awake()
    {
        base.Awake();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        _cameraShakeSource = GetComponent<CameraShakeSource>();
    }
    public override void Hurt(int damage, Vector2 hitDirection)
    {
        base.Hurt(damage, hitDirection);
        Debug.Log("Boss took damage: " + damage);
        _damageFlash.CallDamageFlash();

        HitStop.Instance.Stop(0.05f);
        _cameraShakeSource.ShakeCamera(0.1f, Vector3.up);
        CheckPhaseTwo();
    }

    private void CheckPhaseTwo()
    {
        if(health <= 50 && Phase == 1)
        {
            Phase = 2;
            TriggerPhaseTwo();
            Debug.Log("Boss going to Phase two");
        }
    }

    protected virtual void TriggerPhaseTwo()
    {

    }
    public Transform PlayerTransform => _playerTransform;
    public float MoveSpeed => _moveSpeed;

}
