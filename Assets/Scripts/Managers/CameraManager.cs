using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineImpulseSource impulseSource;

    [SerializeField] private Vector2 shakeVelocity;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake()
    {
        Vector2 direction = Random.onUnitSphere;
        impulseSource.m_DefaultVelocity = direction.normalized * shakeVelocity;

        impulseSource.GenerateImpulse();
    }


}
