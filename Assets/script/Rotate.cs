using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField] float rotate_speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,rotate_speed);
    }
}
