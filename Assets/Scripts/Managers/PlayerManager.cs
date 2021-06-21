using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ObjectInfo test;
    RaycastHit hit;
    List<UnitController> selectedUnits = new List<UnitController>();
    List<UnitController> selectedEnemyUnits = new List<UnitController>();
    List<ColleterUnitController> selectedCollecter = new List<ColleterUnitController>();
    bool isDragging = false;
    Vector3 mousePositon;
    public TaskList task;
    GameObject targetNode;

    public GameObject selectedObject;

    private ObjectInfo selectedInfo;


    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePositon, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.blue);
        }

    }

    // Update is called once per frame
    void Update()
    {

        //Detect if mouse is down
        if (Input.GetMouseButtonDown(0))
        {
            mousePositon = Input.mousePosition;
            //Create a ray from the camera to our space
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Shoot that ray and get the hit data
            if (Physics.Raycast(camRay, out hit))
            {
                //Do something with that data 
                //Debug.Log(hit.transform.tag);
                if (hit.transform.CompareTag("PlayerUnit"))
                {
                    SelectUnit(hit.transform.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftShift));
                    DeselectEnemyUnits();
                    DeselectCollecterUnits();
                    selectedObject = null;
                }
                else if(hit.transform.CompareTag("EnemyUnit"))
                {

                    SelectEnemyUnit(hit.transform.GetComponent<UnitController>(), false);
                    selectedObject = null;
                }
                else if(hit.transform.CompareTag("PlayerCollect"))
                {
                    //Debug.Log("Click");
                    //SelectUnit(hit.transform.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftShift));
                    SelectCollecterUnit(hit.transform.GetComponent<ColleterUnitController>(), Input.GetKey(KeyCode.LeftShift));
                    /*selectedObject = hit.collider.gameObject;
                    selectedInfo = selectedObject.GetComponent<ObjectInfo>();
                    selectedInfo.isSelected = true;*/
                    selectedObject = hit.collider.gameObject;
                    selectedInfo = selectedObject.GetComponent<ObjectInfo>();


                    selectedInfo.isSelected = true;
                }
                else
                {
                    isDragging = true;
                    selectedObject = null;
                    DeselectEnemyUnits();
                    DeselectCollecterUnits();
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                DeselectUnits();
                DeselectEnemyUnits();
                DeselectCollecterUnits();
                foreach (var selectableObject in FindObjectsOfType<PlayerUnitController>())
                {
                    if (IsWithinSelectionBounds(selectableObject.transform))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<UnitController>(), true);
                    }
                }

                isDragging = false;
            }
            else 
            {
                DeselectEnemyUnits();
                DeselectCollecterUnits();
            }

        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Shoot that ray and get the hit data
            if (Physics.Raycast(camRay, out hit))
            {
                //Do something with that data 
                //Debug.Log(hit.transform.tag);
                if (hit.transform.CompareTag("Ground"))
                {
                    foreach (var selectableObj in selectedUnits)
                    {
                        selectableObj.MoveUnit(hit.point);
                    }
                }
                else if (hit.transform.CompareTag("EnemyUnit"))
                {
                    foreach (var selectableObj in selectedUnits)
                    {
                        selectableObj.SetNewTarget(hit.transform);
                    }
                }
                else if (hit.transform.CompareTag("Resource"))
                {
                    foreach (var selectableObj in selectedCollecter)
                    {
                        selectableObj.MoveColleterUnit(hit.point);
                    }
                }
            }
        }
    }

    private void SelectUnit(UnitController unit, bool isMultiSelect = false)
    {
        if (!isMultiSelect)
        {
            DeselectUnits();
        }
        selectedUnits.Add(unit);
        unit.SetSelected(true);
    }

    private void SelectEnemyUnit(UnitController unit, bool isMultiSelect = false)
    {
        if (!isMultiSelect)
        {
            DeselectEnemyUnits();
        }
        selectedEnemyUnits.Add(unit);
        unit.SetSelected(true);

    }

    private void SelectCollecterUnit(ColleterUnitController unit, bool isMultiSelect = false)
    {
        if (!isMultiSelect)
        {
            DeselectUnits();
        }
        selectedCollecter.Add(unit);
        unit.SetSelected(true);
    }

    private void DeselectUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            // selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
            selectedUnits[i].SetSelected(false);
        }
        selectedUnits.Clear();
    }

    private void DeselectEnemyUnits()
    {
        for (int i = 0; i < selectedEnemyUnits.Count; i++)
        {
            // selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
            selectedEnemyUnits[i].SetSelected(false);
        }
        selectedEnemyUnits.Clear();
    }

    private void DeselectCollecterUnits()
    {
        for (int i = 0; i < selectedEnemyUnits.Count; i++)
        {
            // selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
            selectedCollecter[i].SetSelected(false);
        }
        selectedCollecter.Clear();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        var camera = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(camera, mousePositon, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }
}