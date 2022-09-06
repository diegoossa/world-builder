//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Settings/Input/TouchInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TouchInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchInput"",
    ""maps"": [
        {
            ""name"": ""Touch"",
            ""id"": ""10af303e-bea4-41a8-8039-068c63d6abec"",
            ""actions"": [
                {
                    ""name"": ""PrimaryTouchPosition"",
                    ""type"": ""Value"",
                    ""id"": ""20adb8ab-0782-4f51-8a5e-606cc51fee9a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryTouchPosition"",
                    ""type"": ""Value"",
                    ""id"": ""3b84a540-7afc-49ef-bc7e-cf272658885c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryTouchContact"",
                    ""type"": ""Button"",
                    ""id"": ""a95c276e-84c6-400c-8392-a6dd2efdc8e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrimaryTouchDelta"",
                    ""type"": ""Value"",
                    ""id"": ""927631e4-46ba-428a-92eb-f0ee54cd9182"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryTouchDelta"",
                    ""type"": ""Value"",
                    ""id"": ""8b737820-2f1f-4f87-94bf-679e160488ac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PrimaryTap"",
                    ""type"": ""Button"",
                    ""id"": ""99c5baa7-2b66-49a6-a3f0-8366498c740d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0bcc2fab-c4da-4266-99d5-b878de52e861"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""973d31ca-e2e8-4200-9c40-bdaa74185edb"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb195fa5-b947-4bf7-8074-b28171c5c2bf"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b2aac77-4058-47ca-afb8-115232ced47b"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42aeb10b-d14a-44e1-a103-c34e811b863c"",
                    ""path"": ""<Touchscreen>/touch1/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8751c15-16aa-4ae5-b0b7-bc2695f20095"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Touch
        m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
        m_Touch_PrimaryTouchPosition = m_Touch.FindAction("PrimaryTouchPosition", throwIfNotFound: true);
        m_Touch_SecondaryTouchPosition = m_Touch.FindAction("SecondaryTouchPosition", throwIfNotFound: true);
        m_Touch_SecondaryTouchContact = m_Touch.FindAction("SecondaryTouchContact", throwIfNotFound: true);
        m_Touch_PrimaryTouchDelta = m_Touch.FindAction("PrimaryTouchDelta", throwIfNotFound: true);
        m_Touch_SecondaryTouchDelta = m_Touch.FindAction("SecondaryTouchDelta", throwIfNotFound: true);
        m_Touch_PrimaryTap = m_Touch.FindAction("PrimaryTap", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Touch
    private readonly InputActionMap m_Touch;
    private ITouchActions m_TouchActionsCallbackInterface;
    private readonly InputAction m_Touch_PrimaryTouchPosition;
    private readonly InputAction m_Touch_SecondaryTouchPosition;
    private readonly InputAction m_Touch_SecondaryTouchContact;
    private readonly InputAction m_Touch_PrimaryTouchDelta;
    private readonly InputAction m_Touch_SecondaryTouchDelta;
    private readonly InputAction m_Touch_PrimaryTap;
    public struct TouchActions
    {
        private @TouchInput m_Wrapper;
        public TouchActions(@TouchInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryTouchPosition => m_Wrapper.m_Touch_PrimaryTouchPosition;
        public InputAction @SecondaryTouchPosition => m_Wrapper.m_Touch_SecondaryTouchPosition;
        public InputAction @SecondaryTouchContact => m_Wrapper.m_Touch_SecondaryTouchContact;
        public InputAction @PrimaryTouchDelta => m_Wrapper.m_Touch_PrimaryTouchDelta;
        public InputAction @SecondaryTouchDelta => m_Wrapper.m_Touch_SecondaryTouchDelta;
        public InputAction @PrimaryTap => m_Wrapper.m_Touch_PrimaryTap;
        public InputActionMap Get() { return m_Wrapper.m_Touch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
        public void SetCallbacks(ITouchActions instance)
        {
            if (m_Wrapper.m_TouchActionsCallbackInterface != null)
            {
                @PrimaryTouchPosition.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchPosition;
                @SecondaryTouchPosition.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchPosition;
                @SecondaryTouchPosition.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchPosition;
                @SecondaryTouchPosition.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchPosition;
                @SecondaryTouchContact.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchContact;
                @PrimaryTouchDelta.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchDelta;
                @PrimaryTouchDelta.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchDelta;
                @PrimaryTouchDelta.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTouchDelta;
                @SecondaryTouchDelta.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchDelta;
                @SecondaryTouchDelta.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchDelta;
                @SecondaryTouchDelta.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnSecondaryTouchDelta;
                @PrimaryTap.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTap;
                @PrimaryTap.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTap;
                @PrimaryTap.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnPrimaryTap;
            }
            m_Wrapper.m_TouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryTouchPosition.started += instance.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.performed += instance.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.canceled += instance.OnPrimaryTouchPosition;
                @SecondaryTouchPosition.started += instance.OnSecondaryTouchPosition;
                @SecondaryTouchPosition.performed += instance.OnSecondaryTouchPosition;
                @SecondaryTouchPosition.canceled += instance.OnSecondaryTouchPosition;
                @SecondaryTouchContact.started += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled += instance.OnSecondaryTouchContact;
                @PrimaryTouchDelta.started += instance.OnPrimaryTouchDelta;
                @PrimaryTouchDelta.performed += instance.OnPrimaryTouchDelta;
                @PrimaryTouchDelta.canceled += instance.OnPrimaryTouchDelta;
                @SecondaryTouchDelta.started += instance.OnSecondaryTouchDelta;
                @SecondaryTouchDelta.performed += instance.OnSecondaryTouchDelta;
                @SecondaryTouchDelta.canceled += instance.OnSecondaryTouchDelta;
                @PrimaryTap.started += instance.OnPrimaryTap;
                @PrimaryTap.performed += instance.OnPrimaryTap;
                @PrimaryTap.canceled += instance.OnPrimaryTap;
            }
        }
    }
    public TouchActions @Touch => new TouchActions(this);
    public interface ITouchActions
    {
        void OnPrimaryTouchPosition(InputAction.CallbackContext context);
        void OnSecondaryTouchPosition(InputAction.CallbackContext context);
        void OnSecondaryTouchContact(InputAction.CallbackContext context);
        void OnPrimaryTouchDelta(InputAction.CallbackContext context);
        void OnSecondaryTouchDelta(InputAction.CallbackContext context);
        void OnPrimaryTap(InputAction.CallbackContext context);
    }
}
