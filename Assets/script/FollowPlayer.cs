using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    Vector3 ToVec;
    // Start is called before the first frame update
    void Start()
    {
        ToVec = transform.position - Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position + ToVec;
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
}
