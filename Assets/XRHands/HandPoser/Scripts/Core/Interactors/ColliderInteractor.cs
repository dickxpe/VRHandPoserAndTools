// Author: Cody Tedrick https://github.com/ctedrick
// Edited by: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Cody Tedrick - Copyright (c) 2024 Peter Dickx

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

                if (interactable.colliders != null & interactable.colliders.Contains(other) || interactable.colliders == null)
                {
                    HandleHoverEnter(interactable);
                }
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {
                    if (baseInteractable.colliders != null & baseInteractable.colliders.Contains(other) || baseInteractable.colliders == null)
                    {

                        HandleHoverEnter(baseInteractable);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out BaseInteractable interactable))
            {
                if (interactable.colliders != null & interactable.colliders.Contains(other) || interactable.colliders == null)
                {
                    HandleHoverEnter(interactable);
                }
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {

                    if (baseInteractable.colliders != null & baseInteractable.colliders.Contains(other) || baseInteractable.colliders == null)
                    {
                        HandleHoverEnter(baseInteractable);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BaseInteractable interactable))
            {
                if (interactable.colliders != null & interactable.colliders.Contains(other) || interactable.colliders == null)
                {
                    HandleHoverExit(interactable);
                }
            }
            else
            {
                BaseInteractable baseInteractable = other.gameObject.GetComponentInParent<BaseInteractable>();
                if (baseInteractable)
                {
                    if (baseInteractable.colliders != null & baseInteractable.colliders.Contains(other) || baseInteractable.colliders == null)
                    {
                        HandleHoverExit(baseInteractable);
                    }
                }
            }
        }
    }
}