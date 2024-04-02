// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx
using System;
using System.Collections.Generic;
using System.Reflection;
using UltEvents;
using UnityEngine;
public class MeanTween : MonoBehaviour
{
    [Serializable]
    public class UpdateEventVector : UltEvent<Vector3> { };

    public enum SPACE
    {
        Local,
        Global
    }
    public enum LOOPTYPE
    {
        Once,
        Restart,
        PingPong
    }
    public enum TWEENTYPE { Move, Rotate, Scale, SpriteAlpha, SpriteColor, ComponentFieldValue };
    public enum AROUND { x, y, z };
    public enum VALUETYPE { FloatValue, Vector3Value };

    [HideInInspector]
    public int selectedComponent = 0;

    [HideInInspector]
    public int selectedField = 0;
    [HideInInspector]
    public FieldInfo fieldInfo;
    [HideInInspector]
    public PropertyInfo propertyInfo;

    [SerializeField]
    public string tweenName = "Tween1";

    [SerializeField]
    public GameObject objectToTween;

    [SerializeField]
    public TWEENTYPE tweenType = TWEENTYPE.Move;

    [SerializeField]
    public bool spline = false;

    [SerializeField]
    public bool rotateAroundAxis = false;
    [SerializeField]
    public AROUND axis = AROUND.x;

    [SerializeField]
    public float degrees = 360;

    [SerializeField]
    public SPACE space = SPACE.Local;
    [SerializeField]
    public LeanTweenType easeType = LeanTweenType.easeInOutCubic;

    [SerializeField]
    public bool additive = false;
    [SerializeField]
    public Vector3 target;

    [SerializeField]
    public Color color;

    [SerializeField]
    public float alpha;

    [SerializeField]
    public float value;

    [SerializeField]
    public Vector2 vector2Value;

    [SerializeField]
    public Component component;

    [SerializeField]
    public List<Vector3> splinePositions = new List<Vector3>();
    [SerializeField]
    public float duration = 2;

    [SerializeField]
    public bool playOnAwake = true;
    [SerializeField]
    public LOOPTYPE loopType = LOOPTYPE.Once;
    [SerializeField]
    public bool infiniteLoop = true;
    [SerializeField]
    public int loops = 2;
    [SerializeField]
    public bool ignoreTimeScale;
    [SerializeField]
    public UltEvent onStart;
    [SerializeField]
    public UpdateEventVector onUpdate;
    [SerializeField]
    public UltEvent onComplete;
    MethodInfo pushNewTween;

    private LTDescr tween;

    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

    void Reset()
    {
        objectToTween = gameObject;
        tweenName = "Tween " + (Array.IndexOf(GetComponents<MeanTween>(), this) + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        pushNewTween = typeof(LeanTween).GetMethod("pushNewTween", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        if (playOnAwake)
        {
            Animate();
        }
    }


    void UpdateVector(Vector3 vector)
    {

        if (tweenType == TWEENTYPE.ComponentFieldValue)
        {
            if (fieldInfo != null)
            {
                if (fieldInfo.FieldType == typeof(float))
                {
                    fieldInfo.SetValue(component, vector.x);
                }
                else if (fieldInfo.FieldType == typeof(Vector3))
                {
                    fieldInfo.SetValue(component, vector);
                }
                else if (fieldInfo.FieldType == typeof(Vector2))
                {
                    fieldInfo.SetValue(component, new Vector2(vector.x, vector.y));
                }
            }
            else if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(float))
                {
                    propertyInfo.SetValue(component, vector.x);
                }
                else if (propertyInfo.PropertyType == typeof(Vector3))
                {
                    propertyInfo.SetValue(component, vector);
                }
                else if (propertyInfo.PropertyType == typeof(Vector2))
                {
                    propertyInfo.SetValue(component, new Vector2(vector.x, vector.y));
                }
            }
        }

    }

    public void Animate()
    {
        tween = LeanTween.options();

        pushNewTween.Invoke(this, new object[] { objectToTween, target, duration, tween });

        tween.setTo(target)
            .setTime(duration)
            .setEase(easeType)
            .setOnStart(() => { onStart.Invoke(); })
            .setOnUpdate((Vector3 vector) => { onUpdate.Invoke(vector); UpdateVector(vector); })
            .setOnComplete(() => { onComplete.Invoke(); })
            .setOnCompleteOnRepeat(true)
            .setIgnoreTimeScale(ignoreTimeScale);

        if (infiniteLoop)
        {
            loops = -1;
        }

        if (loopType == LOOPTYPE.Once)
        {
            tween.setLoopOnce();
        }
        else if (loopType == LOOPTYPE.Restart)
        {
            tween.setLoopClamp(loops);
            tween.setLoopClamp();
        }
        else if (loopType == LOOPTYPE.PingPong)
        {
            tween.setLoopPingPong(loops);
            tween.setLoopPingPong();
        }

        if (tweenType == TWEENTYPE.SpriteColor)
        {
            tween.to = new Vector3(1.0f, color.a, 0.0f);
            SpriteRenderer ren = objectToTween.GetComponent<SpriteRenderer>();
            if (ren == null)
            {
                Debug.LogError("No SpriteRenderer on Gameobject " + objectToTween.name);
            }
            else
            {
                tween.spriteRen = ren;
                tween.setColor().setPoint(new Vector3(color.r, color.g, color.b));
            }

        }
        else if (tweenType == TWEENTYPE.SpriteAlpha)
        {
            tween.to = new Vector3(alpha, 0, 0);
            SpriteRenderer ren = objectToTween.GetComponent<SpriteRenderer>();
            if (ren == null)
            {
                Debug.LogError("No SpriteRenderer on Gameobject " + objectToTween.name);
            }
            else
            {
                tween.spriteRen = ren;
                tween.from = new Vector3(ren.color.a, 0, 0);
                tween.setAlpha();
            }
        }
        else if (tweenType == TWEENTYPE.ComponentFieldValue)
        {
            tween.setValue3();
        }
        else
        {
            if (space == SPACE.Global)
            {
                if (tweenType == TWEENTYPE.Move)
                {
                    if (spline)
                    {
                        splinePositions.Insert(0, objectToTween.transform.position);
                        tween.optional.spline = new LTSpline(splinePositions.ToArray());
                        tween.setMoveSpline();
                    }
                    else
                    {
                        if (additive)
                        {
                            tween.to = objectToTween.transform.position + tween.to;
                        }
                        tween.setMove();
                    }

                }
                else if (tweenType == TWEENTYPE.Rotate)
                {
                    if (rotateAroundAxis)
                    {
                        if (axis == AROUND.x)
                        {
                            tween.setAxis(Vector3.right);
                        }
                        else if (axis == AROUND.y)
                        {
                            tween.setAxis(Vector3.up);
                        }
                        else if (axis == AROUND.z)
                        {
                            tween.setAxis(Vector3.forward);
                        }

                        tween.to = new Vector3(degrees, 0, 0);
                        tween.setRotateAround();
                    }
                    else
                    {
                        if (additive)
                        {
                            tween.to = objectToTween.transform.rotation.eulerAngles + tween.to;
                        }
                        tween.setRotate();
                    }

                }
                else if (tweenType == TWEENTYPE.Scale)
                {
                    if (additive)
                    {
                        tween.to = objectToTween.transform.localScale + tween.to;
                    }
                    tween.setScale();

                }

            }
            else if (space == SPACE.Local)
            {
                if (tweenType == TWEENTYPE.Move)
                {
                    if (spline)
                    {
                        splinePositions.Insert(0, objectToTween.transform.localPosition);
                        tween.optional.spline = new LTSpline(splinePositions.ToArray());
                        tween.setMoveSplineLocal();
                    }
                    else
                    {
                        if (additive)
                        {
                            tween.to = objectToTween.transform.localPosition + tween.to;
                        }
                        tween.setMoveLocal();
                    }
                }
                else if (tweenType == TWEENTYPE.Rotate)
                {
                    if (rotateAroundAxis)
                    {
                        if (axis == AROUND.x)
                        {
                            tween.setAxis(Vector3.right);
                        }
                        else if (axis == AROUND.y)
                        {
                            tween.setAxis(Vector3.up);
                        }
                        else if (axis == AROUND.z)
                        {
                            tween.setAxis(Vector3.forward);
                        }
                        tween.to = new Vector3(degrees, 0, 0);
                        tween.setRotateAroundLocal();
                    }
                    else
                    {
                        if (additive)
                        {
                            tween.to = objectToTween.transform.localRotation.eulerAngles + tween.to;
                        }
                        tween.setRotateLocal();
                    }
                }
                else if (tweenType == TWEENTYPE.Scale)
                {
                    if (additive)
                    {
                        tween.to = objectToTween.transform.localScale + tween.to;
                    }
                    tween.setScale();

                }

            }

        }

    }

    public void Cancel()
    {
        LeanTween.cancel(gameObject);
    }

    public void Pause()
    {
        LeanTween.pause(gameObject);
    }

    public void Resume()
    {
        LeanTween.resume(gameObject);
    }

}