using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbstractState : AccessBehaviour
{
    protected bool toRemoveSelection = false;

    protected static Vector3 prevMouseScreenPos = Vector3.zero;
    protected static Vector3 prevMouseWorldPos = Vector3.zero;

    private static int lastSelectedID;
    protected static IGraphPart lastSelected
    {
        get { return IDManager.GetGP(lastSelectedID); }
        set { lastSelectedID = (value != null) ? value.GetID() : 0; }
    }

    protected static Camera mainCamera;

    protected bool isAdding = false;

    protected bool isCameraMoving;
    protected bool isGraphPartMoving;

    protected static Collector collectorPrefab;

    public static void MyStart(Camera _mainCamera, Collector _collectorPrefab)
    {
        mainCamera = _mainCamera;
        collectorPrefab = _collectorPrefab;
    }

    public void MouseMovement(InputAction.CallbackContext context, IAbstractState controlState)
    {
        Vector2 mousPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //riesi konkretny
        //if (isGraphPartMoving)

        if (isCameraMoving)
        {
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                mainCamera.transform.position -= ((Vector3)context.ReadValue<Vector2>() - (Vector3)prevMouseScreenPos) * Time.deltaTime * Settings.PlayerSettings.MouseSensitivity * mainCamera.orthographicSize;
            }
        }

        prevMouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        prevMouseScreenPos = context.ReadValue<Vector2>();
    }

    public void Scroll(InputAction.CallbackContext context)
    {
        Vector2 zoom = context.ReadValue<Vector2>();
        if (zoom.y != 0)
        {
            mainCamera.orthographicSize -= zoom.y * Time.deltaTime * Settings.PlayerSettings.MouseWheelSensitivity;
            if (mainCamera.orthographicSize < 3)
            {
                mainCamera.orthographicSize = 3;
            }
        }
    }
}
