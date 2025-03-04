using UnityEngine;

public class DragAndDrop3D : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 startPosition;
    private ItemView selectedObject;
    private float originalZ;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("ItemView"))
                {
                    selectedObject = hit.collider.GetComponent<ItemView>();

                    var position = selectedObject.transform.position;
                    startPosition = position;
                    offset = position - GetMouseWorldPos();
                    isDragging = true;

                    originalZ = position.z;

                    position = new Vector3(position.x, position.y, -2f);
                    selectedObject.transform.position = position;
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging && selectedObject != null)
        {
            selectedObject.transform.position = GetMouseWorldPos() + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            var grid = IsOverGridView();
            if (grid == null)
            {
                selectedObject.transform.position = startPosition;
            }
            else
            {
                if (grid.PutNewItemToSlot(selectedObject))
                {
                    selectedObject = null;
                    grid.DestroyItems();
                }
                else
                {
                    selectedObject.transform.position = startPosition;
                }
            }

            selectedObject = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(selectedObject.transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private GridView IsOverGridView()
    {
        int draggingGridLayer = LayerMask.GetMask("ItemView");
        int layerMask = ~draggingGridLayer;

        if (!Physics.Raycast(selectedObject.transform.position, Vector3.forward, out var hit, Mathf.Infinity, layerMask)) return null;

        return hit.collider.CompareTag("GridView") ? hit.collider.GetComponent<GridView>() : null;
    }
}