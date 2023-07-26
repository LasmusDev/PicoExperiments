using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomShaderScreenPos : MonoBehaviour
{
    [SerializeField] private Material material;

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector3(pos.x / Screen.width, pos.y / Screen.height, pos.z);

        material.SetVector("_ObjectScreenPosition", pos);

    }
}
