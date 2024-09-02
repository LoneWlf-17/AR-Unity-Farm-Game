using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;

    private LandManager previousLandSelecton;
    private FarmManager previousFieldSelecton;

    private float backButtonTimer = 0f;
    private bool hasClickedBefore = false;

    RaycastHit hit;

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                castRayAndCheck(ray);
            }
        }
        else
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !IsPointerOverUIObject(canvas, Input.GetTouch(0).position))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
                castRayAndCheck(ray);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (hasClickedBefore && backButtonTimer < 1f)
                {
                    hasClickedBefore = false;
                    Application.Quit();
                }
                hasClickedBefore = true;
            }
            if (hasClickedBefore)
            {
                backButtonTimer += Time.deltaTime;
                if (backButtonTimer > 1f)
                {
                    backButtonTimer = 0f;
                    hasClickedBefore = false;
                }
            }
        }
    }

    public void castRayAndCheck(Ray ray)
    {
        if (Physics.Raycast(ray, out hit, 50f))
        {
            if (hit.collider.tag == "BuySign")
            {
                landSelection(hit.collider.GetComponentInParent<LandManager>(), hit.collider.GetComponentInParent<AreaManager>());
            }

            else if (hit.collider.tag == "Field")
            {
                FieldSelection(hit.collider.GetComponent<FarmManager>());
            }

            else if (hit.collider.tag == "Tree")
            {
                treeSelection(hit.collider.GetComponent<TreeManager>());
            }

            else
            {
                GameManager.Instance.togglePanel(-1);
            }
        }
        else
        {
            GameManager.Instance.togglePanel(-1);

            if (previousLandSelecton != null)
                previousLandSelecton.toggleSelection(false);
            if (previousFieldSelecton != null)
                previousFieldSelecton.toggleSelection(false);
        }
    }

    private void landSelection(LandManager landManager, AreaManager areaManager)
    {
        GameManager.Instance.togglePanel(0);

        landManager.toggleSelection(true);

        if (previousLandSelecton != null && previousLandSelecton != landManager)
            previousLandSelecton.toggleSelection(false);
        if (previousFieldSelecton != null)
            previousFieldSelecton.toggleSelection(false);
        previousLandSelecton = landManager;
    }

    private void FieldSelection(FarmManager farmManager)
    {
        if (farmManager.isHarvestable)
        {
            farmManager.harvestField();
            GameManager.Instance.togglePanel(-1);
        }
        else
        {
            GameManager.Instance.togglePanel(1);

            farmManager.toggleSelection(true);
        }
        if (previousFieldSelecton != null && previousFieldSelecton != farmManager)
            previousFieldSelecton.toggleSelection(false);
        if (previousLandSelecton != null)
            previousLandSelecton.toggleSelection(false);
        previousFieldSelecton = farmManager;
    }

    private void treeSelection(TreeManager treeManager)
    {
        if (treeManager.isHarvestable)
        {
            treeManager.harvestFruits();
        }
        GameManager.Instance.togglePanel(-1);
    }

    private bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;

        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
