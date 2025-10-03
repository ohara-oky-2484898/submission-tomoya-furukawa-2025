// GENERATED AUTOMATICALLY FROM 'Assets/02_controller/UIControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @UIControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @UIControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UIControls"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""43c1fe1d-a8c3-44a2-9701-d2c0f160ff5c"",
            ""actions"": [
                {
                    ""name"": ""gamestr"",
                    ""type"": ""Button"",
                    ""id"": ""a3dfe589-6930-4778-a891-aaec649b3831"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""gamequit"",
                    ""type"": ""Button"",
                    ""id"": ""dddf78ce-4fbb-4f81-89c8-f0e879296e51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""53a3449c-9cd0-45c5-99b8-440061e0535f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f2a0a0f4-21c1-4935-b4b4-eac9cdc10d8c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gamestr"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb630d71-9908-447c-82d4-288eac4682f3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gamequit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec5f895d-9b63-415f-9b57-3be6e3629bfc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12aa3454-86de-488d-aa3f-c9885c17ebc1"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_gamestr = m_UI.FindAction("gamestr", throwIfNotFound: true);
        m_UI_gamequit = m_UI.FindAction("gamequit", throwIfNotFound: true);
        m_UI_Quit = m_UI.FindAction("Quit", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_gamestr;
    private readonly InputAction m_UI_gamequit;
    private readonly InputAction m_UI_Quit;
    public struct UIActions
    {
        private @UIControls m_Wrapper;
        public UIActions(@UIControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @gamestr => m_Wrapper.m_UI_gamestr;
        public InputAction @gamequit => m_Wrapper.m_UI_gamequit;
        public InputAction @Quit => m_Wrapper.m_UI_Quit;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @gamestr.started -= m_Wrapper.m_UIActionsCallbackInterface.OnGamestr;
                @gamestr.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnGamestr;
                @gamestr.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnGamestr;
                @gamequit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnGamequit;
                @gamequit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnGamequit;
                @gamequit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnGamequit;
                @Quit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnQuit;
                @Quit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnQuit;
                @Quit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnQuit;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @gamestr.started += instance.OnGamestr;
                @gamestr.performed += instance.OnGamestr;
                @gamestr.canceled += instance.OnGamestr;
                @gamequit.started += instance.OnGamequit;
                @gamequit.performed += instance.OnGamequit;
                @gamequit.canceled += instance.OnGamequit;
                @Quit.started += instance.OnQuit;
                @Quit.performed += instance.OnQuit;
                @Quit.canceled += instance.OnQuit;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IUIActions
    {
        void OnGamestr(InputAction.CallbackContext context);
        void OnGamequit(InputAction.CallbackContext context);
        void OnQuit(InputAction.CallbackContext context);
    }
}
