using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Input _input;

    public Camera MainCamera => _camera;
    public Input MainInput => _input;

    private void Awake()
    {
        _input = new Input();
        Debug.Log("input added");
    }
}
