using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGaugemuki : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        // EnemyGaugeをMain Cameraへ向かせる
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
