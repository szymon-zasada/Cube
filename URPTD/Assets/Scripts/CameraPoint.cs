using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cinemachine;


public class CameraPoint : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLook;
    public async void SetPosition(Vector3 pos)
    {
        float timer = .5f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, pos, 2f * Time.deltaTime);
            await Task.Yield();
        }
        StopCameraMovement();
    }

    public void StopCameraMovement()
    {
        freeLook.m_XAxis.m_MaxSpeed = 0f;
        freeLook.m_YAxis.m_MaxSpeed = 0f;
    }

    public void StartCameraMovement()
    {
        freeLook.m_XAxis.m_MaxSpeed = 60f;
        freeLook.m_YAxis.m_MaxSpeed = 2f;
    }
}
