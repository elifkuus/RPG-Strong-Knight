using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public GameObject mousePoint;

    private CursorMode mode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero; //mouse ucu

    void Start()
    {

    }


    void Update()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, mode);

        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Vector3 newPos = hit.point;
                    newPos.y = 0.6f;
                    Instantiate(mousePoint, newPos, Quaternion.identity);
                }
            }
        }
    }
}
