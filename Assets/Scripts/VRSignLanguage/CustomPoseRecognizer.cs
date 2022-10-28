using Oculus.Interaction.Input;
using System;
using Oculus.Interaction.PoseDetection.Debug;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class CustomPoseRecognizer : TransformRecognizerDebugVisual
{
    public bool poseRecognised = false;
    [SerializeField] private float _timeDelay = 1.25f;
    public UnityEvent onPoseRecognised;
    private float t;
    

    public void ResetRecognizerStatus()
    {
        poseRecognised = false;
        
    }

    private void DelayCheck()
    {
        bool delayCheck = AllActive();

        if (delayCheck)
        {
            onPoseRecognised.Invoke();
            ResetRecognizerStatus();
        }

        else
        {
            ResetRecognizerStatus();
        }
            
        
    }
    
    
    protected override void Update()
    {
        bool isActive = AllActive();

        if (isActive)
        {
            if (poseRecognised) return;
            poseRecognised = true;
            Invoke(nameof(DelayCheck), _timeDelay);
         
        }
      
    }
}
