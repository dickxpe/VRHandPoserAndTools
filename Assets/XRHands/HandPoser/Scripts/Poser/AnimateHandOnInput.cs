// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionsToolkit.Poser
{
    public class AnimateHandOnInput : MonoBehaviour
    {
        public InputActionProperty pinchAnimationAction;

        public InputActionProperty grabAnimationAction;

        public InputActionProperty thumbAnimationAction;
        public InputActionProperty IndexAnimationAction;

        private PoserHand poserHand;


        public void CloseCompleteHand()
        {
            prevGrabValue = 0;
            prevThumbValue = 0;
            prevTriggerValue = 0;
        }

        public void OpenCompleteHand()
        {
            prevGrabValue = 1;
            prevThumbValue = 1;
            prevTriggerValue = 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            poserHand = GetComponent<PoserHand>();
        }

        [SerializeField] private PoseData openPose;
        [SerializeField] private PoseData closedPose;

        public PoseData OpenPose => openPose;

        float prevTriggerValue = 0;
        float prevGrabValue = 0;
        float prevThumbValue = 0;

        bool[] triggerMask = new bool[] { false, true, false, false, false, false };
        bool[] grabMask = new bool[] { false, false, true, true, true, true };
        bool[] thumbMask = new bool[] { true, false, false, false, false, false };

        float timePassed = 0;

        public PoseData ClosedPose => closedPose;
        // Update is called once per frame
        void Update()
        {

            timePassed += Time.deltaTime;

            if (timePassed >= 0.05f)
            {
                timePassed = 0;
                float triggerValue = IndexAnimationAction.action.ReadValue<float>();

                if (Mathf.Abs(triggerValue - prevTriggerValue) > 0.01f)
                {
                    if (triggerValue > prevTriggerValue)
                    {
                        poserHand.SetMaskedPose(closedPose, triggerMask, triggerValue);

                    }
                    else if (triggerValue < prevTriggerValue)
                    {
                        poserHand.SetMaskedPose(openPose, triggerMask, 1 - triggerValue);
                    }
                }

                float grabValue = grabAnimationAction.action.ReadValue<float>();

                if (Mathf.Abs(grabValue - prevGrabValue) > 0.01f)
                {
                    if (grabValue > prevGrabValue)
                    {
                        poserHand.SetMaskedPose(closedPose, grabMask, grabValue);
                    }
                    else if (grabValue < prevGrabValue)
                    {
                        poserHand.SetMaskedPose(openPose, grabMask, 1 - grabValue);
                    }
                }

                float thumbValue = thumbAnimationAction.action.ReadValue<float>();

                if (Mathf.Abs(thumbValue - prevThumbValue) > 0.01f)
                {
                    if (thumbValue > prevThumbValue)
                    {
                        poserHand.SetMaskedPose(closedPose, thumbMask, thumbValue, 0.1f);

                    }
                    else if (thumbValue < prevThumbValue)
                    {
                        poserHand.SetMaskedPose(openPose, thumbMask, 1 - thumbValue, 0.1f);
                    }
                }

                prevGrabValue = grabValue;
                prevTriggerValue = triggerValue;
                prevThumbValue = thumbValue;

            }
        }
    }
}
