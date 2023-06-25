using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class EyeTrackingCalibration : MonoBehaviour
{
    private Main main; // Main singleton

    // Positions
    public List<Transform> PositionList;
    private int positionIndex = 0; // Position Index for the red target
    bool nextPosition = false;
    // Targets
    private Transform gazePoint;
    private Transform target; // red target
    private GameObject completed; // GO for completed text message
    private GameObject welcome; // GO for welcome text message

    // Internals
    private Matrix4x4 matrix;
    private bool calibrate = false;
    private static int calibrationSampleCount = 50;
    private Vector3[] calibrationSamples = new Vector3[calibrationSampleCount];
    private int calibrationIdx = 0; // sample index in the array
    float clickGuard = 0.0f;
    bool targetScaleReverse = false;
    public float targetThreshold = 0.05f; // threshold for single dots

    void Start()
    {
        main = GameObject.Find("Main").GetComponent<Main>();
        target = GameObject.Find("target").transform;
        gazePoint = GameObject.Find("gazePoint").transform;
        gazePoint.gameObject.SetActive(false);

        welcome = GameObject.Find("Welcome");
        completed = GameObject.Find("Completed");
        completed.SetActive(false);

        // initialize first index position
        main.EyeTrackingDirectionAdjustments.Add(Vector3.zero);
        target.SetParent(PositionList[positionIndex]);
        target.localPosition = Vector3.zero;

        StartCoroutine(DelayCalibration());
        StartCoroutine(EyeRaycast(0.01f)); // default: 0.04 sec. = 24 FPS
    }
    void Update()
    {
        if (clickGuard > 0)
            clickGuard -= Time.deltaTime;

        // shrink and grow target
        if (targetScaleReverse && target.localScale.x > 1)
            target.localScale *= .98f;
        else if (targetScaleReverse && target.localScale.x <= 1)
            targetScaleReverse = false;
        else if (!targetScaleReverse && target.localScale.x < 2)
            target.localScale *= 1.02f;
        else if (!targetScaleReverse && target.localScale.x >= 2)
            targetScaleReverse = true;
    }

    IEnumerator EyeRaycast(float steptime)
    {
        while (true)
        {

            if (nextPosition) // Jump to next position
            {
                nextPosition = false;
                if (positionIndex + 1 < PositionList.Count) // Check if position left
                {
                    positionIndex += 1;
                    main.EyeTrackingDirectionAdjustments.Add(Vector3.zero);
                    target.SetParent(PositionList[positionIndex]);
                    target.localPosition = Vector3.zero;
                }
                else // Show end message and exit
                {
                    Debug.Log("Calibration completed!");
                    calibrate = false;
                    completed.SetActive(true);
                    StartCoroutine(GoMain());
                }
            }

            PXR_EyeTracking.GetHeadPosMatrix(out matrix);
            PXR_EyeTracking.GetCombineEyeGazePoint(out Vector3 Origin);
            PXR_EyeTracking.GetCombineEyeGazeVector(out Vector3 Direction);
            Vector3 DirectionAdjusted = Direction + main.EyeTrackingDirectionAdjustments[positionIndex];
            Vector3 OriginOffset = matrix.MultiplyPoint(Origin);
            Vector3 DirectionOffset = matrix.MultiplyVector(DirectionAdjusted);

            Ray ray = new Ray(OriginOffset, DirectionOffset);
            RaycastHit hit;
            if (calibrate && Physics.Raycast(ray, out hit, 25))
            {
                gazePoint.gameObject.SetActive(true);
                gazePoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);

                if (calibrationIdx < calibrationSampleCount)
                {
                    var directionToTarget = (target.position - OriginOffset).normalized; // should be
                    var directionToGazepoint = (hit.point - OriginOffset).normalized; // is

                    // Save sample
                    calibrationSamples[calibrationIdx] = directionToTarget - directionToGazepoint;
                    calibrationIdx++;
                }
                else
                {
                    Vector3 meanCalibrationSample = Vector3.zero;
                    // Get mean for adjustment samples
                    for (int i = 0; i < calibrationSamples.Length; i++)
                        meanCalibrationSample += calibrationSamples[i];
                    meanCalibrationSample /= calibrationSampleCount;

                    // To calculate difference between last adjustment and current adjustment
                    var lastAdjustment = main.EyeTrackingDirectionAdjustments[positionIndex];

                    main.EyeTrackingDirectionAdjustments[positionIndex] += meanCalibrationSample;

                    // Debug.Log("TW: DirectionAdjustment " + main.EyeTrackingDirectionAdjustments[positionIndex]);
                    var deltaDirectionAdjustment = (main.EyeTrackingDirectionAdjustments[positionIndex] - lastAdjustment).magnitude;
                    // Debug.Log("TW: DeltaDirectionAdjustment " + deltaDirectionAdjustment);

                    // Difference between mean "should be" direction and actual gaze direction.
                    var deltaShouldIs = (PositionList[positionIndex].position - gazePoint.position).magnitude;
                    // Debug.Log("TW: DeltaShouldIs " + deltaShouldIs);

                    if ((deltaShouldIs < targetThreshold) && (deltaDirectionAdjustment < targetThreshold))
                        nextPosition = true;
                    else
                        calibrationIdx = 0; // (re-)set calibration sample count
                }
            }
            else
            {
                gazePoint.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(steptime);
        }

    }
    IEnumerator GoMain(int sceneIndex = 0, int delay = 3)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
    IEnumerator DelayCalibration(int delay = 3)
    {
        yield return new WaitForSeconds(delay);
        welcome.SetActive(false);
        calibrate = true;
    }
}