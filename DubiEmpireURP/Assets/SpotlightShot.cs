using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightShot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    void DDD()
    {
        RenderTexture rt = GetComponent<Camera>().targetTexture;
        //    = Selection.activeObject as RenderTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = "Assets/" + FindObjectOfType<TestNew>().GetCraftName() + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        // AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            DDD();

    }
}
