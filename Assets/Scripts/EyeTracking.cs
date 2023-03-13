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
        StartCoroutine(EyeRaycast(0.02f)); // default: 0.04 sec. = 24 FPS
    }

    IEnumerator EyeRaycast(float steptime)
    {
        while (true)
        {
            if (Camera.main)
            {
                matrix = Matrix4x4.TRS(Camera.main.transform.position, Camera.main.transform.rotation, Vector3.one);
            }
            else
            {
                matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            }
            bool result = (PXR_EyeTracking.GetCombineEyeGazePoint(out Vector3 Origin) && PXR_EyeTracking.GetCombineEyeGazeVector(out Vector3 Direction));
            PXR_EyeTracking.GetCombineEyeGazePoint(out Origin);
            PXR_EyeTracking.GetCombineEyeGazeVector(out Direction);
            var OriginOffset = matrix.MultiplyPoint(Origin);
            var DirectionOffset = matrix.MultiplyVector(Direction);

            var positionIndex = 0; // default position

            RaycastHit hit;
            Ray ray = new Ray(OriginOffset, Direction); // unadjusted hit
            if (Physics.Raycast(ray, out hit, 200))
            {
                // Determine position index
                // see Calibration Routine List
                // 0 center, 1 top right, 2 Top Left, 3 Bottom right 4 bottom left
                // if gaze is approx. within circle radius in the middle of the screen (delta between origin and unadjusted cast)

            }


            var DirectionAdjusted = Direction + main.EyeTrackingDirectionAdjustments[positionIndex];
            if (result)
            {
                ray = new Ray(OriginOffset, DirectionAdjusted);
                if (Physics.Raycast(ray, out hit, 200))
                {
                    if (ShowGazePoint)
                    {
                        gazePoint.gameObject.SetActive(true);
                        gazePoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);
                    }
                }
                else
                {
                    gazePoint.gameObject.SetActive(false);
                }
                // Invoke logging event
                OnEyeTrackingEvent?.Invoke(OriginOffset, DirectionOffset, hit);
            }
            yield return new WaitForSeconds(steptime);
        }
    }
}