using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody m_Rigidbody;
    public float fMaxForce = 500.0f;
    private float m_CurForce = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标左键

        if(Input.GetMouseButton(0)) {
            m_CurForce+=Time.deltaTime*100.0f;
            if(m_CurForce > fMaxForce )
            {
                m_CurForce = fMaxForce;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Jump();
            m_CurForce = 0.0f;
        }

        ShowScale();
    }

    //蓄力表现,通过对y轴进行缩放
    private void ShowScale()
    {
        float sc = 1 - (m_CurForce*0.5f/fMaxForce);
        Vector3 currentScale = transform.localScale;
        currentScale.y = 0.2f * sc;
        transform.localScale = currentScale;
    }

    private void Jump()
    {
        m_Rigidbody.AddForce(Vector3.up* m_CurForce);
        m_Rigidbody.AddForce(Vector3.forward* m_CurForce);
    }
}
