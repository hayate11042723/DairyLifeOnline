using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePointerScript : MonoBehaviour
{
    [SerializeField]private float x,y,z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ワールドのy軸に沿って1秒間に90度回転
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime, Space.World);
    }
}
