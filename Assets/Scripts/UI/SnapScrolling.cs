using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range(1,50)]
    [Header("Controllers")]
    public int panelCount;
    [Range(0,500)]
    public int panelOffset;
    [Range(0f,20f)]
    public float snapSpeed;
    [Range(0f,20f)]
    public float scaleTime;
    [Range(0f,20f)]
    public float scaleOffset;
    
    [Header("Other objects")]
    public GameObject scrollPanelPrefab;

    public ScrollRect scrollRect;

    private GameObject[] instPans;
    private Vector2[] panelPositions;
    private Vector2[] panelsScale;

    private RectTransform _contentRect;
    private int _selectedPanelId;
    private bool isScrolling;
    private Vector2 _contentVector;

    private void Start()
    {
        instPans = new GameObject[panelCount];
        panelPositions = new Vector2[panelCount];
        panelsScale = new Vector2[panelCount];
        _contentRect = GetComponent<RectTransform>();
        
        for (int i = 0; i < panelCount; i++)
        {
            //spawning prefab on transform of the Content and will use local coordinates
            instPans[i] = Instantiate(scrollPanelPrefab, transform, false);
            if(i == 0) continue;

            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x +
                                                              scrollPanelPrefab.GetComponent<RectTransform>().sizeDelta.x +
                                                              panelOffset, instPans[i].transform.localPosition.y);
            panelPositions[i] = - instPans[i].transform.localPosition; // because content position extends to minus coorditates and prefabs go into plus
        }
    }

    private void FixedUpdate()
    {
        //Checking if panels are behind screen from either side - stop inertia and scrolling
        if (_contentRect.anchoredPosition.x >= panelPositions[0].x && !isScrolling ||
            _contentRect.anchoredPosition.x <= panelPositions[panelPositions.Length - 1].x && !isScrolling)
        {
            scrollRect.inertia = false;
        }

        float nearestPos = float.MaxValue;
        for (int i = 0; i < panelCount; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - panelPositions[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                _selectedPanelId = i;
            }

            float scale = Mathf.Clamp(1 / (distance / panelOffset) * scaleOffset, 0.5f, 1f);
            panelsScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleTime * Time.fixedDeltaTime);
            panelsScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleTime * Time.fixedDeltaTime);
            instPans[i].transform.localScale = panelsScale[i];
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) //if scroll velocity is low and not scrolling inertia is off
        {
            scrollRect.inertia = false;
        }

        if(isScrolling || scrollVelocity > 400) return;
        _contentVector.x =
            Mathf.SmoothStep(_contentRect.anchoredPosition.x, panelPositions[_selectedPanelId].x, snapSpeed * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentVector;
    }

    //Method that used in ScrollView event triggers
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if(scroll)
            scrollRect.inertia = true; //if scrolling inertia is on
    }
}
