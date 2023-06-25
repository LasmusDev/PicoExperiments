using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class EyeTrackingVerification : MonoBehaviour
{
    private Main main; // Main singleton

    // Positions
    public List<Transform> PositionList;
    private int positionIndex = 0; // Position Index for the red target
    // Targets
    private Transform gazePoint;
    private Transform target; // red target
    private GameObject completed; // GO for completed text message
    private GameObject welcome; // GO for welcome text message

    // Internals
    private Matrix4x4 matrix;
    float clickGuard = 0.0f;
    bool targetScaleReverse = false;
    public float targetThreshold = 0.1f; // threshold for single dots

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

        StartCoroutine(Delay());
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
            PXR_EyeTracking.GetHeadPosMatrix(out matrix);
            PXR_EyeTracking.GetCombineEyeGazePoint(out Vector3 Origin);
            PXR_EyeTracking.GetCombineEyeGazeVector(out Vector3 Direction);
            Vector3 DirectionAdjusted = Direction + main.EyeTrackingDirectionAdjustments[positionIndex];
            Vector3 OriginOffset = matrix.MultiplyPoint(Origin);
            Vector3 DirectionOffset = matrix.MultiplyVector(DirectionAdjusted);

            Ray ray = new Ray(OriginOffset, DirectionOffset);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 25))
            {
                gazePoint.gameObject.SetActive(true);
                gazePoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);
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
    IEnumerator Delay(int delay = 3)
    {
        yield return new WaitForSeconds(delay);
        welcome.SetActive(false);
    }
}