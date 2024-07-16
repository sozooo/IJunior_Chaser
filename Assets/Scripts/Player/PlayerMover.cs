using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _strafeSpeed = 10;

    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _horizontalSensitivity = 20;
    [SerializeField] private float _verticalSensitivity = 10;
    [SerializeField] private float _verticalMinAngle = -89;
    [SerializeField] private float _verticalMaxAngle = 89;

    private Input _input;
    private CharacterController _characterController;
    private Vector2 _moveDirection;
    private Transform _cameraTransform;

    private Vector2 _lookDirection;
    private float _cameraAngle = 0;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _cameraTransform = _camera.transform;
        _input = new Input();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += context => _moveDirection = context.ReadValue<Vector2>();
        _input.Player.Move.canceled += context => _moveDirection = Vector2.zero;

        _input.Player.Look.performed += context => _lookDirection = context.ReadValue<Vector2>();
        _input.Player.Look.canceled += context => _lookDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Move.performed -= context => _moveDirection = context.ReadValue<Vector2>();
        _input.Player.Move.canceled -= context => _moveDirection = Vector2.zero;

        _input.Player.Look.performed -= context => _lookDirection = context.ReadValue<Vector2>();
        _input.Player.Look.canceled -= context => _lookDirection = Vector2.zero;
    }

    private void Move()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

        Vector3 direction = forward * _moveDirection.y * _speed + right * _moveDirection.x * _strafeSpeed;
        direction *= Time.deltaTime;
        Vector3 gravity = Vector3.down * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            _characterController.Move(direction + gravity);
        }
        else
        {
            _characterController.Move(_characterController.velocity * Time.deltaTime + gravity);
        }
    }

    private void Look()
    {
        _cameraAngle -= _lookDirection.y * _verticalSensitivity * Time.deltaTime;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraAngle;

        transform.Rotate(Vector3.up * _lookDirection.x * _horizontalSensitivity * Time.deltaTime);
    }
}