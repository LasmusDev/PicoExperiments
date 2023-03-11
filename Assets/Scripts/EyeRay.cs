using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;

public class EyeRay : MonoBehaviour
{
    private Main main; // Main singleton

    // Positions & Targets
    public List<Transform> PositionList = new();
    private Transform gazePoint;
    private Transform target;

    // Internals
    private Matrix4x4 matrix;
    private bool calibrate = false;
    private static int calibrationSampleCount = 9;
    private Vector3[] calibrationSamples = new Vector3[calibrationSampleCount];
    private int calibrationIdx = 0;
    int positionIndex = -1;
    bool nextPosition = true;

    // Logging
    public event EyeTrackingEvent OnEyeTrackingEvent;
    public delegate void EyeTrackingEvent(Vector3 origin, Vector3 direction, RaycastHit hit);

    void Start()
    {
        main = GameObject.Find("Main").GetComponent<Main>();
        target = GameObject.Find("target").transform;
        gazePoint = GameObject.Find("gazePoint").transform;
        StartCoroutine(EyeRaycast(0.02f)); // default: 0.04 sec. = 24 FPS
    }

    IEnumerator EyeRaycast(float steptime)
    {
        while (true)
        {
            // Position Target
            if (nextPosition)
            {
                if (positionIndex + 1 < PositionList.Count)
                {
                    positionIndex += 1;
                    main.EyeTrackingDirectionAdjustment[positionIndex] = Vector3.zero;
                    nextPosition = false;
                    target.SetParent(PositionList[positionIndex]);
                    target.localPosition = Vector3.zero;
                }
                else
                {
                    print("Calibration completed!");
                }
            }

            // Activate calibration with trigger press
            if (main.XR_GetKeyDown(XRNode.RightHand, CommonUsages.triggerButton) || main.XR_GetKeyDown(XRNode.LeftHand, CommonUsages.triggerButton))
            {
                calibrate = true;
                calibrationIdx = 0;
            }

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
            var DirectionAdjusted = Direction + main.EyeTrackingDirectionAdjustment[positionIndex];
            if (result)
            {
                Ray ray = new Ray(OriginOffset, DirectionAdjusted);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200))
                {
                    gazePoint.gameObject.SetActive(true);
                    gazePoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);

                    if (calibrate)
                    {
                        if (calibrationIdx < calibrationSampleCount)
                        {
                            var directionToTarget = (target.position - OriginOffset).normalized;

                            // Save sample and increase index
                            calibrationSamples[calibrationIdx] = directionToTarget;
                            calibrationIdx++;
                        }
                        else
                        {
                            Vector3 meanCalibrationSample = Vector3.zero;
                            // Get mean for adjustment samples
                            for (int i = 0; i < calibrationSamples.Length; i++)
                            {
                                meanCalibrationSample += calibrationSamples[i];
                            }
                            meanCalibrationSample /= calibrationSampleCount;

                            // Difference between mean "should be" direction and actual gaze direction.
                            main.EyeTrackingDirectionAdjustment[positionIndex] = meanCalibrationSample - Direction;
                            Debug.Log("TW: DirectionAdjustment " + main.EyeTrackingDirectionAdjustment);
                            calibrate = false;
                            nextPosition = true;
                        }
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