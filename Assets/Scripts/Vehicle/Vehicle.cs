using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 50;
    [SerializeField] float maxSpeed = 50;
    [SerializeField] float scoreMultiplier = 1;
    public float ScoreMultiplier { get { return scoreMultiplier; } }

    public float m_MaxDistance;
    bool m_HitDetect;
    RaycastHit m_Hit;
    Collider m_Collider;
    Transform player;
    float i = 0;

    private FMODUnity.StudioEventEmitter istance;

    public float distance;

    private FMOD.Studio.EventInstance instance;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        var target = GameObject.Find("Player");
        player = target.GetComponent<Transform>();

        m_MaxDistance = 50.0f;
        m_Collider = GetComponent<Collider>();

        istance = GetComponent<StudioEventEmitter>();


    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddRelativeForce(Vector3.forward * speed);

        distance = Vector3.Distance(transform.position, player.transform.position);

        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);

        if (m_Hit.transform == player)
        {


            if (distance < 20)
            {

                if (i == 0)
                {
                    instance = FMODUnity.RuntimeManager.CreateInstance("event:/Horn");
                    instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    instance.start();
                    i++;
                }
                else
                    StartCoroutine(Wait());
            }



            Debug.Log("Hit : " + m_Hit.collider.name + " " + distance + " " + i);
        }


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("waitisover");
        i = 0;
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
