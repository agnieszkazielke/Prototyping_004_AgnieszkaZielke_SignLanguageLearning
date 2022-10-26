using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusController : ActionBasedController
{
    [SerializeField] OVRHand ovrHand;
    [SerializeField] private Logger _screenDebug;

    protected override void UpdateInput(XRControllerState controllerState)
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.Hands))
        {
            controllerState.selectInteractionState.SetFrameState(ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index));
            
            //controllerState.selectInteractionState.SetFrameState(ovrHand.PointerPose);
            //_screenDebug.LogInfo("Index pinching on!");
        }
        else
        {
            base.UpdateTrackingInput(controllerState);
        }
    }
}
