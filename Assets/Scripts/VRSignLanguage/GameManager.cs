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
    }


    private void DelaySelectNextSign()
    {
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
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
        ResetSkeletonDebug();
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    // maybe move to be in DelaySelectNext Sign???
    private void ResetSkeletonDebug()
    {
        //_shapeHandDebug.enabled = false;
        _handShapeVisualiser.gameObject.SetActive(false);
        _transformHandDebug.enabled = false;
        Invoke(nameof(ConfigureSkeletonDebug), 1.5f);

    }
    
    private void ConfigureSkeletonDebug()
    {
        //_shapeHandDebug.enabled = true;
        _handShapeVisualiser.gameObject.SetActive(true);
        _transformHandDebug.enabled = true;
        _shapeHandDebug._shapeRecognizerActiveState = _currentSign.GetComponent<ShapeRecognizerActiveState>();
        _transformHandDebug._transformRecognizerActiveState = _currentSign.GetComponent<TransformRecognizerActiveState>();
  
    }


    private void OnDestroy()
    {
  
    }
}
