using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.PoseDetection.Debug;
using Oculus.Interaction.Samples;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Learning = 0,
        Challenge = 1
    }

    public GameMode gameMode;
    [SerializeField] private List<GameObject> _signPosesRightHand = new List<GameObject>();
    [SerializeField] private HandShapeSkeletalDebugVisual _shapeHandDebug;
    [SerializeField] private TransformFeatureVectorDebugParentVisual _transformHandDebug;
    [SerializeField] private GameObject _handShapeVisualiser;
    [SerializeField] private Logger _debugLogger;
    [SerializeField] private CarouselView _displayView;
    [SerializeField] private TMP_Text _messageText;
    private int _signIndex = -1;
    private GameObject _currentSign;
    
    void Start()
    {
        if (gameMode == GameMode.Learning)
        {
            SelectNextSign();
            ResetSkeletonDebug();
            UpdateMessageText(" ");
        }
        
        
    }
    
    public void RestartScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    private void DelaySelectNextSign()
    {
        UpdateMessageText("You got it!");
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        ResetSkeletonDebug();
        Invoke(nameof(SelectNextSign), 1.5f);
    }
    
    public void UISelectNextSign()
    {
        UpdateMessageText(" ");
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        ResetSkeletonDebug();
        Invoke(nameof(SelectNextSign), 0.5f);
    }

    
    public void UISelectPreviousSign()
    {
        UpdateMessageText(" ");
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        ResetSkeletonDebug();
        Invoke(nameof(SelectPreviousSign), 0.5f);
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
        UpdateMessageText("Try this one!");
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    private void SelectPreviousSign()
    {
        _signIndex--;
        if (_signIndex < 0)
        {
            _signIndex = _signPosesRightHand.Count - 1;
            //_displayView.ScrollLeft();
        }

        _currentSign = _signPosesRightHand[_signIndex];
        _displayView.ScrollLeft();
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
        UpdateMessageText("Try this one!");
        _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
    }

    private void ResetSkeletonDebug()
    {
        _shapeHandDebug.RestartActiveState();
        _handShapeVisualiser.gameObject.SetActive(false);
        Invoke(nameof(ConfigureSkeletonDebug), 2.5f);

    }
    
    
    private void ConfigureSkeletonDebug()
    {
        
        _handShapeVisualiser.gameObject.SetActive(true);
        _shapeHandDebug._shapeRecognizerActiveState = _currentSign.GetComponent<ShapeRecognizerActiveState>(); 
        _transformHandDebug._transformRecognizerActiveState = _currentSign.GetComponent<TransformRecognizerActiveState>();
        _shapeHandDebug.ImportActiveState();
        _transformHandDebug.ImportActiveState();
        _debugLogger.LogInfo("Skeleton reconfigured");
  
    }

    private void UpdateMessageText(string text)
    {
        _messageText.text = text;
    }


    private void OnDestroy()
    {
  
    }
}
