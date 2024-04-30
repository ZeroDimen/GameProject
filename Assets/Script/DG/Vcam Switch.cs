using UnityEngine;
using Cinemachine;

public class VcamSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    //public CinemachineVirtualCamera vcam2;
    public CinemachineBlendListCamera vcam2;

    private void Update()
    {
        // 버튼 입력 감지
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleVCam();
        }
    }

    private void ToggleVCam()
    {
        // VCam1이 활성화되어 있다면 VCam2 활성화, VCam1 비활성화
        if (vcam1.gameObject.activeSelf)
        {
            vcam1.gameObject.SetActive(false);
            vcam2.gameObject.SetActive(true);
        }
        // VCam2가 활성화되어 있다면 VCam1 활성화, VCam2 비활성화
        else
        {
            vcam2.gameObject.SetActive(false);
            vcam1.gameObject.SetActive(true);
        }
    }
}
