using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{

    private const int MaxBounces = 2;

    private int _mBounceCount;
    private bool _mReadyToShoot;
    private bool _mHasBounced;
    private Rigidbody _mRigidBody;
    private Vector3 _mOriginalPosition;
    [SerializeField] private Vector3 m_forceVector;
    private Camera _mCamera;
    private SphereCollider _mSphereCollider;
    public Action<bool> OnScore;
    private Collider _mTargetZone;
    
    public Collider TargetZone
    {
        get => _mTargetZone;
        set => _mTargetZone = value;
    }
    
    void Start()
    {
        _mBounceCount = 0;
        _mOriginalPosition = transform.position;
        _mRigidBody = GetComponent<Rigidbody>();
        _mCamera = Camera.main;
        ResetPosition();
    }

    void Update()
    {
        if (_mReadyToShoot)
            FollowCamera();
    }

    private void ResetPosition()
    {
        _mBounceCount = 0;
        transform.position = _mOriginalPosition;
        _mReadyToShoot = true;
        _mHasBounced = false;
        _mRigidBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public void LaunchBall(InputAction.CallbackContext context)
    {
        if (!_mReadyToShoot)
            return;
        
        _mReadyToShoot = false;
        _mRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        var transformedForce = _mCamera.transform.TransformDirection(m_forceVector);
        _mRigidBody.AddForce(transformedForce);
    }

    private void FollowCamera()
    {
        transform.position = _mCamera.transform.TransformDirection(_mOriginalPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Target"))
        {
            OnScore(_mHasBounced);
            ResetPosition();
        }
        else if (_mBounceCount >= MaxBounces || transform.position.z > _mTargetZone.bounds.max.z)
        {
            ResetPosition();
        }
        else
        {
            _mHasBounced = true;
            _mBounceCount += 1;
        }
        
    }


}
