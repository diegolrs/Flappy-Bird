using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputs _input;
    [SerializeField] PlayerAudio _audio;
    public PlayerParameters MovementParameters;

    Vector3 _velocity;
    Vector3 _rotation;

    private bool _isDead;
    public Vector3 Velocity { get => _velocity; }

    public void Die()
    {
        _isDead = true;
        _velocity = Vector3.zero;
        _audio.OnDie();
    }

    private void Awake()
    {
        _velocity = new Vector3();
        _rotation = new Vector3();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        ApplyGravity(in delta);
        TryApplyFallRotation(in delta);

        if(!_isDead)
        {
            MoveForward();
            ProcessInput();
        }

        transform.position += _velocity * delta;
        transform.rotation = Quaternion.Euler(_rotation);
    }

    private void ProcessInput() 
    {
        if(_input.TapUp())
        {
            Flap();
        }
    }

    public void OnHitGround()
    {
        _audio.OnHitGround();
        enabled = false;
    }

    public void Flap()
    {
        _velocity.y = MovementParameters.FlapSpeed;
        _rotation.z = MovementParameters.FlapRotation;
        _audio.OnFlap();
    }

    private void MoveForward()
    {
        _velocity.x = MovementParameters.ForwardSpeed;
    }

    private void ApplyGravity(in float delta)
    {
        _velocity.y -= MovementParameters.Gravity * delta;
    }

    private void TryApplyFallRotation(in float delta)
    {
        if(_velocity.y <= 0)
        {
            _rotation.z -= delta * MovementParameters.FallingRotationSpeed;
            _rotation.z = Mathf.Clamp(
                                        _rotation.z, 
                                        MovementParameters.FallingRotationAngle,
                                        MovementParameters.FlapRotation
                                    );
        }
    }
}
