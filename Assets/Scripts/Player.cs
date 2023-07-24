using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody m_Rigidbody;
    public float fMaxForce = 500.0f;
    private float m_CurForce = 0.0f;

    public GameObject Box=null;
    public float fMinDistance = 1.2f;
    public float fMaxDistance = 3.0f;
    public float fMinHeight = 0.3f;
    public float fMaxHeight = 1.2f;

    private Vector3 m_Directiion = Vector3.forward;
    private float m_Distance = 0.0f;
    private float m_Height = 0.0f;

    private GameObject m_CurCube = null;
    private GameObject m_NextCube = null;

    private int m_Score = 0;

    private Vector3 m_CameraOffest = Vector3.zero;
    private GameObject m_Plane = null;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody=GetComponent<Rigidbody>();
        m_Plane = GameObject.FindGameObjectWithTag("Plane");
        m_NextCube =GenerateBox();
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标左键
        GameObject obj=GetHitObject();
        //获取当前cube
        if (obj != null)
        {
            if (obj.tag == "Cube")
            {
               if (m_CurCube == null)
               {
                    m_CurCube = obj;
                    m_CameraOffest = Camera.main.transform.position - m_CurCube.transform.position;//初次的差值
               }
               else if(m_NextCube==obj)
               {
                    m_Score++;
                    Debug.Log(m_Score);

                    Destroy(m_CurCube);
                    m_CurCube=m_NextCube;
                    m_NextCube = GenerateBox();

                    m_Rigidbody.Sleep();//刚体冻结一帧
                    m_Rigidbody.WakeUp();

                   
                }

                if (Input.GetMouseButton(0))
                {
                    m_CurForce += Time.deltaTime * 100.0f;
                    if (m_CurForce > fMaxForce)
                    {
                        m_CurForce = fMaxForce;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Jump();
                    m_CurForce = 0.0f;
                }


              
                ShowScale();
                MoveCameraAndPlane();

            }
            else
            {

            }
        }


      
    }

    private void MoveCameraAndPlane()
    {
        //Camera.main.transform.position = m_CurCube.transform.position + m_CameraOffest;//新立方体加上固定偏移量
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
           m_CurCube.transform.position + m_CameraOffest, Time.deltaTime * 2);
        Vector3 pos = m_CurCube.transform.position;
        pos.y = 0;
        m_Plane.transform.position = pos;
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
        m_Rigidbody.AddForce(m_Directiion * m_CurForce);
    }

    private GameObject GenerateBox()
    {
        GameObject box = GameObject.Instantiate(Box);

        m_Distance=Random.Range(fMinDistance, fMaxDistance);
        m_Height=Random.Range(fMinHeight, fMaxHeight);
        m_Directiion = Random.Range(0, 2) == 1 ? Vector3.forward : Vector3.left; //[0,2)

        Vector3 pos = Vector3.zero;
        if (m_CurCube == null)
            pos = m_Directiion * m_Distance + transform.position;
        else
            pos = m_Directiion * m_Distance + m_CurCube.transform.position;

        pos.y = fMaxHeight;//添加刚体之后自然下落效果
        box.transform.position = pos;
        box.transform.localScale = new Vector3(1, m_Height, 1);//只缩放y
        box.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        
        return box;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("enter"+collision.gameObject.tag);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("exit"+collision.gameObject.tag);
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("stay"+collision.gameObject.tag);
    //}

    private GameObject GetHitObject()
    {
        RaycastHit hit;//射线检测可以避免，player和cube紧挨着的碰撞
        if (Physics.Raycast(transform.position, Vector3.down,out hit, 0.2f))
        {
            Debug.Log(hit.collider.tag);
            return hit.collider.gameObject;
        }
        else
        {
            Vector3[] vOffests = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            foreach(Vector3 vof in vOffests)
            {
                //检测前后左右0.1范围内射线，保证加分
                if (Physics.Raycast(transform.position+vof*0.1f, Vector3.down, out hit, 0.2f))
                {
                    Debug.Log(hit.collider.tag);
                    return hit.collider.gameObject;
                }
            }
        }

        return null;
    }

}
