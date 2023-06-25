using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;

public class EyeTracking : MonoBehaviour
{
    private Main main; // Main singleton

    // Positions & Targets
    private Transform gazePoint;
    public bool ShowGazePoint = true;

    // Internals
    private Matrix4x4 matrix;

    // Logging
    public event EyeTrackingEvent OnEyeTrackingEvent;
    public delegate void EyeTrackingEvent(Vector3 origin, Vector3 direction, RaycastHit hit);

    void Start()
    {
        main = GameObject.Find("Main").GetComponent<Main>();
        gazePoint = GameObject.Find("gazePoint").transform;
        StartCoroutine(EyeRaycast(0.01f)); // default: 0.04 sec. = 24 FPS
    }

    IEnumerator EyeRaycast(float steptime)
    {
        while (true)
        {
#if !UNITY_EDITOR
            PXR_EyeTracking.GetHeadPosMatrix(out matrix);
            PXR_EyeTracking.GetCombineEyeGazePoint(out Vector3 Origin);
            PXR_EyeTracking.GetCombineEyeGazeVector(out Vector3 Direction);

            // Find Adjustment Index
            var positionIndex = 0; // default position
            var gazeVec = Origin + Direction;
            var forwardPt = Origin + Vector3.forward;
            var gazeDistance = (gazeVec - forwardPt).magnitude;
            if (gazeDistance > 0.25f)
            {
                if (gazeVec.x > 0 && gazeVec.y > 0) // top right
                    positionIndex = 1;
                else if (gazeVec.x < 0 && gazeVec.y > 0) // top left
                    positionIndex = 2;
                else if (gazeVec.x > 0 & gazeVec.y < 0) // bottom right
                    positionIndex = 3;
                else if (gazeVec.x < 0 & gazeVec.y < 0) // bottom left
                    positionIndex = 4;
            }
            var DirectionAdjusted = Direction + main.EyeTrackingDirectionAdjustments[positionIndex];

            var OriginOffset = matrix.MultiplyPoint(Origin);
            var DirectionOffset = matrix.MultiplyVector(DirectionAdjusted);


            RaycastHit hit;
            Ray ray = new Ray(OriginOffset, DirectionOffset);
            if (Physics.Raycast(ray, out hit, 20))
            {
                if (ShowGazePoint && gazePoint != null)
                {
                    gazePoint.gameObject.SetActive(true);
                    gazePoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);
                    print("hit distance " + hit.distance.ToString());
                }
                else
                {
                    gazePoint.gameObject.SetActive(false);
                }
            }
            else
            {
                gazePoint.gameObject.SetActive(false);
            }
            // Invoke logging event
            OnEyeTrackingEvent?.Invoke(OriginOffset, DirectionOffset, hit);
#endif
            yield return new WaitForSeconds(steptime);
        }
    }
}