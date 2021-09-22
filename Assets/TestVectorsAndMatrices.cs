using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectorsAndMatrices : MonoBehaviour
{
    public GameObject gov1, gov2;
    public Vector3 v1, v2;
    public float K= 1.5f;
    public GameObject goCubePrefab;

    public GameObject goCubev1, goCubev2;

    // Start is called before the first frame update
    void Start()
    {
        v1 = new Vector3(-1.0f, 2.5f, 1.5f);
        v2 = new Vector3(1.0f, -2.5f, +3.5f);

        

        Debug.Log("v1=" + v1);
        Debug.Log("v2=" + v2);

        gov1.transform.position = v1;
        gov2.transform.position = v2;

        goCubev1=Instantiate(goCubePrefab, v1, Quaternion.identity);
        goCubev2=Instantiate(goCubePrefab, v2, Quaternion.identity);

        goCubev1.name = "CubeV1";
        goCubev2.name = "CubeV2";

        Debug.Log(v1 + v2);
        Debug.Log(v2 + v1); //should be the same
        Debug.Assert(v1 + v2 == v2 + v1);

        Debug.Log(v2 - v1);
        Debug.Log(v1 - v2); //should be with opposite signs
        Debug.Assert(v1-v2 == (-1) * (v2-v1));

        Debug.Log(K*v1);
        Debug.Log(K*v2);

        float v1_v2 =Vector3.Dot(v1, v2);
        float v2_v1 = Vector3.Dot(v2, v1);
        Debug.Assert(v1_v2 == v2_v1);

        Vector3 v1xv2 = Vector3.Cross(v1, v2);
        Vector3 v2xv1 = Vector3.Cross(v2, v1);
        Debug.Assert(v1xv2 == (-1)*v2xv1);

        //linear interpolation
        // t0, t1  and two values y0,y1 =>
        // for t in [t0,t1] we can find y in [y0,y1] such that
        // trhe point (t,y) is in the line from (t0,y0) to (t1,y1)
        //how:
        // (t-t0)/(t1-t0)=(y-y0)/(y1-y0) =>
        // y =lerp(p0,p1,t)
        //   = y0+(t-t0)/(t1-t0)*(y1-y0)

        //If t0==0, and t1==1 =>
        //lerp01(y0,y1,t)=y0+t*(y1-y0)

        //lerp01(v0,v1,t)=v0+t*(v1-v0)=(1-t)*v0+t*v1

        float t = .25f;
        Vector3 v = v1 + t * (v2 - v1);
        //Vector3 v_m = Mathf.Lerp(v0 + t * (v1 - v0);
        Vector3 v_m = Vector3.Lerp(v1, v2, t); // 0 + t * (v1 - v0);
        Debug.Assert(v == v_m);


        Debug.Log(v1.magnitude);
        Debug.Log(v2.magnitude);
        //Vector3.

        // Heading_C_T =Target-Current;

        ////Lab at Home: Due before class next monday
        //Given two Vectors:
        //v1 = (1.5, 2.5, 3.5)  and v1 = (1.0, -3.5, 1.5)

        //and a constant

        //K = 0.75

        //Calculate the following quantities:

        //v1 + v2
        //v1 - v2

        //K* v1
        //K* v2

        //v1.v2(dot product)

        //v1 x v2(cross product)

        //| v1 | (magnitude or size of v1)
        //| v2 | (magnitude or size of v2)

        //angle(v1, v2)(the angle between v1 and v2: Hint: find y = cos(angle) first, then the angle with Mathf.acos(y))


        //From notes: cos(angle(v1, v2)) =v1.v2 / | v1 | *| v2 |

        //(Do this with both methods: this one and Mathf.Angle method and compare them).
















    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
