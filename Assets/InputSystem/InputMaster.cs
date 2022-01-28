// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""11cfcd02-fb9e-4669-acf3-6c6f920db4bd"",
            ""actions"": [
                {
                    ""name"": ""QueueEdur"",
                    ""type"": ""Button"",
                    ""id"": ""68512863-54f3-44cc-846f-905913d2396c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QueueCinos"",
                    ""type"": ""Button"",
                    ""id"": ""e0aa369c-11f2-4e9d-9afc-55d73a2a218b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QueueSoleis"",
                    ""type"": ""Button"",
                    ""id"": ""ec15c70d-a31f-49a8-94b0-710a455e0c0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CombineLumes"",
                    ""type"": ""Button"",
                    ""id"": ""2bd2882c-e736-4890-a18d-b1266f2cf794"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9f6da866-d51c-4729-9425-e272045be8d0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CastSpell"",
                    ""type"": ""Button"",
                    ""id"": ""97066eb3-ffdd-4dd3-b631-95fb711cbcaa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CancelSpellCast"",
                    ""type"": ""Button"",
                    ""id"": ""2d64ee09-1747-4984-98b5-193608733ba6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move_X"",
                    ""type"": ""Button"",
                    ""id"": ""702f84cb-8d45-483c-bf47-dbfee3fa1936"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move_Z"",
                    ""type"": ""Button"",
                    ""id"": ""c6013319-c54c-4685-bd8b-7a2a7f98cad6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ClickToMove"",
                    ""type"": ""Button"",
                    ""id"": ""a1a8f1e6-526f-45dd-9a68-c1441b4cee02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f308fde4-7a8c-4af9-ac39-5ee5d3d139ae"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""QueueEdur"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75282fb2-3eb7-4cd2-8c7d-665950f4c140"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CombineLumes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07c7608d-4ff3-4bf6-bb92-9f4641445727"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2840342-954e-4717-8652-aa34584b38ed"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CastSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""badcd29f-b1dc-410b-8867-77ede43d4da4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CancelSpellCast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ae8f8a9-e3e1-45e1-9e9f-b5e44a7e0db3"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CancelSpellCast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93894eaa-6d39-4ec3-800c-2755086ff5fc"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""QueueCinos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f76a4b2c-60e4-46a5-b3fa-891b498b2142"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_X"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d6de290b-b8ef-4eac-86ad-f4596ee9e758"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fbddb97e-412b-4ab3-8d77-7be6d1e1b81e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""1169d4c5-2d66-4b77-bf6f-620b32aebbc7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Z"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c7e5231f-a6fb-4274-b97f-2f52b5efc2a5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""30760f6f-34a2-47d8-a5a2-231de2bf34b1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""82cd7f58-3402-4ceb-a0db-481c7f4ba5c3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""ClickToMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45f91dc3-9058-46c8-9797-cb2aeb81dead"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""QueueSoleis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""c9cc8f9a-789b-4562-ab53-f226134f085c"",
            ""actions"": [
                {
                    ""name"": ""EscButton"",
                    ""type"": ""Button"",
                    ""id"": ""fbc0db8d-1ba8-434f-bba8-ea9461dc3ed9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""e8e69683-df89-4d1d-9cfa-e395bb0179a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""6adde802-5002-4686-95de-903de9fb5c31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1fffce6d-b7cd-473d-98ff-4b46e752ea2f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""EscButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f56a891-9d53-4ad6-b3ed-6fcdf9713a67"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa4d07f4-7871-4ffc-8190-d8092da65d11"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""d57f698e-e3f5-413f-9ed8-7bd7a6517beb"",
            ""actions"": [
                {
                    ""name"": ""OpenDebugMenu"",
                    ""type"": ""Button"",
                    ""id"": ""9097e14b-80a2-46f9-8360-117d74f04c49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3a17b666-eb93-46ba-91d7-1009dd1640ce"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""OpenDebugMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_QueueEdur = m_Player.FindAction("QueueEdur", throwIfNotFound: true);
        m_Player_QueueCinos = m_Player.FindAction("QueueCinos", throwIfNotFound: true);
        m_Player_QueueSoleis = m_Player.FindAction("QueueSoleis", throwIfNotFound: true);
        m_Player_CombineLumes = m_Player.FindAction("CombineLumes", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_CastSpell = m_Player.FindAction("CastSpell", throwIfNotFound: true);
        m_Player_CancelSpellCast = m_Player.FindAction("CancelSpellCast", throwIfNotFound: true);
        m_Player_Move_X = m_Player.FindAction("Move_X", throwIfNotFound: true);
        m_Player_Move_Z = m_Player.FindAction("Move_Z", throwIfNotFound: true);
        m_Player_ClickToMove = m_Player.FindAction("ClickToMove", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_EscButton = m_UI.FindAction("EscButton", throwIfNotFound: true);
        m_UI_Interact = m_UI.FindAction("Interact", throwIfNotFound: true);
        m_UI_Tab = m_UI.FindAction("Tab", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_OpenDebugMenu = m_Debug.FindAction("OpenDebugMenu", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_QueueEdur;
    private readonly InputAction m_Player_QueueCinos;
    private readonly InputAction m_Player_QueueSoleis;
    private readonly InputAction m_Player_CombineLumes;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_CastSpell;
    private readonly InputAction m_Player_CancelSpellCast;
    private readonly InputAction m_Player_Move_X;
    private readonly InputAction m_Player_Move_Z;
    private readonly InputAction m_Player_ClickToMove;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @QueueEdur => m_Wrapper.m_Player_QueueEdur;
        public InputAction @QueueCinos => m_Wrapper.m_Player_QueueCinos;
        public InputAction @QueueSoleis => m_Wrapper.m_Player_QueueSoleis;
        public InputAction @CombineLumes => m_Wrapper.m_Player_CombineLumes;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @CastSpell => m_Wrapper.m_Player_CastSpell;
        public InputAction @CancelSpellCast => m_Wrapper.m_Player_CancelSpellCast;
        public InputAction @Move_X => m_Wrapper.m_Player_Move_X;
        public InputAction @Move_Z => m_Wrapper.m_Player_Move_Z;
        public InputAction @ClickToMove => m_Wrapper.m_Player_ClickToMove;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @QueueEdur.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueEdur;
                @QueueEdur.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueEdur;
                @QueueEdur.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueEdur;
                @QueueCinos.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueCinos;
                @QueueCinos.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueCinos;
                @QueueCinos.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueCinos;
                @QueueSoleis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueSoleis;
                @QueueSoleis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueSoleis;
                @QueueSoleis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQueueSoleis;
                @CombineLumes.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCombineLumes;
                @CombineLumes.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCombineLumes;
                @CombineLumes.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCombineLumes;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @CastSpell.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCastSpell;
                @CastSpell.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCastSpell;
                @CastSpell.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCastSpell;
                @CancelSpellCast.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancelSpellCast;
                @CancelSpellCast.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancelSpellCast;
                @CancelSpellCast.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancelSpellCast;
                @Move_X.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_X;
                @Move_X.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_X;
                @Move_X.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_X;
                @Move_Z.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_Z;
                @Move_Z.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_Z;
                @Move_Z.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove_Z;
                @ClickToMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClickToMove;
                @ClickToMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClickToMove;
                @ClickToMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClickToMove;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @QueueEdur.started += instance.OnQueueEdur;
                @QueueEdur.performed += instance.OnQueueEdur;
                @QueueEdur.canceled += instance.OnQueueEdur;
                @QueueCinos.started += instance.OnQueueCinos;
                @QueueCinos.performed += instance.OnQueueCinos;
                @QueueCinos.canceled += instance.OnQueueCinos;
                @QueueSoleis.started += instance.OnQueueSoleis;
                @QueueSoleis.performed += instance.OnQueueSoleis;
                @QueueSoleis.canceled += instance.OnQueueSoleis;
                @CombineLumes.started += instance.OnCombineLumes;
                @CombineLumes.performed += instance.OnCombineLumes;
                @CombineLumes.canceled += instance.OnCombineLumes;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @CastSpell.started += instance.OnCastSpell;
                @CastSpell.performed += instance.OnCastSpell;
                @CastSpell.canceled += instance.OnCastSpell;
                @CancelSpellCast.started += instance.OnCancelSpellCast;
                @CancelSpellCast.performed += instance.OnCancelSpellCast;
                @CancelSpellCast.canceled += instance.OnCancelSpellCast;
                @Move_X.started += instance.OnMove_X;
                @Move_X.performed += instance.OnMove_X;
                @Move_X.canceled += instance.OnMove_X;
                @Move_Z.started += instance.OnMove_Z;
                @Move_Z.performed += instance.OnMove_Z;
                @Move_Z.canceled += instance.OnMove_Z;
                @ClickToMove.started += instance.OnClickToMove;
                @ClickToMove.performed += instance.OnClickToMove;
                @ClickToMove.canceled += instance.OnClickToMove;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_EscButton;
    private readonly InputAction m_UI_Interact;
    private readonly InputAction m_UI_Tab;
    public struct UIActions
    {
        private @InputMaster m_Wrapper;
        public UIActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @EscButton => m_Wrapper.m_UI_EscButton;
        public InputAction @Interact => m_Wrapper.m_UI_Interact;
        public InputAction @Tab => m_Wrapper.m_UI_Tab;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @EscButton.started -= m_Wrapper.m_UIActionsCallbackInterface.OnEscButton;
                @EscButton.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnEscButton;
                @EscButton.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnEscButton;
                @Interact.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Tab.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTab;
                @Tab.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTab;
                @Tab.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTab;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @EscButton.started += instance.OnEscButton;
                @EscButton.performed += instance.OnEscButton;
                @EscButton.canceled += instance.OnEscButton;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Tab.started += instance.OnTab;
                @Tab.performed += instance.OnTab;
                @Tab.canceled += instance.OnTab;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_OpenDebugMenu;
    public struct DebugActions
    {
        private @InputMaster m_Wrapper;
        public DebugActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenDebugMenu => m_Wrapper.m_Debug_OpenDebugMenu;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @OpenDebugMenu.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnOpenDebugMenu;
                @OpenDebugMenu.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnOpenDebugMenu;
                @OpenDebugMenu.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnOpenDebugMenu;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenDebugMenu.started += instance.OnOpenDebugMenu;
                @OpenDebugMenu.performed += instance.OnOpenDebugMenu;
                @OpenDebugMenu.canceled += instance.OnOpenDebugMenu;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnQueueEdur(InputAction.CallbackContext context);
        void OnQueueCinos(InputAction.CallbackContext context);
        void OnQueueSoleis(InputAction.CallbackContext context);
        void OnCombineLumes(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnCastSpell(InputAction.CallbackContext context);
        void OnCancelSpellCast(InputAction.CallbackContext context);
        void OnMove_X(InputAction.CallbackContext context);
        void OnMove_Z(InputAction.CallbackContext context);
        void OnClickToMove(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnEscButton(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnOpenDebugMenu(InputAction.CallbackContext context);
    }
}
