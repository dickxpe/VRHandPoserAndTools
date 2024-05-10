// Author: Cody Tedrick https://github.com/ctedrick
// Edited by: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Cody Tedrick - Copyright (c) 2024 Peter Dickx
using System.Collections;
using InteractionsToolkit.Poser;
using UnityEngine;

namespace InteractionsToolkit.Core
{
    public class GrabInteractable : BaseInteractable
    {
        PoserHand poserHand;
        bool followHand = false;
        protected static void DetachInteractable(Transform interactable) => interactable.SetParent(null);

        protected void SetPose(PoserHand poserHand, PoseData poseData, float poseDuration)
        {
            this.poserHand = poserHand;
            HandPoseJoints pose;
            PoseTransform parentTransform;
            if (poseData != null)
            {
                if (poserHand.Type == Handedness.Left)
                {
                    pose = poseData.LeftJoints;
                    parentTransform = poseData.LeftParentTransform;
                }
                else
                {

                    pose = poseData.RightJoints;
                    parentTransform = poseData.RightParentTransform;
                }

                if (ObjectToHand)
                {
                    ApplyAttachmentTransform(parentTransform, poserHand.AttachTransform);
                    poserHand.SetPose(pose, poseDuration);

                    transform.SetParent(poserHand.AttachTransform);

                    StopAllCoroutines();
                    StartCoroutine(AttachHandRoutine(transform, Vector3.zero, Quaternion.identity, poseDuration));
                }
                else
                {

                    SkinnedMeshRenderer skinnedRenderer = poserHand.GetComponentInChildren<SkinnedMeshRenderer>();
                    skinnedRenderer.enabled = false;
                    MeshRenderer[] renderers = poserHand.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer renderer in renderers)
                    {
                        renderer.enabled = false;
                    }
                    followHand = true;
                    // transform.SetParent(poserHand.AttachTransform);
                    poserHand.ghostHand.gameObject.SetActive(true);
                    poserHand.ghostHand.transform.SetParent(null);
                    poserHand.ghostHand.SetPose(pose, poseDuration);
                    if (attachTransform)
                    {
                        poserHand.ghostHand.GetComponent<GhostHand>().followObject = attachTransform;
                    }
                    else
                    {
                        poserHand.ghostHand.GetComponent<GhostHand>().followObject = colliders[0].transform;
                    }
                    // poserHand.ghostHand.transform.SetParent(transform, true);
                    ApplyHandTransform(poserHand.ghostHand.transform, poserHand.AttachTransform);
                    // StopAllCoroutines();
                    // StartCoroutine(AttachHandRoutine(poserHand.ghostHand.transform, Vector3.zero, Quaternion.identity, poseDuration));

                }
            }
        }

        public void Update()
        {
            if (followHand)
            {
                transform.position = poserHand.transform.position;
                transform.rotation = poserHand.transform.rotation;
            }
        }

        public override void HandleSelectExit(BaseInteractor interactor)
        {
            base.HandleSelectExit(interactor);
            followHand = false;
        }

        protected static IEnumerator AttachHandRoutine(Transform target, Vector3 positionEnd, Quaternion rotationEnd, float attachDuration)
        {
            var time = 0f;
            var startPosition = target.localPosition;
            var startRotation = target.localRotation;

            while (time < attachDuration)
            {
                target.localPosition = Vector3.Lerp(startPosition, positionEnd, time / attachDuration);
                target.localRotation = Quaternion.Lerp(startRotation, rotationEnd, time / attachDuration);
                time += Time.deltaTime;
                yield return null;
            }

            target.localPosition = positionEnd;
            target.localRotation = Quaternion.identity;
        }

        private static void ApplyAttachmentTransform(PoseTransform parentTransformData, Transform attachTransform)
        {
            var adjustedPosition = parentTransformData.LocalPosition * -1f;
            var adjustedRotation = Quaternion.Inverse(parentTransformData.LocalRotation);
            adjustedPosition = Quaternion.Euler(adjustedRotation.eulerAngles) * adjustedPosition;

            // Apply offset to hand attach transform
            attachTransform.localPosition = adjustedPosition;
            attachTransform.localRotation = adjustedRotation;
        }

        private static void ApplyHandTransform(Transform parentTransform, Transform attachTransform)
        {
            var adjustedPosition = attachTransform.localPosition * -1f;
            var adjustedRotation = Quaternion.Inverse(attachTransform.localRotation);
            adjustedPosition = Quaternion.Euler(adjustedRotation.eulerAngles) * adjustedPosition;

            // Apply offset to hand attach transform
            parentTransform.localPosition = adjustedPosition;
            parentTransform.localRotation = adjustedRotation;
        }
    }
}