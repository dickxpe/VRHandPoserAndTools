using InteractionsToolkit.Utility;
using UnityEngine;

namespace InteractionsToolkit.Core
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        public bool AllowMultipleSelection { get; protected set; }

        public bool ObjectToHand = true;

        protected virtual void Start()
        {
            var interactionManager = FindObjectOfType<InteractionManager>();
            if (interactionManager) interactionManager.RegisterInteractable(this);
        }

        public void HandleHoverEnter(BaseInteractor interactor)
        {
            print($"{interactor.name} is hovering on {name}");
        }

        public void HandleHoverExit(BaseInteractor interactor)
        {
            print($"{interactor.name} is no longer hovering on {name}");
        }

        public virtual void HandleSelectEnter(BaseInteractor interactor)
        {

            // Vector3 finalPosition = interactor.transform.position * -1f;
            // Quaternion finalRotation = Quaternion.Inverse(interactor.transform.rotation);


            // finalPosition.RotatePointAroundPivot(Vector3.zero, finalRotation.eulerAngles);

            // transform.localPosition = finalPosition;
            // transform.localRotation = finalRotation;

        }

        public virtual void HandleSelectExit(BaseInteractor interactor)
        {
            print($"{interactor.name} select exited {name}");
        }
    }
}