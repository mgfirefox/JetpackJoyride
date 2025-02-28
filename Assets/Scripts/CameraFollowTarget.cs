using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private float distanceBetweenCameraCenterAndTarget;

    private void Start()
    {
        distanceBetweenCameraCenterAndTarget = transform.position.x - target.transform.position.x;
    }

    private void Update()
    {
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = target.transform.position.x + distanceBetweenCameraCenterAndTarget;
        transform.position = newCameraPosition;
    }
}
