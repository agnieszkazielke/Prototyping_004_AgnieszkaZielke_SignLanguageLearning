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
    
    [Header("General")]
    [SerializeField] private List<GameObject> _signPosesRightHand = new List<GameObject>();
    [SerializeField] private HandShapeSkeletalDebugVisual _shapeHandDebug;
    [SerializeField] private TransformFeatureVectorDebugParentVisual _transformHandDebug;
    [SerializeField] private GameObject _handShapeVisualiser;
    [SerializeField, Optional] private Logger _debugLogger;
    [SerializeField, Optional] private TMP_Text _messageText;
    
    [Header("Learning")]
    [SerializeField, Optional] private CarouselView _displayView;
    private int _signIndex = -1;
    private GameObject _currentSign;


    [Header("Challenge")] 
    [SerializeField, Optional] private List<GameObject> _challengeWords = new List<GameObject>();
    [SerializeField, Optional] private ChallengeSpellingWord _challengeWord;
    [SerializeField, Optional] private TMP_Text _challengeSignText;
    private int _wordIndex = -1;
    
    
    void Start()
    {
        if (gameMode == GameMode.Learning)
        {
            SelectNextSign();
            ResetSkeletonDebug();
            UpdateMessageText(" ");
        }
        
        else if (gameMode == GameMode.Challenge)
        {
            SelectNextChallengeWord();
            ResetSkeletonDebug();
            UpdateMessageText("Spell this word!");
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
        UpdateMessageText("Spell this word!");
        _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.RemoveAllListeners();
        ResetSkeletonDebug();
        Invoke(nameof(SelectPreviousSign), 0.5f);
    }

    private void SelectNextChallengeWord()
    {
        _wordIndex++;
        if (_wordIndex >= _challengeWords.Count)
        {
            _wordIndex = 0;
        }
        _challengeWord = _challengeWords[_wordIndex].GetComponent<ChallengeSpellingWord>();
        SelectNextSign();
    }
    
    private void SelectNextSign()
    {
        _signIndex++;

        if (gameMode == GameMode.Learning)
        {
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

        else if (gameMode == GameMode.Challenge)
        {
            if (_signIndex >= _challengeWord._signPosesSequence.Count)
            {
                _signIndex = -1;
                UpdateMessageText("You cracked the word!");
                Invoke(nameof(SelectNextChallengeWord), 1.5f);
                
                return;
                
            }
            _currentSign = _challengeWord._signPosesSequence[_signIndex];
            _currentSign.GetComponent<SelectorUnityEventWrapper>().WhenSelected.AddListener(DelaySelectNextSign);
            
            string name = _challengeWord.gameObject.name;
            char letter = name[_signIndex];

            _challengeSignText.text = name;
            UpdateMessageText(letter.ToString());
            _debugLogger.LogInfo("Current sign: " + _currentSign.gameObject.name);
            
        }
        
        
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
