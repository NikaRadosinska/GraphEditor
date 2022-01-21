using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Settings : MainBehaviour
{
    private GraphEditorInputAction graphEditorInputAction;
    private InputAction IAWheelButtonPressed;
    private InputAction IALeftMouseButtonPressed;
    private InputAction IAMouseMovement;
    private InputAction IAScroll;
    private InputAction IATPress;
    private InputAction IADeletePress;
    private InputAction IACtrlC;
    private InputAction IACtrl;

    private string jsonPlayerSettingsPath;
    public PlayerSettings PlayerSettings;

    private Camera mainCamera;
    public AbstractControlStrategy controlStrategy;

    private bool isAdding = false;

    private bool isCameraMoving;
    public bool isGraphPartMoving;
    public bool setToClassicControlStrategy = false;
    private Vector2 prevMousScreenPos;
    private Vector2 prevMousWorldPos;

    public Collector collectorPrefab;

    private void Awake()
    {
        base.Awake();
        graphEditorInputAction = new GraphEditorInputAction();
    }

    private void OnEnable()
    {
        IAWheelButtonPressed = graphEditorInputAction.Player.WheelButtonPressed;
        IALeftMouseButtonPressed = graphEditorInputAction.Player.LeftMouseButtonPressed;
        IAMouseMovement = graphEditorInputAction.Player.MouseMovement;
        IAScroll = graphEditorInputAction.Player.Scroll;
        IATPress = graphEditorInputAction.Player.AddBehaviourOnOff;
        IADeletePress = graphEditorInputAction.Player.OnDelete;
        IACtrlC = graphEditorInputAction.Player.CtrlC;
        IACtrl = graphEditorInputAction.Player.Ctrl;

        IAWheelButtonPressed.performed += OnWheelButtonPressedDown;
        IAWheelButtonPressed.canceled += OnWheelButtonReleased;
        IALeftMouseButtonPressed.performed += OnLeftMouseButtonClick;
        IALeftMouseButtonPressed.canceled += OnLeftMouseButtonReleased;
        IALeftMouseButtonPressed.canceled += GraphManager.StopMovement;
        IAMouseMovement.performed += OnMouseMovement;
        IAScroll.performed += OnScroll;
        IATPress.performed += OnTPress;
        IADeletePress.performed += OnDeleteButtonPressedDown;
        IACtrlC.performed += OnCtrlC;
        IACtrl.performed += OnCtrlButtonPressedDown;
        IACtrl.canceled += OnCtrlButtonReleased;

        graphEditorInputAction.Enable();
    }

    private void Start()
    {
        SetControlStrategy(ControlStrategyType.CLASSIC);
        mainCamera = Camera.main;

        jsonPlayerSettingsPath = Application.persistentDataPath + "/playerSettings.json";
        if (File.Exists(jsonPlayerSettingsPath))
        {
            PlayerSettings = JsonUtility.FromJson<PlayerSettings>(File.ReadAllText(jsonPlayerSettingsPath));
        }
        else
        {
            PlayerSettings = new PlayerSettings();
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    public void SetControlStrategy(ControlStrategyType controlStrategyType)
    {
        switch (controlStrategyType)
        {
            case ControlStrategyType.CLASSIC:
                ClassicControl classicControl = new ClassicControl(GraphManager);
                controlStrategy = classicControl;
                break;
            case ControlStrategyType.CTRL:
                CtrlControl ctrlControl = new CtrlControl(collectorPrefab, GraphManager);
                controlStrategy = ctrlControl;
                break;
            case ControlStrategyType.SHIFT:
                ShiftControl shiftControl = new ShiftControl(GraphManager);
                controlStrategy = shiftControl;
                break;
            case ControlStrategyType.ADD:
                AddControlStrategy addControlStrategy = new AddControlStrategy(GraphManager);
                controlStrategy = addControlStrategy;
                break;
        }
    }

    public void OnSettingsChanges()
    {
        string jsonPlayerSettingsData = JsonUtility.ToJson(PlayerSettings);
        File.WriteAllText(jsonPlayerSettingsPath, jsonPlayerSettingsData);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    public void OnWheelButtonPressedDown(InputAction.CallbackContext context) { isCameraMoving = true; }
    public void OnWheelButtonReleased(InputAction.CallbackContext context) { isCameraMoving = false; }
    public void OnCtrlButtonPressedDown(InputAction.CallbackContext context) { SetControlStrategy(ControlStrategyType.CTRL);  }
    public void OnCtrlButtonReleased(InputAction.CallbackContext context) {
        if (isGraphPartMoving || isCameraMoving)
        {
            setToClassicControlStrategy = true;
            return;
        }
        SetControlStrategy(ControlStrategyType.CLASSIC); 
    }
    public void OnDeleteButtonPressedDown(InputAction.CallbackContext context) { controlStrategy.OnDelete(); }
    public void OnCtrlC(InputAction.CallbackContext context)
    {
        GUIUtility.systemCopyBuffer = GraphManager.GetAllGraphInfo();
        Debug.Log(GUIUtility.systemCopyBuffer);
    }

    public void OnLeftMouseButtonClick(InputAction.CallbackContext context)
    {
        if (controlStrategy.OnClick((Vector2)mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue())) == null){
            OnWheelButtonPressedDown(context);
        } 
        else
        {
            isGraphPartMoving = true;
        }
    }

    public void OnLeftMouseButtonReleased(InputAction.CallbackContext context) { 
        isGraphPartMoving = false;  
        controlStrategy.OnHoldEnd((Vector2)mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        OnWheelButtonReleased(context);
        if (setToClassicControlStrategy)
        {
            SetControlStrategy(ControlStrategyType.CLASSIC);
            setToClassicControlStrategy = false;
        }
    }


    public void OnMouseMovement(InputAction.CallbackContext context)
    {
        Vector2 mousPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (isGraphPartMoving)
        {
            controlStrategy.OnHold(mousPos - prevMousWorldPos, mousPos);
        }

        if (isCameraMoving)
        {
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                mainCamera.transform.position -= ((Vector3)context.ReadValue<Vector2>() - (Vector3)prevMousScreenPos) * Time.deltaTime * PlayerSettings.MouseSensitivity * mainCamera.orthographicSize;
            }
        }

        controlStrategy.OnMouseMove(mousPos - prevMousWorldPos, mousPos);
        prevMousWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        prevMousScreenPos = context.ReadValue<Vector2>();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 zoom = context.ReadValue<Vector2>();
        if (zoom.y != 0)
        {
            mainCamera.orthographicSize -= zoom.y * Time.deltaTime * PlayerSettings.MouseWheelSensitivity;
            if (mainCamera.orthographicSize < 1)
            {
                mainCamera.orthographicSize = 1;
            }
        }
    }

    public void OnTPress(InputAction.CallbackContext context)
    {
        if (isAdding == false)
        {
            SetControlStrategy(ControlStrategyType.ADD);
            isAdding = true;
        }
        else
        {
            SetControlStrategy(ControlStrategyType.CLASSIC);
            isAdding = false;
        }
    }
}
