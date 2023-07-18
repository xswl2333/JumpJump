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
        //ЪѓБъзѓМќ

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
        
    }

    private void Jump()
    {
        m_Rigidbody.AddForce(Vector3.up* m_CurForce);
        m_Rigidbody.AddForce(Vector3.forward* m_CurForce);
    }
}
