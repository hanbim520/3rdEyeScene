﻿using UnityEngine;

namespace UI
{
  public class TextScrollView : MonoBehaviour
  {
    public int MaxLines
    {
      get { return _maxLines; }
      set { _maxLines = value; }
    }

    public RectTransform TextItem
    {
      get { return _textItem; }
    }

    public UnityEngine.UI.ScrollRect ScrollView
    {
      get
      {
        if (_scrollView == null)
        {
          _scrollView = GetComponentInChildren<UnityEngine.UI.ScrollRect>();
        }
        return _scrollView;
      }
    }

    public virtual void Append(string message)
    {
      RectTransform newItem = GameObject.Instantiate(_textItem);
      UnityEngine.UI.Text uiText = newItem.GetComponentInChildren<UnityEngine.UI.Text>();
      if (uiText)
      {
        uiText.text = message;
        Append(newItem);
      }
    }

    protected void Append(RectTransform newItem)
    {
      if (_maxLines > 0)
      {
        while (ScrollView.content.transform.childCount >= _maxLines)
        {
          Transform child = ScrollView.content.transform.GetChild(0);
          child.SetParent(null);
          Destroy(child.gameObject);
        }
      }

      if (newItem.gameObject.activeSelf)
      {
        ScrollView.AppendContentV(newItem.gameObject);
        if (_atEnd)
        {
          ScrollToBottom();
        }
      }
      else
      {
        newItem.SetParent(ScrollView.content.transform);
      }
    }

    public void ScrollToBottom()
    {
      _suppressEvents = true;
      try
      {
        RectTransform scrollContentRect = ScrollView.content.transform as RectTransform;
        RectTransform scrollRect = ScrollView.transform as RectTransform;
        Vector2 pos = scrollContentRect.anchoredPosition;
        if (scrollContentRect.sizeDelta.y > scrollRect.sizeDelta.y)
        {
          pos.y = scrollContentRect.sizeDelta.y - scrollRect.sizeDelta.y;
          scrollContentRect.anchoredPosition = pos;
        }
        _atEnd = true;
      }
      finally
      {
        _suppressEvents = false;
      }
    }

    public void Clear()
    {
      RectTransform scrollContenctRect = ScrollView.content.transform as RectTransform;
      Transform child;
      for (int i = scrollContenctRect.childCount - 1; i >= 0; --i)
      {
        child = scrollContenctRect.GetChild(i);
        child.SetParent(null);
        Destroy(child.gameObject);
      }
    }

    public void OnViewPosChange(Vector2 pos)
    {
      if (!_suppressEvents)
      {
        RectTransform scrollContenctRect = ScrollView.content.transform as RectTransform;
        _atEnd = scrollContenctRect.position.y >= scrollContenctRect.sizeDelta.y;
      }
    }

    void Start()
    {
      // Use the property to resolve the _scrollView.
      ScrollView.onValueChanged.RemoveListener(OnViewPosChange);
      ScrollView.onValueChanged.AddListener(OnViewPosChange);
    }

    [SerializeField]
    private RectTransform _textItem = null;
    private UnityEngine.UI.ScrollRect _scrollView = null;
    [SerializeField]
    private int _maxLines = 0;
    private bool _atEnd = true;
    private bool _suppressEvents = false;
  }
}
