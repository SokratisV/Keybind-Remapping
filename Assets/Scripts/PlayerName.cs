using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public GameObject player;
    private Camera mainCam;
    public float positionOffset;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + Vector3.up * positionOffset;
        transform.LookAt(transform.position * 2 - mainCam.transform.position);
    }
}
