using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType
{
    left,
    Right
}

public class HandController : MonoBehaviour
{

    public HandType handType;
    private Animator animator;
    private InputDevice device;
 
    public float thumbMoveSpeed = 0.1f;
    float indexValue;
    float threeFingerValue;
    float thumbValue;

    [SerializeField] uint motorChannel;
    [SerializeField] float amplitude;
    [SerializeField] float vibrationDuration;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        device = GetInputDevice();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandType.left)
        {
            controllerCharacteristics = controllerCharacteristics | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristics = controllerCharacteristics | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        return inputDevices[0];
    }

    public void AnimateHand()
    {
        device.TryGetFeatureValue(CommonUsages.trigger, out indexValue);
        device.TryGetFeatureValue(CommonUsages.grip, out threeFingerValue);
        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primaryTouched);
        device.TryGetFeatureValue(CommonUsages.secondary2DAxisClick, out bool secondaryTouched);


        if (primaryTouched || secondaryTouched)
        {
            thumbValue += thumbMoveSpeed;
        }
        else
        {
            thumbValue -= thumbMoveSpeed;
        }


        thumbValue = Mathf.Clamp(thumbValue, 0, 1);

        animator.SetFloat("Index", indexValue);
        animator.SetFloat("ThreeFingers", threeFingerValue);
        animator.SetFloat("Thumb", thumbValue);

        
        
    }

    public void SendImpulse(string handType)
    {
        Debug.Log("send impluse in " + handType);

        if (handType == "ShootLeft")
        {
            amplitude = 0.7f;
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(motorChannel, amplitude, vibrationDuration);
        }
        else if (handType == "ShootRight")
        {
            Debug.Log("dsada");
            amplitude = 0.7f;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(1, amplitude, vibrationDuration);
        }
    }
}
