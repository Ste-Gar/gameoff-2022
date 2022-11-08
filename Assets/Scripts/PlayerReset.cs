using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    const string RESETPLAYER_BTN = "ResetPlayer";

    Vector3 startPosition;
    Quaternion startRotation;
    RagdollManager ragdollManager;

    private void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    //void Update()
    //{
    //    if (Input.GetButtonDown(RESETPLAYER_BTN))
    //    {
    //        //transform.position = startPosition + Vector3.up * .5f;
    //        //transform.rotation = startRotation;

    //        ragdollManager.DisableRagdoll();
    //    }
    //}
}
