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
    Transform player;
    Collider p_Collider;

    private float distance;

    private FMOD.Studio.EventInstance instance;
    private FMODUnity.StudioEventEmitter m_music;

    [SerializeField] float hornInterval = 10f;
    private float hornTimer;


    private void Awake()
    {
        var target = GameObject.Find("Player");
        player = target.GetComponent<Transform>();
        m_Collider = GetComponent<Collider>();
        p_Collider = target.GetComponent<Collider>();
        var target2 = GameObject.Find("BackgroundMusic");
        m_music = target2.GetComponent<FMODUnity.StudioEventEmitter>();
        }

    private void Start()
    {
        hornTimer = hornInterval;
    }

    private void FixedUpdate()
    {
        hornTimer += Time.fixedDeltaTime;
        if (hornTimer < hornInterval) return;
        
        distance = Vector3.Distance(transform.position, player.transform.position);

        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);

        if (m_Hit.transform == player)
        {


            if (distance < 20)
            {

                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Horn");
                instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                instance.start();

                hornTimer = 0;

            }

           // Debug.Log("Hit : " + m_Hit.collider.name + " " + distance);
        }

        if (!p_Collider.enabled)
            m_music.SetParameter("Flight", 1);
        else
            m_music.SetParameter("Flight", 0);


            

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }

}
