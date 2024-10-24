using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;                   //�ܻ� ȿ�� ���� �ð�
    public MovementInput moveScript;                //ĳ������ �������� �����ϴ� ��ũ��Ʈ
    public float speedBoost = 6;                    //�ܻ� ȿ�� ���� �ӵ� ������
    public Animator animator;                       //ĳ������ �ִϸ��̼��� �����ϴ� ������Ʈ
    public float animSpeedBoost = 1.5f;             //�ܻ� ȿ�� ��� �� �ִϸ��̼� �ӵ� ������

    [Header("Mash Releted")]                        //�޽�(3D ��) ���� ����
    public float meshRefreshRate = 0.1f;            //�ܻ��� �����Ǵ� �ð� ����
    public float meshDestoryDelay = 3.0f;           //������ �ܻ��� ������� �� �ɸ��� �ð�
    public Transform positionToSpawn;

    [Header("Shader Releted")]                      //���̴� ���� ����
    public Material mat;                            //�ܻ� ���� ����
    public string shaderVarRef;                     //���̴����� ����� ���� �̸� (alpha)
    public float shaderVarRate = 0.1f;              //���̴� ȿ���� ��ȭ �ӵ�
    public float shaderVarRefreshRate = 0.05f;      //���̴� ȿ���� ������Ʈ �Ǵ� �ð� ����

    private SkinnedMeshRenderer[] skinnedRenderer;      //ĳ������ 3D ���� ������ �ϴ� ������Ʈ��
    private bool isTreilActive;                         //���� �ܻ� ȿ���� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���ϴ� ����

    private float normalSpeed;                          //���� �̵� �ӵ��� �����ϴ� ����
    private float normalAnimSpeed;                      //���� �ִϸ��̼� �ӵ��� �����ϴ� ����
    
    //������ ������ ������ �����ϴ� �ڷ�ƾ
    IEnumerator AnimateMaterialFloat(Material m , float valueGoal, float rate, float refreshRate)
    {
        float valueToAnimate = m.GetFloat(shaderVarRef);

        while (valueToAnimate > valueGoal)
        {
            valueToAnimate -= rate;
            m.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator ActivateTrail(float timeActivated)
    {
        normalSpeed = moveScript.movementSpeed;
        moveScript.movementSpeed = speedBoost;

        normalAnimSpeed = animator.GetFloat("animSpeed");
        animator.SetFloat("animSpeed", animSpeedBoost);

        while (timeActivated > 0)
        {
            timeActivated -= meshRefreshRate;

            if (skinnedRenderer == null)
                skinnedRenderer = positionToSpawn.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedRenderer.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh m = new Mesh();
                skinnedRenderer[i].BakeMesh(m);
                mf.mesh = m;
                mr.material = mat;
                //�ܻ��� ���̵�ƿ� ȿ�� ����
                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestoryDelay);    //���� �ð� �� �ܻ� ����
            }
            //���� �ܻ� �������� ���
            yield return new WaitForSeconds(meshRefreshRate);
        }
        //���� �ӵ��� �ִϸ��̼� �ӵ��� ����
        moveScript.movementSpeed = normalAnimSpeed;
        animator.SetFloat("animSpeed",normalAnimSpeed);
        isTreilActive = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isTreilActive)
        {
            isTreilActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }
}
