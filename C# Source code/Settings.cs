using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using ExtentionMethods;

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
    private InputAction IACtrlZ;
    private InputAction IACtrlY;
    private InputAction IACtrl;
    private InputAction IACtrlS;
    private InputAction IAShowHints;

    private string jsonPlayerSettingsPath;

    public PlayerSettings PlayerSettings;

    private Camera mainCamera;

    public IAbstractState controlState;

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
        IACtrlZ = graphEditorInputAction.Player.CtrlZ;
        IACtrlY = graphEditorInputAction.Player.CtrlY;
        IACtrlS = graphEditorInputAction.Player.CtrlS;
        IACtrl = graphEditorInputAction.Player.Ctrl;
        IAShowHints = graphEditorInputAction.Player.ShowHints;

        IAWheelButtonPressed.performed += OnWheelButtonPressedDown;
        IAWheelButtonPressed.canceled += OnWheelButtonReleased;
        IALeftMouseButtonPressed.performed += OnLeftMouseButtonClick;
        IALeftMouseButtonPressed.canceled += OnLeftMouseButtonReleased;
        IAMouseMovement.performed += OnMouseMovement;
        IAScroll.performed += OnScroll;
        IATPress.performed += OnToAddPressed;
        IATPress.canceled += OnToAddReleased;
        IADeletePress.performed += OnDeleteButtonPressedDown;
        IACtrlC.performed += OnCtrlC;
        IACtrlZ.performed += OnCtrlZ;
        IACtrlY.performed += OnCtrlY;
        IACtrlS.performed += OnCtrlS;
        IACtrl.performed += OnCtrlButtonPressedDown;
        IACtrl.canceled += OnCtrlButtonReleased;
        IAShowHints.performed += OnShowHints;
        IAShowHints.canceled += OnShowHints;

        graphEditorInputAction.Enable();
    }

    private void Start()
    {
        AbstractState.MyStart(Camera.main, collectorPrefab);

        controlState = new ClassicControlState();

        jsonPlayerSettingsPath = Application.persistentDataPath + "/playerSettings.json";
        if (File.Exists(jsonPlayerSettingsPath))
        {
            PlayerSettings = JsonUtility.FromJson<PlayerSettings>(File.ReadAllText(jsonPlayerSettingsPath));
        }
        else
        {
            PlayerSettings = new PlayerSettings();
            PlayerSettings.MyGraphs = new List<SubGraphInfo>();
            PlayerSettings.MyGraphNames = new List<string>();
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        mainCamera = Camera.main;

        PlayerSettings.MouseSensitivity = 0.1f;
        OnSettingsChanged();
    }

    public void OnSettingsChanged()
    {
        string jsonPlayerSettingsData = JsonUtility.ToJson(PlayerSettings);
        File.WriteAllText(jsonPlayerSettingsPath, jsonPlayerSettingsData);
    }

    public void ChangeState(ControlStateType type)
    {
        switch (type)
        {
            case ControlStateType.CLASSIC:
                UIManager.FromAdd();
                UIManager.FromCtrl();
                controlState = new ClassicControlState();
                break;
            case ControlStateType.SHIFT:
                //controlState = new ShiftControlState();
                break;
            case ControlStateType.CTRL:
                UIManager.ToCtrl();
                controlState = new CtrlControlState();
                break;
            case ControlStateType.ADD:
                UIManager.ToAdd();
                controlState = new AddControlState();
                break;
        }
    }

    public void ChangeStateOnHold()
    {
        controlState = new HoldControlState(GraphManager.GetSelectedSubGraphInfo());
    }

    public void ChangeStateOnHold(SubGraphInfo subGraphInfo)
    {
        controlState = new HoldControlState(subGraphInfo);
    }

    public void SaveSubGraph()
    {
        PlayerSettings.MyGraphs.Add(GraphManager.GetSelectedSubGraphInfo());
        UIManager.SaveName();
    }

    public void SaveName(string s)
    {
        PlayerSettings.MyGraphNames.Add(s);
        OnSettingsChanged();
    }

    public void DeleteMyGraph(int index)
    {
        PlayerSettings.MyGraphNames.RemoveAt(index);
        PlayerSettings.MyGraphs.RemoveAt(index);
        OnSettingsChanged();
        UIManager.OpenMyGraphsPanel();
    }

    public bool DesibleGraphManagement()
    {
        if(controlState.GetType() == ControlStateType.CLASSIC)
        {
            controlState = new EmptyState();
            return true;
        } 
        else
        {
            return false;
        }
    }

    public void EnableGraphManagement()
    {
        ChangeState(ControlStateType.CLASSIC);
    }

    private bool clickedOnUI;
    //////////////////////////////////////////////////////////////////////////////////////
    public void OnWheelButtonPressedDown(InputAction.CallbackContext context) { controlState.WheelButtonPressed(); }
    public void OnWheelButtonReleased(InputAction.CallbackContext context) { controlState.WheelButtonReleased(); }
    public void OnCtrlButtonPressedDown(InputAction.CallbackContext context) { controlState.CtrlPressed();  }
    public void OnCtrlButtonReleased(InputAction.CallbackContext context) { controlState.CtrlReleased(); }
    public void OnDeleteButtonPressedDown(InputAction.CallbackContext context) { controlState.DeleteButtonPressedDown(); }
    public void OnCtrlC(InputAction.CallbackContext context) { controlState.CtrlC(); }
    public void OnLeftMouseButtonClick(InputAction.CallbackContext context) {
        if (MyExtentions.IsPointerOverUIElement())
        {
            clickedOnUI = true;
            return;
        }
        controlState.LeftMouseButtonClick((Vector2)mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue())); 
    }
    public void OnLeftMouseButtonReleased(InputAction.CallbackContext context) {
        if (clickedOnUI)
        {
            clickedOnUI = false;
            return;
        }
        controlState.LeftMouseButtonReleased(context); 
    }
    public void OnMouseMovement(InputAction.CallbackContext context) { controlState.MouseMovement(context); }
    public void OnScroll(InputAction.CallbackContext context) { controlState.Scroll(context); }
    public void OnToAddPressed(InputAction.CallbackContext context) { controlState.ToAddPressed(); }
    public void OnToAddReleased(InputAction.CallbackContext context) { controlState.ToAddReleased(); }
    public void OnCtrlS(InputAction.CallbackContext context) { controlState.CtrlS(); }
    public void OnCtrlZ(InputAction.CallbackContext context) { controlState.CtrlZ(); }
    public void OnCtrlY(InputAction.CallbackContext context) { /*controlState.CtrlY();*/ }
    public void OnShowHints(InputAction.CallbackContext context) { UIManager.ShowHints(); }
    public void OnCopyInfo()
    {
        GUIUtility.systemCopyBuffer = GraphManager.GetAllGraphInfo();
        Debug.Log(GUIUtility.systemCopyBuffer);
    }
}
