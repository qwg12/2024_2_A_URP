using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disslove : MonoBehaviour
{
    public Material material;                       //���׸����� �޾ƿ´�    
    public float amount;                            //������ ���� �ҷ��´�
    public bool bDisslove;                          //���� �÷���

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
