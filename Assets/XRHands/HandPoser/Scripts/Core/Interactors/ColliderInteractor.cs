using UnityEngine;

namespace InteractionsToolkit.Core
{
    public class ColliderInteractor : BaseInteractor
    {
        private Collider triggerCollider;

        protected override void Awake()
        {
            base.Awake();

            if (TryGetComponent(out Collider col))
            {
                triggerCollider = col;
                triggerCollider.isTrigger = true;
            }
            else
            {
                Debug.LogError($"{nameof(triggerCollider)} is missing. Add collider to {gameObject.name}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out BaseInteractable interactable))
            {
                HandleHoverEnter(interactable);
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {
                    HandleHoverEnter(baseInteractable);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out BaseInteractable interactable))
            {
                HandleHoverEnter(interactable);
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {
                    HandleHoverEnter(baseInteractable);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BaseInteractable interactable))
            {
                HandleHoverExit(interactable);
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {
                    HandleHoverExit(baseInteractable);
                }
            }
        }
    }
}