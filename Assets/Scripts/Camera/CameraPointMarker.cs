using UnityEngine;

public class CameraPointMarker : MonoBehaviour
{
    public CameraPoint data;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
    }
}
