using JetBrains.Annotations;
using InteractionsToolkit.Poser;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionsToolkit.Core
{
    public abstract class BaseInteractor : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference primary;

        [Header("Manager")]
        public InteractionManager interactionManager;

        [Header("Properties")]
        [SerializeField] private PoseData defaultPose;
        [SerializeField] private PoserHand poserHand;

        public PoseData DefaultPose => defaultPose;

        private bool isButtonPressed;
        private bool isButtonDown;

        protected virtual void Awake()
        {
            if (primary)
            {
                primary.action.Enable();
                primary.action.performed += context =>
                {
                    isButtonPressed = true;
                    TryToInteract();
                };
                primary.action.canceled += context =>
                {
                    isButtonPressed = false;
                    TryToInteract();
                };
            }
        }

        private void Start()
        {
            if (!interactionManager)
            {
                Debug.LogError($"{nameof(InteractionManager)} is null. Add reference on {gameObject.name}");
                return;
            }

            interactionManager.RegisterInteractor(this);

            if (defaultPose && poserHand) poserHand.SetPose(defaultPose);
        }

        protected void HandleHoverEnter(BaseInteractable interactable)
        {
            interactionManager.HandleHoverEnter(this, interactable);
        }

        protected void HandleHoverExit(BaseInteractable interactable)
        {
            interactionManager.HandleHoverExit(this, interactable);
        }

        private void TryToInteract()
        {
            if (isButtonPressed && !isButtonDown)
            {
                isButtonDown = true;
                interactionManager.TryToInteract(this);
            }
            else if (!isButtonPressed && isButtonDown)
            {
                isButtonDown = false;
                interactionManager.TryToInteract(this);
            }

        }

        /// <summary>
        /// Called when interact button is pressed but no valid targets exist
        /// </summary>
        public void HandleInteractionPressedWithNoValidTargets()
        {
            // trigger pose, event, etc...
            print($"{name} notified of idle selection");
        }

        [UsedImplicitly]
        public void NotifySelectEnter(BaseInteractable interactable)
        {
            //PoserManager.Instance.ApplyPose(poserHand, defaultPose);
            Rigidbody iRigidBody = interactable.GetComponent<Rigidbody>();
            iRigidBody.isKinematic = true;
            iRigidBody.useGravity = false;



        }

        [UsedImplicitly]
        public void NotifySelectExit(BaseInteractable interactable)
        {
            print($"{name} notified of selection exit");
            Debug.Log(interactable.gameObject.name);
            Rigidbody iRigidBody = interactable.GetComponent<Rigidbody>();
            iRigidBody.isKinematic = false;
            iRigidBody.useGravity = true;
        }
    }
}