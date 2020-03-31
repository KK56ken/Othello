using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed;
    Rigidbody rigidbody;
    Vector3 InputVec;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 vec = new Vector3(x, 0, z) * speed;
        InputVec = vec;
        //transform.position += vec; 
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(InputVec);

    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Item"))
        {
            //アイテムに当たった場合
            collider.gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
    }
}
