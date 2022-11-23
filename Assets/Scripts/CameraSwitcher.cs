using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void SwitchCamera()
    {
        //cam.Priority = 1;
        cam.enabled = false;
    }
}
