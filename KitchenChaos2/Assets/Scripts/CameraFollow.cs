using UnityEngine;
using System;

public class CameraFollow : MonoBehaviour
{

    private Func<Vector3> GetCameraFollowPositionFunc;
    private Camera myCamera;

    public void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
        
    }
    public void Update()
    {
        HandleMovement();
        
    }

    private void HandleMovement()
    {
        
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.y = transform.position.y;
        cameraFollowPosition.z = transform.position.z;
        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 1f;

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
            
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }

    
}
