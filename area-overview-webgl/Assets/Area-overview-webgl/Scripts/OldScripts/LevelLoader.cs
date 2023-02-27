using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Script control fade in/out effects and load next lvl
 */
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float delayTime = 0.3f; // delay after fade screen 
    
    [Header("Empty if don't need teleport with fade screen on scene")]
    [SerializeField] private Transform floorFirst;
    [SerializeField] private GameObject firstFloorBtn;
    [SerializeField] private Transform floorSecond;
    [SerializeField] private GameObject secondFloorBtn;
    [SerializeField] private Transform player;

    [Header("Block click via gameobject")] [SerializeField]
    private Transform teleportBtn;

    [SerializeField] private Camera cam;
    public void Awake()
    {
        Instance = this;
        StartCoroutine(FadeImage(true, -1));
    }

    // call from ui
    public void LoadLevel(int lvl)
    {
        RaycastHit hit;
        if (Physics.Linecast(cam.transform.position, teleportBtn.position, out hit))
        {
            Debug.Log("blocked" + hit.collider.gameObject.name);
            
        }
        else
        {
            StartCoroutine(FadeImage(false, lvl));
        }
    }

    private void Update()
    {
        Vector3 worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(worldMousePosition, teleportBtn.position);
    }


    IEnumerator FadeImage(bool fadeAway, int _lvl)
    {
        if (fadeAway)
        {
            // hide black screen
            float _j= 1;
            while (_j >= 0)
            {
                _j -= Time.fixedDeltaTime / .5f;
                // set color with i as alpha
                fadeScreen.color = new Color(0, 0, 0, _j);
                yield return null;
            }

            yield return new WaitForSeconds(delayTime);
        }
        else
        {
            // show black screen
            float _i = 0;
            while (_i <= 1)
            {
                _i += Time.fixedDeltaTime / .5f;
                // set color with i as alpha
                fadeScreen.color = new Color(0, 0, 0, _i);
                yield return null;
            }
        }
        if (!fadeAway)
        {
            UnityEngine.Resources.UnloadUnusedAssets();
            SceneManager.LoadScene(_lvl);
        }
    }
    
    // call from ui
    public void LoadFirstFloor(bool val)
    {
        StartCoroutine(LoadFirstFloorCor(val));
    }

    private IEnumerator LoadFirstFloorCor(bool _val)
    {
        // show black screen
        float _i = 0;
        while (_i <= 1)
        {
            _i += Time.fixedDeltaTime / .5f;
            // set color with i as alpha
            fadeScreen.color = new Color(0, 0, 0, _i);
            yield return null;
        }
        
        yield return new WaitForSeconds(delayTime);
        
        if (!_val)
            player.transform.position = floorSecond.position;
        else
            player.transform.position = floorFirst.position;
        
        firstFloorBtn.SetActive(!_val);
        secondFloorBtn.SetActive(_val);

        // hide black screen
        float _j= 1;
        while (_j >= 0)
        {
            _j -= Time.fixedDeltaTime / .5f;
            // set color with i as alpha
            fadeScreen.color = new Color(0, 0, 0, _j);
            yield return null;
        }
    }

    public void CheckFloorBtnStatus(Transform _trigger, Transform _player)
    {
        if (_trigger.position.y > _player.position.y)
        {
            firstFloorBtn.SetActive(false);
            secondFloorBtn.SetActive(true);
        }
        else
        {
            firstFloorBtn.SetActive(true);
            secondFloorBtn.SetActive(false);
        }
    }
}
