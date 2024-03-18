using InteractionsToolkit.Poser;
using UnityEngine;

namespace InteractionsToolkit.Core
{
    public class SingleGrabInteractable : GrabInteractable
    {
        [Header("Pose")]
        [SerializeField] private PoseData pose;

        [Header("Animation Settings")]
        [SerializeField] protected float AttachDuration = 0.2f;

        private PoserHand activeHand;

        public override void HandleSelectEnter(BaseInteractor interactor)
        {

            if (interactor.TryGetComponent(out PoserHand newHand))
            {
                if (activeHand == null)
                {
                    activeHand = newHand;
                    activeHand.SetIsPosing(true);
                    SetPose(activeHand, pose, AttachDuration);
                }
            }
        }

        public override void HandleSelectExit(BaseInteractor interactor)
        {
            base.HandleSelectExit(interactor);
            Debug.Log("HandleSelectExit");
            activeHand.SetPose(interactor.DefaultPose);
            activeHand.SetIsPosing(false);
            activeHand.ResetHandPose();
            DetachInteractable(transform);
            activeHand = null;
        }
    }
}