﻿// Author: Cody Tedrick https://github.com/ctedrick
// MIT License - Copyright (c) 2024 Cody Tedrick

using System;
using JetBrains.Annotations;
using InteractionsToolkit.Core;
using UnityEngine;

namespace InteractionsToolkit.Utility
{
    public class RaycastUtility<T> where T : BaseInteractable
    {
        public event Action<T> OnHoverEnter;
        public event Action<T> OnHoverExit;


        private readonly Transform source;
        private readonly LayerMask layerMask;

        private readonly float range;
        private readonly float radius;
        private readonly bool isSphere;

        private T currentTarget;

        public RaycastUtility(Transform source, LayerMask layerMask, float range)
        {
            this.source = source;
            this.range = range;
            this.layerMask = layerMask;
        }

        [UsedImplicitly]
        public RaycastUtility(Transform source, float range, float radius)
        {
            this.source = source;
            this.range = range;
            this.radius = radius;
            isSphere = true;
        }

        public void FixedUpdate()
        {
            var hitInfo = HitInfo();

            if (hitInfo.transform != null && hitInfo.transform.GetComponent<T>() != null)
            {
                var newTarget = hitInfo.transform.GetComponent<T>();

                if (currentTarget != null && newTarget != currentTarget) OnHoverExit?.Invoke(currentTarget);

                if (newTarget != currentTarget)
                {
                    currentTarget = newTarget;
                    OnHoverEnter?.Invoke(currentTarget);
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    OnHoverExit?.Invoke(currentTarget);
                    currentTarget = null;
                }
            }
        }

        private RaycastHit HitInfo()
        {
            RaycastHit hit;

            if (isSphere)
            {
                if (Physics.SphereCast(source.position, radius, source.TransformDirection(Vector3.forward), out hit, layerMask)) { }
            }
            else
            {
                if (Physics.Raycast(source.position, source.TransformDirection(Vector3.forward), out hit, range, layerMask)) { }
            }

            return hit;
        }
    }
}