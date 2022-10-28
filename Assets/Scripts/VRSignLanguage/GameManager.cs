using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.PoseDetection.Debug;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _signPosesRightHand = new List<GameObject>();
    [SerializeField] private HandShapeSkeletalDebugVisual _shapeHandDebug;
    [SerializeField] private TransformFeatureVectorDebugParentVisual _transformHandDebug;
    [SerializeField] private CustomPoseRecognizer _poseChecker;
    [SerializeField] private Logger _debugLogger;
    private int _signIndex;
    private GameObject _currentSign;
    
    void Start()
    {
        SetFirstSign();
        //_poseChecker.onPoseRecognised.AddListener(ConfigureSkeletonDebug);
        
    }

    private void SetFirstSign()
    {
        _signIndex = 0;
        _currentSign = _signPosesRightHand[_signIndex];
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
        ResetSkeletonDebug();
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    private void DelaySelectNextSign()
    {
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        Invoke(nameof(SelectNextSign), 3.0f);
    }
    
    private void SelectNextSign()
    {
        
        _signIndex++;
        _currentSign = _signPosesRightHand[_signIndex];
        ResetSkeletonDebug();
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    private void ResetSkeletonDebug()
    {
        _shapeHandDebug.enabled = false;
        _transformHandDebug.enabled = false;
        Invoke(nameof(ConfigureSkeletonDebug), 1f);

    }
    
    private void ConfigureSkeletonDebug()
    {
        _shapeHandDebug.enabled = true;
        _transformHandDebug.enabled = true;
        _shapeHandDebug._shapeRecognizerActiveState = _currentSign.GetComponent<ShapeRecognizerActiveState>();
        _transformHandDebug._transformRecognizerActiveState = _currentSign.GetComponent<TransformRecognizerActiveState>();
        
        
        
        //_poseChecker._transformRecognizerActiveStates.Clear();
        //_poseChecker._transformRecognizerActiveStates.Add(_signPosesRightHand[_signIndex].GetComponent<TransformRecognizerActiveState>()); 
         
        //_poseChecker.ResetRecognizerStatus();

    }


    private void OnDestroy()
    {
        _poseChecker.onPoseRecognised.RemoveAllListeners();
    }
}
