using System;
using UnityEngine;
using UnityEngine.UI;

public class AdjustedSnapScrolling : MonoBehaviour
{
    [Header("Controllers")]
    // [Range(1,50)]
    // public int panelCount;
    [Range(0,500)]
    public int panelOffset;
    [Range(0f,20f)]
    public float snapSpeed;
    [Range(0f,20f)]
    public float scaleTime;
    [Range(0f,20f)]
    public float scaleOffset;
    
    [Header("Other objects")]
    //public GameObject scrollPanelPrefab;

    public ScrollRect scrollRect;

    [SerializeField]
    private GameObject[] instPans;
    private Vector2[] _panelPositions;
    private Vector2[] _panelsScale;
    private Button[] _buttons;

    private RectTransform _contentRect;
    private int _selectedPanelId;
    private bool _isScrolling;
    private Vector2 _contentVector;


    private void Awake()
    {
        _panelPositions = new Vector2[instPans.Length];
        _panelsScale = new Vector2[instPans.Length];
        
        _buttons = new Button[instPans.Length];
        
        _contentRect = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        for (int i = 0; i < instPans.Length; i++)
        {
            //spawning prefab on transform of the Content and will use local coordinates
            instPans[i] = Instantiate(instPans[i], transform, false);
            if(i == 0) continue;

            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x +
                                                              instPans[i].GetComponent<RectTransform>().sizeDelta.x +
                                                              panelOffset, instPans[i].transform.localPosition.y);
            _panelPositions[i] = - instPans[i].transform.localPosition; // because content position extends to minus coordinates and prefabs go into plus
        }
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i] = instPans[i].GetComponent<Button>();
            _buttons[i].onClick.AddListener(delegate {EventBroker.CallChangeHat(_selectedPanelId);  });
        }
    }

    private void FixedUpdate()
    {
        //Checking if panels are behind screen from either side - stop inertia and scrolling
        if (_contentRect.anchoredPosition.x >= _panelPositions[0].x && !_isScrolling ||
            _contentRect.anchoredPosition.x <= _panelPositions[_panelPositions.Length - 1].x && !_isScrolling)
        {
            scrollRect.inertia = false;
        }

        float nearestPos = float.MaxValue;
        for (int i = 0; i < instPans.Length; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panelPositions[i].x);
            
            if (distance < nearestPos)
            {
                nearestPos = distance;
                _selectedPanelId = i;
            }

            float scale = Mathf.Clamp(1 / (distance / panelOffset) * scaleOffset, 0.5f, 1f);
            _panelsScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleTime * Time.fixedDeltaTime);
            _panelsScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleTime * Time.fixedDeltaTime);
            instPans[i].transform.localScale = _panelsScale[i];
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !_isScrolling) //if scroll velocity is low and not scrolling inertia is off
        {
            scrollRect.inertia = false;
        }

        if(_isScrolling || scrollVelocity > 400) return;
        _contentVector.x =
            Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelPositions[_selectedPanelId].x, snapSpeed * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentVector;
    }

    //Method that used in ScrollView event triggers
    public void Scrolling(bool scroll)
    {
        _isScrolling = scroll;
        if(scroll)
            scrollRect.inertia = true; //if scrolling inertia is on
    }
}
