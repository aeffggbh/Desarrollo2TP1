﻿using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementCalculator : IPlayerMovementCalculator
{
    private CinemachineBrain _cineMachineBrain;
    public Camera Camera { get; set; }

    public PlayerMovementCalculator()
    {
        _cineMachineBrain = CineMachineManager.Instance?.cineMachineBrain;
        Camera = _cineMachineBrain?.GetComponent<Camera>();
    }

    public void SetCamera(Camera camera)
    {
        Camera = camera;
    }

    public Vector3 GetDirection(Vector2 moveInput)
    {
        if (!Camera)
            return new(moveInput.x, 0, moveInput.y);

        moveInput.Normalize();
        Vector3 forward = Camera.transform.forward;
        Vector3 right = Camera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;
        movement.y = 0;

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        return movement;
    }
}