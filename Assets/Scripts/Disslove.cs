using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disslove : MonoBehaviour
{
    public Material material;                       //메테리얼을 받아온다    
    public float amount;                            //설정한 값을 불러온다
    public bool bDisslove;                          //시작 플레그

    // Start is called before the first frame update
    void Start()
    {
        amount = -1;
        material.SetFloat("_Amount", amount);
    }

    // Update is called once per frame
    void Update()
    {
        if(bDisslove)
        {
            amount += Time.deltaTime;
            material.SetFloat("_Amount", amount);
        }        
    }
}
