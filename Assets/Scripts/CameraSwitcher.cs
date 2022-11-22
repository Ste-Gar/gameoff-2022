using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    CinemachineVirtualCamera menuCam;

    private void Awake()
    {
        menuCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void SwitchCamera()
    {
        menuCam.Priority = 1;
    }
}
