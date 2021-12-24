using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform player;

    void Start()
    {
        transform.position = player.position;
    }

    void Update()
    {
        transform.position = player.transform.position;
    }
}