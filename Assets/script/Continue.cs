using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class Continue : MonobitEngine.MonoBehaviour
{
    [SerializeField] GameObject ui_start;
    [SerializeField] GameObject ui_result;

    public board b;
    public MonoScript mono;

    // Start is called before the first frame update
    void Start()
    {
        if (!MonobitNetwork.isHost)
        {
            Destroy(gameObject.transform.FindChild("btn_rematch").gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClickContinue()
    {
        if (System_manager.play_mode == PLAY_MODE.single)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (b.komaArray[i, j] != null)
                        Destroy(b.komaArray[i, j].gameObject);
                }
            }
            ui_result.SetActive(false);
            ui_start.SetActive(true);
        } 
        else if (System_manager.play_mode == PLAY_MODE.multi)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (b.komaArray[i, j] != null) 
                        Destroy(b.komaArray[i, j].gameObject);
                }
            }
            ui_result.SetActive(false);
            ui_start.SetActive(true);
        }
    }
}
