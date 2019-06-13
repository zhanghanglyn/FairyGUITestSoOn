using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂在cube立方体上，会把所有碰撞到的模型的三角形保存并生成cube中的网格
public class MeshDecal : MonoBehaviour {

    private MeshFilter cubeMeshFilter;
    private MeshRenderer cubeMeshRenderer;

    //存储模型数据的各个列表
    List<Vector3> verticeList = new List<Vector3>();
    List<Vector2> UVList = new List<Vector2>();
    List<int> triangleList = new List<int>();
    List<Vector3> normalList = new List<Vector3>();

    public float PointDistanceThreshold = 0.0001f;
    public float ExpendThreshold = 0.0001f;

    public bool bUseZAsUV = false;
    public bool bTailor = false;

    private void Awake()
    {
        cubeMeshFilter = GetComponent<MeshFilter>();
        cubeMeshRenderer = GetComponent<MeshRenderer>();
    }

    //获取与cube碰撞的模型的三角形
    [ContextMenu("GetColliderMesh")]
    public void GetColliderMesh()
    {
        if(cubeMeshFilter == null)
            cubeMeshFilter = GetComponent<MeshFilter>();
        if (cubeMeshRenderer == null)
            cubeMeshRenderer = GetComponent<MeshRenderer>();

        MeshRenderer[] renders = FindObjectsOfType<MeshRenderer>();
        
        foreach ( MeshRenderer render in renders )
        {
            //自身不用判断
            if (render.GetComponent<MeshDecal>() != null)
                continue;

            //如果发生了碰撞
            if (cubeMeshRenderer.bounds.Intersects(render.bounds))
            {
                GenerateDecalMesh(render);
            }
        }

        SetVerticeExpend();
        GenerateUnityMesh();
    }

    //根据传入的MeshRenderer生成cube中的mesh数据
    public void GenerateDecalMesh( MeshRenderer _render )
    {
        Mesh render = _render.GetComponent<MeshFilter>().sharedMesh;
        if (render == null)
            return;

        Vector3[] meshVertices = render.vertices;   //点的坐标
        int[] meshTriangles = render.triangles;  //triangle中存储的是点连成三角形的顺序！

        //把碰撞模型的点坐标从模型空间转到该立方体的模型空间中
        Matrix4x4 colliderToCubeMatrix = cubeMeshRenderer.transform.worldToLocalMatrix * _render.transform.localToWorldMatrix;

        //把三角形连起来，每一次循环就相当于创建了一个三角形
        for (int i = 0; i < meshTriangles.Length; i = i+3)
        {
            int triangleOrder1 = meshTriangles[i];
            int triangleOrder2 = meshTriangles[i+1];
            int triangleOrder3 = meshTriangles[i+2];

            Vector3 triangle1 = meshVertices[triangleOrder1];
            Vector3 triangle2 = meshVertices[triangleOrder2];
            Vector3 triangle3 = meshVertices[triangleOrder3];
            triangle1 = colliderToCubeMatrix.MultiplyPoint(triangle1);
            triangle2 = colliderToCubeMatrix.MultiplyPoint(triangle2);
            triangle3 = colliderToCubeMatrix.MultiplyPoint(triangle3);


            //生成三角形UV，normal等信息,根据两个三角形的cross来计算normal
            Vector3 dir1 = triangle1 - triangle2;
            Vector3 dir2 = triangle1 - triangle3;
            Vector3 normal = Vector3.Cross(dir1, dir2);

            //转换后的空间坐标点
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(triangle1);
            vertices.Add(triangle2);
            vertices.Add(triangle3);

            //判断三角形是否在立方体中,讲该三角形加入列表
            if (bTailor == false)
            {
                if (CubeCollisionChecker.CheckBeInCube(vertices))
                    AddTriangle(vertices, normal);
            }else
            {
                if (CubeCollisionChecker.CheckBeInCubeTailor(vertices))
                {
                    if (vertices.Count > 2)
                        AddTriangle(vertices, normal);
                }
                    
            }
            

        }  
    }

    /// <summary>
    /// 将该三角形normal等数据写入list，triangle数据可以根据传入的_vertices重新生成
    /// </summary>
    /// <param name="_triangle"></param>
    /// <param name="_normal"></param>
    public void AddTriangle( List<Vector3> _vertices , Vector3 _normal)
    {
        //每一个循环相当于三角形的一个点
        /*for (int i = 0; i< _vertices.Count; i += 3 )
        {
            AddPolygon(_vertices[i], _normal);
            AddPolygon(_vertices[i+1], _normal);
            AddPolygon(_vertices[i+2], _normal);
        }*/
        int ind1 = AddPolygon(_vertices[0], _normal);

        for (int i = 1; i < _vertices.Count - 1; i++)
        {
            int ind2 = AddPolygon(_vertices[i], _normal);
            int ind3 = AddPolygon(_vertices[i + 1], _normal);

            triangleList.Add(ind1);
            triangleList.Add(ind2);
            triangleList.Add(ind3);
        }
    }

    public int AddPolygon( Vector3 vector , Vector3 _normal )
    {
        //判断改点是否已经存在,-1则为不存在
        int index = BeRepeatPoint(vector);
        if (index == -1)
        {
            int startIndex = verticeList.Count;
            verticeList.Add(vector);
            normalList.Add(_normal);
            //计算UV
            float u = Mathf.Lerp(0.0f, 1.0f, vector.x + 0.5f);
            float v = 0;
            if (bUseZAsUV == true)
                v = Mathf.Lerp(0.0f, 1.0f, vector.z + 0.5f);
            else
                v = Mathf.Lerp(0.0f, 1.0f, vector.y + 0.5f);
            UVList.Add(new Vector2(u, v));
            //triangleList.Add(startIndex);
            return startIndex;
        }
        else//小于阈值，认为是同一个点来处理,同一个点重新计算norma 
        {
            normalList[index] = (normalList[index] + _normal).normalized;
            //triangleList.Add(index);
            return index;
        }
    }

    /// <summary>
    /// 判断是否是重复点，如果是重复点，需要重新计算nomral
    /// 根据两点间的距离来判断，小于0.001就算是同一个点
    /// </summary>
    public int BeRepeatPoint( Vector3 _vector)
    {
        for (int i = 0; i< verticeList.Count;i++)
        {
            //小于阈值，认为是同一个点来处理,返回当前点的index
            if (Vector3.Distance(verticeList[i], _vector) < PointDistanceThreshold)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 设置每个点向外扩展一些
    /// </summary>
    public void SetVerticeExpend()
    {
        for(int i = 0;i < verticeList.Count; i++)
        {
            verticeList[i] += (normalList[i] * ExpendThreshold);
        }
    }

    /// <summary>
    /// 创建最终的模型
    /// </summary>
    public void GenerateUnityMesh()
    {
        Mesh unityMesh = new Mesh();
        unityMesh.Clear(true);

        unityMesh.vertices = verticeList.ToArray();
        unityMesh.triangles = triangleList.ToArray();
        unityMesh.uv = UVList.ToArray();
        unityMesh.normals = normalList.ToArray();

        verticeList.Clear();
        triangleList.Clear();
        UVList.Clear();
        normalList.Clear();

        cubeMeshFilter.sharedMesh = unityMesh;
    }

}

/// <summary>
/// 检测三角形是否在一个0.5,0.5的立方体中
/// </summary>
public class CubeCollisionChecker
{

    static Plane forwardPlane = new Plane(Vector3.forward, 0.5f);
    static Plane backPlane = new Plane(Vector3.back, 0.5f);
    static Plane leftPlane = new Plane(Vector3.left, 0.5f);
    static Plane rightPlane = new Plane(Vector3.right, 0.5f);
    static Plane upPlane = new Plane(Vector3.up, 0.5f);
    static Plane downPlane = new Plane(Vector3.down, 0.5f);

    /// <summary>
    /// 立方体每个面进行裁剪，判断三角形是否在面外,返回false代表没有碰撞，该三角形在平面里面，该函数没有三角裁剪
    /// </summary>
    /// <param name="_plane"></param>
    /// <param name="_vectorList"></param>
    /// <returns></returns>
    public static bool CheckCollision( Plane _plane,  List<Vector3> _vectorList )
    {
        int outSideCount = 0;
        for (int i = 0; i < _vectorList.Count; i++)
        {
            //判断一个点是否在平面的正侧(里面）
            if(_plane.GetSide(_vectorList[i]) == true)
            {
                outSideCount++;
                //break;
            }
        }

        //if (outSideCount > 0)
        //    return false;
        //return true;
        if (outSideCount == _vectorList.Count)
            return false;
        return true;
    }

    /// <summary>
    /// 进行了三角裁剪的判断，对在面外的三角形进行裁剪，返回在面内的三角形
    /// </summary>
    /// <param name="_plane"></param>
    /// <param name="_vectorList"></param>
    /// <returns></returns>
    public static void CheckCollisionTailor(Plane _plane,List<Vector3> _vectorList)
    {
        List<Vector3> newVertice = new List<Vector3>();
        for (int i = 0; i < _vectorList.Count; i++)
        {
            //取当前点和后一个点，因为如果当前点在里面，后一个点在外面，则需要裁剪，用向量
            int next_count = (i + 1) % _vectorList.Count;
            Vector3 v1 = _vectorList[i];
            Vector3 v2 = _vectorList[next_count];

            //如果v1在里面，直接加进去
            bool v1Result = _plane.GetSide(v1);
            if (v1Result == true)
            {
                newVertice.Add(v1);
            }
            
            //逆向判断下一个是否在内部！！！如果不相等，则一定有一个点在外部，
            if ( _plane.GetSide(v2) != v1Result )
            {
                float distance;
                Vector3 rayVec = v2 - v1;
                Ray ray = new Ray(v1, rayVec);
                _plane.Raycast(ray,out distance);

                Vector3 newPoint = ray.GetPoint(distance);
                newVertice.Add(newPoint);
            }
        }

        _vectorList.Clear();
        _vectorList.AddRange(newVertice);
    }

    public static bool CheckBeInCube(List<Vector3> _vectorList )
    {
        bool result = CheckCollision(forwardPlane, _vectorList);

        result |= CheckCollision(backPlane, _vectorList);
        result |= CheckCollision(leftPlane, _vectorList);
        result |= CheckCollision(rightPlane, _vectorList);
        result |= CheckCollision(upPlane, _vectorList);
        result |= CheckCollision(downPlane, _vectorList);

        return !result;
    }

    public static bool CheckBeInCubeTailor(List<Vector3> _vectorList)
    {
        CheckCollisionTailor(forwardPlane,_vectorList);
        CheckCollisionTailor(backPlane, _vectorList);
        CheckCollisionTailor(leftPlane, _vectorList);
        CheckCollisionTailor(rightPlane, _vectorList);
        CheckCollisionTailor(upPlane, _vectorList);
        CheckCollisionTailor(downPlane, _vectorList);

        return true;
    }

}