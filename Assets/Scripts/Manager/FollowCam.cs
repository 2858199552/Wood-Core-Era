using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public FollowCam S; //相机单例
    public float easing = 0.05f;

    public bool shakeis; //镜头抖动开关

    public GameObject player;
    public float camZ; //相机position.z轴
    float ddtime = 1;

    private void Awake()
    {
        S = this;
    }

    private void Update()
    {
        Vector3 destination = player.transform.position;
        destination = Vector3.Lerp(transform.position, destination , easing);
        destination.z = transform.position.z;
        if (shakeis)
        {
            float x = Random.Range(-0.5f, 0.5f); //镜头晃动x轴大小
            float y = Random.Range(-0.5f, 0.5f); //镜头晃动y轴大小
            destination = new Vector3(x + destination.x, y + destination.y, destination.z);
        }
        transform.position = destination;
    }
}
