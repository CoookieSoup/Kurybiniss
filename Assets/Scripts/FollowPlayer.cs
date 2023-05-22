using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float CameraFollowSpeed = 2f;
    public Transform player;
    public Transform cameraTransform;

    public bool stopVertical = false;
    public bool stopHorizontal = false;
    public bool stopBoth = false;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cameraTransform = GetComponent<Transform>();
        cameraTransform.position = player.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!stopVertical && !stopHorizontal && !stopBoth)
        {
            Vector3 newPos = new Vector3(player.position.x, player.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, CameraFollowSpeed * Time.deltaTime);
        }
        if (stopVertical)
        {
            Vector3 newPos = new Vector3(player.position.x, transform.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, CameraFollowSpeed * Time.deltaTime);
        }
        if (stopHorizontal)
        {
            Vector3 newPos = new Vector3(transform.position.x, player.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, CameraFollowSpeed * Time.deltaTime);
        }
        if (stopBoth)
        {
            Vector3 newPos = new Vector3(0f, 0f, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, CameraFollowSpeed * Time.deltaTime);
        }

    }
}
