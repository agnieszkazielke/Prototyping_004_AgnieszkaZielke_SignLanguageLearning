using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.PoseDetection.Debug;
using Oculus.Interaction.Samples;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _signPosesRightHand = new List<GameObject>();
    [SerializeField] private HandShapeSkeletalDebugVisual _shapeHandDebug;
    [SerializeField] private TransformFeatureVectorDebugParentVisual _transformHandDebug;
    [SerializeField] private GameObject _handShapeVisualiser;
    [SerializeField] private Logger _debugLogger;
    [SerializeField] private CarouselView _displayView;
    private int _signIndex = -1;
    private GameObject _currentSign;
    
    void Start()
    {
        
        SelectNextSign();
        ResetSkeletonDebug();
        //_displayView.UpdateSignText(); 
    }


    private void DelaySelectNextSign()
    {
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        ResetSkeletonDebug();
        Invoke(nameof(SelectNextSign), 1.5f);
    }
    
    private void SelectNextSign()
    {
        _signIndex++;
        if (_signIndex >= _signPosesRightHand.Count)
        {
            _signIndex = 0;
            _displayView.ScrollRight();
        }

        _currentSign = _signPosesRightHand[_signIndex];
        _displayView.ScrollRight();
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    // maybe move to be in DelaySelectNext Sign???
    private void ResetSkeletonDebug()
    {
        _shapeHandDebug.RestartActiveState();
        _handShapeVisualiser.gameObject.SetActive(false);
        Invoke(nameof(ConfigureSkeletonDebug), 2.5f);

    }
    
    // Find the reason for one behid on the skeleton
    private void ConfigureSkeletonDebug()
    {
        
        _handShapeVisualiser.gameObject.SetActive(true);
        _shapeHandDebug._shapeRecognizerActiveState = _currentSign.GetComponent<ShapeRecognizerActiveState>(); 
        _transformHandDebug._transformRecognizerActiveState = _currentSign.GetComponent<TransformRecognizerActiveState>();
        _shapeHandDebug.ImportActiveState();
        _transformHandDebug.ImportActiveState();
        _debugLogger.LogInfo("Skeleton reconfigured");
  
    }


    private void OnDestroy()
    {
  
    }
}
