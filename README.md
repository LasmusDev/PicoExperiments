# EyeTracking  

- If you have any questions/comments, please visit [**Pico Developer Answers**](https://devanswers.pico-interactive.com/) and raise your question there.

## SDK Versions:
   
   - Pico Unity Integration SDK v2.0.5

## Unity Versions：

   - 2020.3.36f1 and later

## Description：

   - There are four demos in this project:

     ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/1.jpeg)

1.  **User Calibration**
Launch the Tobii User Calibration application.
2.  **Eye Tracking**
The scene is the coordinate system of the eyeball, which is used to view and test the accuracy of the eye-tracking in the coordinate system.

    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/2.png)
3. **Game Demo**
Eye tracking's being used in some game scenes, only for reference.
The scene can be switched by the controller trigger key or the headset "confirm" key.
    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/3.png)
    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/4.png)
    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/5.png)
4.  **Mirror**
    Mainly verify the eye-closing function of eye-tracking
    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/6.png)

##  Note:
With the relevant functions of eye-tracking, user calibration is preferred. The Tobii User Calibration application has been preset in the ROM of Pico Neo2 Eye devices and Pico Neo3 Eye devices, as shown in the red box below. At present, automatic recognition of eye calibration data is not supported, so this operation is required when changing users.
    ![ ](https://github.com/picoxr/Eye-Tracking-UnityXR/blob/main/Screenshots/7.jpeg)

How to enable eye-tracking, please refer to:

https://developer-global.pico-interactive.com/docs/unity/en/13136/unity-xr-sdk-EyeTracking/

For eye-tracking API, please refer to:

https://pdocor.pico-interactive.com/reference/unity/xr/2.05/class_unity_1_1_x_r_1_1_p_x_r_1_1_p_x_r___eye_tracking.html


The Pico Unity Integration SDK v2.0.5 needs to be imported manually. For details about the latest SDK download address and documentation, see the following links:

https://developer-global.pico-interactive.com/sdk?deviceId=1&platformId=1&itemId=12
