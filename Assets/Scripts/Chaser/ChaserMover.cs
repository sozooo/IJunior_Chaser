using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChaserMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private PlayerMover _target;
    [SerializeField] private bool _isActive;
    [SerializeField] private float _distance;

    private Rigidbody _rigidbody;
    private Transform _targetTransform;
    private Vector3 _targetDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _targetTransform = _target.transform;
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, _targetTransform.position) > _distance)
        {
            _targetDirection = new Vector3(_targetTransform.position.x - transform.position.x, Vector3.up.y, _targetTransform.position.z - transform.position.z).normalized;

            _rigidbody.Move(transform.position + _targetDirection * Time.deltaTime * _speed, Quaternion.identity);
        }

        _rigidbody.AddForce(Physics.gravity, ForceMode.Force);
    }
}
