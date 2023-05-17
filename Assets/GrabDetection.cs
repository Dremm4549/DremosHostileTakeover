using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabDetection : XRGrabInteractable
{
    public XRDirectInteractor lHand;
    public XRDirectInteractor rHand;

    public string handType;

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        handType = interactor.tag;
        base.OnSelectEntered(interactor);
    }
}
