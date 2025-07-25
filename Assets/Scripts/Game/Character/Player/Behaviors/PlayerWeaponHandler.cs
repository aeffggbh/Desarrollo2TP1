﻿using System;
using UnityEngine;

public class PlayerWeaponHandler : IPlayerWeaponHandler
{
    public Weapon CurrentWeapon { get; set; }

    private Transform _weaponParent;
    private GameObject _bulletSpawnGO;
    private float _grabDropCooldown;
    private float _maxWeaponDistance;
    PlayerAnimationController _animationController;
    PlayerMediator _playerController;

    public PlayerWeaponHandler(GameObject bulletSpawn, float maxWeaponDistance, float grabDropCooldown, Weapon currentWeapon, PlayerAnimationController playerAnimation, Transform weaponParent)
    {
        _bulletSpawnGO = bulletSpawn;
        _maxWeaponDistance = maxWeaponDistance;
        _grabDropCooldown = grabDropCooldown;
        _animationController = playerAnimation;
        _weaponParent = weaponParent;

        _playerController = PlayerMediator.PlayerInstance;

        CurrentWeapon = _playerController.Player.CurrentWeapon;
    }

    /// <summary>
    /// Returns the weapon that the player is currently pointing to, if any.
    /// </summary>
    /// <returns></returns>
    public Weapon GetPointedWeapon(Camera camera)
    {
        RaycastHit? hit = null;

        if (RayManager.PointingToObject(camera.transform, _maxWeaponDistance, out RaycastHit hitInfo))
            hit = hitInfo;

        if (hit != null)
        {
            Weapon weapon = hit.Value.collider.gameObject.GetComponent<Weapon>();

            if (weapon)
                return weapon;
        }

        return null;

    }

    public void GrabPointedWeapon(Camera camera)
    {
        Weapon pointedWeapon = GetPointedWeapon(camera);
        if (pointedWeapon)
            GrabWeapon(pointedWeapon);
    }

    public void GrabWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
            DropWeapon();

        CurrentWeapon = weapon;
        CurrentWeapon.user = PlayerMediator.PlayerInstance.Player;
        CurrentWeapon.SetBulletSpawn(_bulletSpawnGO);
        CurrentWeapon.Hold(_weaponParent);
        _playerController.Player.CurrentWeapon = CurrentWeapon;
        _animationController?.AnimateGrabWeapon();
    }

    public void DropWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.user = null;
            CurrentWeapon.Drop();
            CurrentWeapon = null;
            _animationController?.AnimateDropWeapon();
        }
    }
}