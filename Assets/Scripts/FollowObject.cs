using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool localSpace;
    public Transform transformToFollow;
    public bool autoCalcOffset;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public bool posFollow = true;
    public bool rotFollow = true;
    // Start is called before the first frame update
    void Start()
    {
        if(autoCalcOffset)
        {
            if (localSpace)
            {
                positionOffset = transformToFollow.localPosition - this.transform.localPosition;
                rotationOffset = transformToFollow.localRotation.eulerAngles - this.transform.localRotation.eulerAngles;
            }
            else
            {
                positionOffset = transformToFollow.position - this.transform.position;
                rotationOffset = transformToFollow.rotation.eulerAngles - this.transform.rotation.eulerAngles;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transformToFollow == null)
        {
            return;
        }
        if(localSpace)
        {
            if (posFollow)
            {
                this.transform.localPosition = transformToFollow.localPosition - positionOffset;
            }
            if (rotFollow)
            {
                this.transform.localRotation = Quaternion.Euler(transformToFollow.localRotation.eulerAngles - rotationOffset);
            }
        } else
        {
            if (posFollow)
            {
                this.transform.position = transformToFollow.position - positionOffset;
            }
            if (rotFollow)
            {
                this.transform.rotation = Quaternion.Euler(transformToFollow.rotation.eulerAngles - rotationOffset);
            }
        }
    }
}
