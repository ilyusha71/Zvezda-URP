using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
public class TestNew : MonoBehaviour
{
    public CinemachineFreeLook FreeLook;
    public Transform core;
    public Transform[] crafts;
    public int index;
    // Start is called before the first frame update
    // public RenderTexture rt;
    public Transform seri;
    public CinemachineFreeLook[] cmFreeLook;

    public Text ttt;

    void Awake ()
    {
        // crafts = seri.GetComponentsInChildren<Kocmoca.Protodesign>();
        crafts = new Transform[seri.childCount];
        cmFreeLook = new CinemachineFreeLook[crafts.Length];
        for (int i = 0; i < crafts.Length; i++)
        {
            crafts[i] = seri.GetChild (i);
            if (crafts[i].GetComponentInChildren<Animator> ())
                crafts[i].GetComponentInChildren<Animator> ().enabled = false;
            cmFreeLook[i] = crafts[i].GetComponentInChildren<CinemachineFreeLook> ();
        }
        // cmsFreeLook.enabled = true;
    }

    void Center ()
    {
        for (int i = 0; i < crafts.Length; i++)
        {
            crafts[i].transform.localPosition = Vector3.one * 999;
            // cmFreeLook[i].enabled = false;
            // if (i == index)
            // {
            //     crafts[i].transform.localPosition = Vector3.zero;
            //     cmFreeLook[i].enabled = true;
            //     cmFreeLook[i].m_YAxis.Value = 1.0f;
            //     cmFreeLook[i].m_XAxis.Value = 0.0f;
            //     cmFreeLook[i].m_Heading.m_Bias = 135.0f;
            // }
        }
        crafts[index].GetComponent<Warfare.Prototype> ().FreeLookSetting (FreeLook);
        FreeLook.m_YAxis.Value = 1.0f;
        FreeLook.m_XAxis.Value = 0.0f;
        FreeLook.m_Heading.m_Bias = six? 150.0f : 135.0f;
        crafts[index].transform.localPosition = Vector3.zero;
        crafts[index].GetComponent<Warfare.Prototype> ().AlignCentre ();
        // GetComponent<Camera> ().orthographicSize = 7.0f / crafts[index].GetComponent<Kocmoca.Protodesign> ().GetScalePower ();
    }
    bool six;
    void DDD ()
    {
        RenderTexture rt = GetComponent<Camera> ().targetTexture;
        //    = Selection.activeObject as RenderTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D (rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
        tex.Apply ();
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG ();

        string path = "Assets/" + crafts[index].transform.name + ".png";
        System.IO.File.WriteAllBytes (path, bytes);
        // AssetDatabase.ImportAsset(path);
        Debug.Log ("Saved to " + path);
    }
    // public CinemachineFreeLook cmFreeLook;

    // Update is called once per frame

    public string GetCraftName ()
    {
        return crafts[index].transform.name;
    }

    void Update ()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        //     DDD();
        if (Input.GetKeyDown (KeyCode.M))
            six = !six;
        if (Input.GetKeyDown (KeyCode.E))
        {
            index = (int) Mathf.Repeat (++index, crafts.Length);
            Center ();
        }
        if (Input.GetKeyDown (KeyCode.Q))
        {
            index = (int) Mathf.Repeat (--index, crafts.Length);
            Center ();
        }
        // if (Input.GetKey (KeyCode.Mouse1))
        // {
        //     cmsFreeLook.m_XAxis.m_InputAxisValue = Input.GetAxis ("Mouse X");
        //     cmsFreeLook.m_YAxis.m_InputAxisValue = Input.GetAxis ("Mouse Y");
        // }
        // else
        // {
        //     cmsFreeLook.m_XAxis.m_InputAxisValue = 0;
        //     cmsFreeLook.m_YAxis.m_InputAxisValue = 0;
        // }
        // if (Input.GetKey (KeyCode.Mouse1))
        // {
        //     cmFreeLook[index].m_XAxis.m_InputAxisValue = Input.GetAxis ("Mouse X");
        //     cmFreeLook[index].m_YAxis.m_InputAxisValue = Input.GetAxis ("Mouse Y");
        // }
        // else
        // {
        //     cmFreeLook[index].m_XAxis.m_InputAxisValue = 0;
        //     cmFreeLook[index].m_YAxis.m_InputAxisValue = 0;
        // }
        if (Input.GetAxis ("Mouse ScrollWheel") != 0)
        {
            oSize = Mathf.Clamp (oSize -= Input.GetAxis ("Mouse ScrollWheel") * 1.0f, 0.1f, 15.0f);
            // cmFreeLook[index].m_Lens.OrthographicSize -= Input.GetAxis ("Mouse ScrollWheel") * 37;
            // radius = Mathf.Clamp (radius -= Input.GetAxis ("Mouse ScrollWheel") * 37, 2.7f, 18.2f);
        }
        // cmFreeLook[index].m_Lens.OrthographicSize = Mathf.Lerp (cmFreeLook[index].m_Lens.OrthographicSize, oSize, 0.073f);
        GetComponent<Camera> ().orthographicSize = oSize;
        // for (int i = 0; i < 2; i++)
        // {
        //     cmFreeLook[index].m_Orbits[i].m_Radius = Mathf.Lerp (cmFreeLook[index].m_Orbits[i].m_Radius, radius, 0.073f);
        // }
        if (Input.GetKey (KeyCode.D))
            transform.localPosition -= Vector3.right * 0.001f;
        if (Input.GetKey (KeyCode.A))
            transform.localPosition += Vector3.right * 0.001f;
        if (Input.GetKey (KeyCode.W))
            transform.localPosition -= Vector3.up * 0.001f;
        if (Input.GetKey (KeyCode.S))
            transform.localPosition += Vector3.up * 0.001f;
        if (Input.GetKey (KeyCode.X))
            transform.localPosition = Vector3.zero;

        Vector3 one = crafts[index].transform.position - transform.position;
        Vector3 two = new Vector3 (one.x, 0, one.z);

        float angle = Vector3.Angle (one, two); //求出两向量之间的夹角
        ttt.text = angle.ToString ();
    }
    float radius, oSize;
}