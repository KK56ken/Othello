using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    Animator anim;
    float defaultY;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        defaultY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (defaultY == transform.position.y)
            {
                GameObject childObj = gameObject.transform.GetChild(0).gameObject;
                KOMA_TYPE type = childObj.GetComponent<komaScript>().type;
                if (type == KOMA_TYPE.Black)
                {
                    Debug.Log("BlackToWhite");
                    anim.Play("BlackToWhite", 0);
                    childObj.GetComponent<komaScript>().type = KOMA_TYPE.White;
                }
                else
                {
                    Debug.Log("WhiteToBlack");
                    anim.Play("WhiteToBlack", 0);
                    childObj.GetComponent<komaScript>().type = KOMA_TYPE.Black;
                }
            }
        }
    }
}
