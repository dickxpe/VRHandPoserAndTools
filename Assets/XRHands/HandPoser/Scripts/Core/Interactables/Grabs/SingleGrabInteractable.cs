using InteractionsToolkit.Poser;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace InteractionsToolkit.Core
{
    public class SingleGrabInteractable : GrabInteractable, IHandPose
    {
        [Header("Pose Settings")]
        [SerializeField]
        PoseData pose;

        [Header("Animation Settings")]
        [SerializeField] protected float AttachDuration = 0.2f;

        [SerializeField] bool enableDefaultAnimations = false;

        private PoserHand activeHand;

        public PoseData PrimaryPose { get => pose; set => pose = value; }
        public PoseData SecondaryPose { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public PoserHand GetActiveHand()
        {
            return activeHand;
        }

        public override void HandleSelectEnter(BaseInteractor interactor)
        {

            if (interactor.TryGetComponent(out PoserHand newHand))
            {
                if (activeHand == null)
                {
                    activeHand = newHand;
                    activeHand.SetIsPosing(!enableDefaultAnimations);
                    SetPose(activeHand, pose, AttachDuration);

                }
            }
        }

        public override void HandleSelectExit(BaseInteractor interactor)
        {
            base.HandleSelectExit(interactor);
            SkinnedMeshRenderer skinnedRenderer = activeHand.GetComponentInChildren<SkinnedMeshRenderer>();
            skinnedRenderer.enabled = true;
            MeshRenderer[] renderers = activeHand.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            activeHand.SetPose(interactor.DefaultPose);
            activeHand.SetIsPosing(false);
            activeHand.ResetHandPose();
            if (!ObjectToHand)
            {
                activeHand.ghostHand.transform.SetParent(activeHand.transform.parent);
                activeHand.ghostHand.transform.localPosition = activeHand.transform.localPosition;
                activeHand.ghostHand.transform.localRotation = activeHand.transform.localRotation;
                activeHand.ghostHand.transform.localScale = new Vector3(1, 1, 1);
                activeHand.ghostHand.GetComponent<GhostHand>().followObject = null;
                activeHand.ghostHand.gameObject.SetActive(false);
            }
            else
            {
                DetachInteractable(transform);
            }
            activeHand.AttachTransform.localPosition = Vector3.zero;
            activeHand = null;

        }
    }
}