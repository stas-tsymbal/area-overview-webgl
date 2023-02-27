using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Contains all additional ui objs and their management logic
 */

public class UiController : MonoBehaviour
{
    #region Signleton

    public static UiController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    [Serializable]
    public class OverlayUiSample
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject view;

        public void SetViewState(bool _val)
        {
            view.SetActive(_val);
            if(_val)
                view.transform.SetAsLastSibling();
        }
        
        public bool IsActive()
        {
            return view.activeSelf;
        }

        public void SetView(GameObject _newView)
        {
            view = _newView;
        }
        
    }

    [SerializeField] private List<OverlayUiSample> overlayUiViews = new List<OverlayUiSample>();

    public void ShowOverlayUI(int _id)
    {
        overlayUiViews[_id].SetViewState(true);
    }

    public void HideOverlayUI(int _id)
    {
        overlayUiViews[_id].SetViewState(false);
    }

    //call this bool to check if player can move
    public bool SomeOverlayUiIsActive()
    {
        return overlayUiViews.Any(p => p.IsActive());
    }
    
    // call from ui for using window
    public void AutoHideShowSetStateObject(int _viewID)
    {
        if (!overlayUiViews[_viewID].IsActive())
            ShowOverlayUI(_viewID);
        else
            HideOverlayUI(_viewID);
    }

    public void SetView(int _idInOverlayUiViews, GameObject _view)
    {
        overlayUiViews[_idInOverlayUiViews].SetView(_view);
    }
}
