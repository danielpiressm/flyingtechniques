using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthGrayScale : MonoBehaviour {

    Vector3[] positions;
    public float[] radiuses;
    public float[] intensities;

    public Material mat;

	// Use this for initialization
	void Start () {
        int M = 256;
        int N = 256;

        positions = new Vector3[M*N];

        for(int i = 0; i < M;i++)
        {
            for(int j = 0; j < N;j++)
            {
                positions[j + N * i] = new Vector3(2,2,2);
            }
        }

        for(int i = 0;i < 9;i++)
        {
            mat.SetInt("_Point" + i.ToString(),2);
        }


        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        mat.SetInt("_Points_Length", positions.Length);
        for(int i = 0; i < positions.Length; i++)
        {
            mat.SetVector("_Points" + i.ToString(), positions[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("x"))
        {
            string str = "Array = [";
            for(int i = 0; i < mat.GetInt("_Points_Length");i++)
            {
                Vector4 point = mat.GetVector("_Points" + i.ToString());
                positions[i] = new Vector3(point.x, point.y, point.z);
            }
            //Vector4[] pointts = mat.GetVector("_Points");
            int x = 2;
            //int z = 2;
            for(int i = 0;i < 9; i++)
            {
                x = mat.GetInt("_Point" + i);
            }
            mat.SetInt("_Point1", 3);
            Debug.Log("BLA = " + mat.GetInt("_Point1"));
        }
        
		//mat.GetVector("_Points", )
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
