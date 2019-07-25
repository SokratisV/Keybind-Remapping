using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public int up, right, back;

    private void Update()
    {
        transform.position = player.position + Vector3.up * up + Vector3.right * right + Vector3.back * back;
    }
}
