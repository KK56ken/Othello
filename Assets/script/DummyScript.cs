using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public int x;
    public int y;
    public KOMA_TYPE koma_type;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void setKoma()
    {
        board board = GameObject.Find("Board").GetComponent<board>();
        System_manager system = GameObject.Find("system").GetComponent<System_manager>();

        try
        {
            board.SetKoma(x, y,koma_type);
            system.put_check();
        }
        catch { }
        Destroy(gameObject);
    }
}
