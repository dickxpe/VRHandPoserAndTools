using System.Collections;
using System.Collections.Generic;
using InteractionsToolkit.Core;
using InteractionsToolkit.Poser;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayPoseOnAction : MonoBehaviour
{

    [SerializeField]
    bool RevertOnRelease = false;

    [SerializeField]
    List<ActionPose> actionPoses = new List<ActionPose>();

    PoseData currentPose;


    SingleGrabInteractable singleGrab;
    // Start is called before the first frame update
    void Start()
    {
        singleGrab = GetComponent<SingleGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (singleGrab)
        {
            if (singleGrab.GetActiveHand() != null)
            {
                foreach (ActionPose actionPose in actionPoses)
                {

                    if (actionPose.action.action.triggered)
                    {

                        currentPose = singleGrab.PrimaryPose;
                        if (RevertOnRelease)
                        {
                            singleGrab.GetActiveHand().SetPose(actionPose.pose, 0.2f);
                        }
                        else
                        {
                            singleGrab.GetActiveHand().SetPose(actionPose.pose, 0.2f, RevertPose);
                        }
                        if (actionPose.actionEvent != null)
                        {
                            actionPose.actionEvent.Invoke();
                        }
                    }
                    else if (RevertOnRelease && actionPose.action.action.WasReleasedThisFrame())
                    {
                        actionPose.undoActionEvent.Invoke();
                        singleGrab.GetActiveHand().SetPose(currentPose);
                    }
                }
            }
        }
    }

    void RevertPose()
    {
        foreach (ActionPose actionPose in actionPoses)
        {
            if (actionPose.action.action.triggered)
            {
                actionPose.undoActionEvent.Invoke();
            }
        }
        singleGrab.GetActiveHand().SetPose(currentPose);
    }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
    }
}
