using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Warfare
{
    public class Prototype : MonoBehaviour
    {

        [Header("Model")]
        public Transform model;
        public Mesh mesh;
        public Vector3 centre;
        public Vector3 size;
        public float xy, dhRate, hRate;

#if UNITY_EDITOR
        public void Reset()
        {
            Vector3 now = transform.position;
            transform.position = Vector3.zero;
            // 若模型有子物件，生成一個合併的Mesh，並用於後續計算使用
            model = transform.GetChild(0).GetChild(0); // 2 = Painting I
            model.localPosition = Vector3.zero;
            MeshFilter[] mfs = model.GetComponentsInChildren<MeshFilter>();
            if (mfs.Length > 1)
            {
                mesh = CombineMesh(model.gameObject);
                centre = mesh.bounds.center;
                size = mesh.bounds.size;
            }
            else
            {
                mesh = model.GetComponentInChildren<MeshFilter>().sharedMesh;
                if (model.childCount > 0) // 適用已修正錨點在基底的單一物件，例如Capsule、Box
                {
                    size = new Vector3(
                        mesh.bounds.size.x * mfs[0].transform.localScale.x,
                        mesh.bounds.size.y * mfs[0].transform.localScale.y,
                        mesh.bounds.size.z * mfs[0].transform.localScale.z);
                    centre = new Vector3(0, 0.5f * size.y, 0);
                }
                else
                {
                    centre = mesh.bounds.center;
                    size = mesh.bounds.size;
                }
            }
            AlignBase();
            Debug.Log("<color=lime>" + name + " data has been preset.</color>");
            transform.position = now;
        }

        /* 網格合併 */
        public Mesh CombineMesh(GameObject obj)
        {
            string MESH_PATH = "Assets/_iLYuSha_Mod/Design/Meshes/";
            string tempPath = MESH_PATH + obj.name + "_mesh.asset";
            if (obj.tag == "Dubi")
            {
                Mesh dubi = AssetDatabase.LoadAssetAtPath<Mesh>(MESH_PATH + "dubi_mesh.asset");
                return dubi;
            }
            else if (obj.tag == "Bear")
            {
                Mesh bear = AssetDatabase.LoadAssetAtPath<Mesh>(MESH_PATH + "bear_mesh.asset");
                return bear;
            }

            Mesh source = AssetDatabase.LoadAssetAtPath<Mesh>(tempPath);
            if (source)
                return source;

            if (obj.GetComponent<MeshRenderer>() == null)
            {
                obj.AddComponent<MeshRenderer>();
            }
            if (obj.GetComponent<MeshFilter>() == null)
            {
                obj.AddComponent<MeshFilter>();
            }

            List<Material> material = new List<Material>();
            Matrix4x4 matrix = obj.transform.worldToLocalMatrix;
            MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();
            int filterLength = filters.Length;
            CombineInstance[] combine = new CombineInstance[filterLength];
            for (int i = 0; i < filterLength; i++)
            {
                MeshFilter filter = filters[i];
                MeshRenderer render = filter.GetComponent<MeshRenderer>();
                if (render == null)
                {
                    continue;
                }
                if (render.sharedMaterial != null && !material.Contains(render.sharedMaterial))
                {
                    material.Add(render.sharedMaterial);
                }
                combine[i].mesh = filter.sharedMesh;
                //对坐标系施加变换的方法是 当前对象和子对象在世界空间中的矩阵 左乘 当前对象从世界空间转换为本地空间的变换矩阵
                //得到当前对象和子对象在本地空间的矩阵。
                combine[i].transform = matrix * filter.transform.localToWorldMatrix;
                // render.enabled = false;
            }

            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            mesh.name = "Combine";
            //合并Mesh
            mesh.CombineMeshes(combine);
            meshFilter.sharedMesh = mesh;
            //合并第二套UV
            Unwrapping.GenerateSecondaryUVSet(meshFilter.sharedMesh);
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            renderer.sharedMaterials = material.ToArray();
            renderer.enabled = true;

            MeshCollider collider = new MeshCollider();
            if (collider != null)
            {
                collider.sharedMesh = mesh;
            }
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, tempPath);
            //PrefabUtility.DisconnectPrefabInstance(obj);
            Mesh target = meshFilter.sharedMesh;
            DestroyImmediate(obj.GetComponent<MeshFilter>(), true);
            DestroyImmediate(obj.GetComponent<MeshRenderer>(), true);
            return target;
        }
#endif
        /* 對齊質心 */
        public void AlignCentre()
        {
            transform.GetChild(0).localPosition = -centre;
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = -new Vector3(0, 0.5f * size.y, 0);
            }
        }
        /* 對齊底座 */
        public void AlignBase()
        {
            transform.GetChild(0).localPosition = -centre + new Vector3(0, 0.5f * size.y, 0);
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = Vector3.zero;
            }
        }
        public void FreeLookSetting()
        {
            float wingspan = size.x;
            float length = size.z;
            float height = size.y;
            float max = wingspan > length ? (wingspan > height ? wingspan : height) : (length > height ? length : height);
            float wingspanScale = wingspan / max;
            float lengthScale = length / max;
            float heightScale = height / max;
            float orthoSize = max * 0.5f;
            float near = orthoSize + 2.7f;
            float far = orthoSize + 19.3f;

            float xyMax = Mathf.Sqrt(wingspan * wingspan + length * length);
            float radius = xyMax * 1.35f;
            float top = radius / Mathf.Sqrt(3);
            xy = xyMax;
            dhRate = xy / height;
            hRate = top / (height * 0.5f);

            CinemachineFreeLook cmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
            if (cmFreeLook == null)
            {
                GameObject cm;
                cm = new GameObject("CM FreeLook");
                cm.SetActive(false);
                cm.AddComponent<CinemachineFreeLook>();
                cm.transform.SetParent(transform);
                cm.SetActive(true);
                cmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
                // cmFreeLook.enabled = false;
            }
            cmFreeLook.gameObject.name = "CM FreeLook";
            cmFreeLook.enabled = true;
            cmFreeLook.Follow = transform;
            cmFreeLook.LookAt = transform;
            cmFreeLook.m_Lens.FieldOfView = 60;
            cmFreeLook.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            cmFreeLook.m_Orbits[0].m_Height = top; // orthoSize + 3; //sizeView.Height * 0.5f + 5;
            cmFreeLook.m_Orbits[1].m_Height = 0;
            cmFreeLook.m_Orbits[2].m_Height = -orthoSize - 3; //-sizeView.Height;
            cmFreeLook.m_Orbits[0].m_Radius = radius; //11; //sizeView.NearView + 2;
            cmFreeLook.m_Orbits[1].m_Radius = 11; //sizeView.HalfSize + 15;
            cmFreeLook.m_Orbits[2].m_Radius = 11; //sizeView.NearView + 1;
            cmFreeLook.m_Heading.m_Bias = Random.Range(-180, 180);
            cmFreeLook.m_YAxis.Value = 1.0f;
            cmFreeLook.m_XAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_YAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_XAxis.m_InvertInput = false;
            cmFreeLook.m_YAxis.m_InvertInput = true;
            cmFreeLook.enabled = false;
        }
        public void FreeLookSetting(CinemachineFreeLook cinemachine)
        {
            float wingspan = size.x;
            float length = size.z;
            float height = size.y;
            float max = wingspan > length ? (wingspan > height ? wingspan : height) : (length > height ? length : height);
            float wingspanScale = wingspan / max;
            float lengthScale = length / max;
            float heightScale = height / max;
            float orthoSize = max * 0.5f;
            float near = orthoSize + 2.7f;
            float far = orthoSize + 19.3f;

            float xyMax = Mathf.Sqrt(wingspan * wingspan + length * length);
            float radius = xyMax * 1.35f;
            float top = radius / Mathf.Sqrt(3);
            xy = xyMax;
            dhRate = xy / height;
            hRate = top / (height * 0.5f);

            cinemachine.enabled = true;
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
            cinemachine.m_Lens.FieldOfView = 60;
            cinemachine.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            cinemachine.m_Orbits[0].m_Height = top; // orthoSize + 3; //sizeView.Height * 0.5f + 5;
            cinemachine.m_Orbits[1].m_Height = 0;
            cinemachine.m_Orbits[2].m_Height = -orthoSize - 3; //-sizeView.Height;
            cinemachine.m_Orbits[0].m_Radius = radius; //11; //sizeView.NearView + 2;
            cinemachine.m_Orbits[1].m_Radius = 11; //sizeView.HalfSize + 15;
            cinemachine.m_Orbits[2].m_Radius = 11; //sizeView.NearView + 1;
            cinemachine.m_Heading.m_Bias = Random.Range(-180, 180);
            cinemachine.m_YAxis.Value = 1.0f;
            cinemachine.m_XAxis.m_InputAxisName = string.Empty;
            cinemachine.m_YAxis.m_InputAxisName = string.Empty;
            cinemachine.m_XAxis.m_InvertInput = false;
            cinemachine.m_YAxis.m_InvertInput = true;
        }
        public float GetScalePower()
        {
            // float max = Mathf.Max(0, size.x);
            // max = Mathf.Max(max, size.y);
            // max = Mathf.Max(max, size.z);
            float max = Mathf.Sqrt(size.x * size.z);
            Debug.Log(max);
            return 12.0f / max;
        }
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Prototype))]
    public class ProtodesignEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var scripts = targets.OfType<Prototype>();
            if (GUILayout.Button("Rebuild"))
                foreach (var script in scripts)
                {
                    script.Reset();
                    // script.FreeLookSetting ();
                }

            if (GUILayout.Button("Align Centre"))
                foreach (var script in scripts)
                    script.AlignCentre();
            if (GUILayout.Button("Align Base"))
                foreach (var script in scripts)
                    script.AlignBase();
            if (GUILayout.Button("Free Look"))
                foreach (var script in scripts)
                    script.FreeLookSetting();
        }
    }
#endif
}