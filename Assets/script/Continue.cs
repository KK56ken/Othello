using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    [SerializeField] GameObject ui_start;
    [SerializeField] GameObject ui_result;

    public board b;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClickContinue()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                Destroy(b.komaArray[i,j].gameObject);
            }
        }
        ui_result.SetActive(false);
        ui_start.SetActive(true);
    }
}
