using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(Rigidbody))]
public class VehicleSound : MonoBehaviour
{
    [SerializeField] float m_MaxDistance = 50f;
    bool m_HitDetect;
    RaycastHit m_Hit;
    Collider m_Collider;

    private FMOD.Studio.EventInstance instance;

    [SerializeField] float hornInterval = 5f;
    private float hornTimer;


    private void Awake()
    {
        m_Collider = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        hornTimer = hornInterval;
    }

    private void FixedUpdate()
    {
        hornTimer += Time.fixedDeltaTime;
        if (hornTimer < hornInterval) return;

        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);

        if (m_Hit.collider == null) return;
        if (m_Hit.collider.CompareTag("Player"))
        {

            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Horn");
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instance.start();

            hornTimer = 0;

            // Debug.Log("Hit : " + m_Hit.collider.name + " " + distance);
        }
    }
}
