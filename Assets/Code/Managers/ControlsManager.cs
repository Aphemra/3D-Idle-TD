﻿using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace Code.Managers
{
    public class ControlsManager : MonoBehaviour
    {
        [SerializeField] private float scrollScale;
        [SerializeField] private Vector2 minMaxFOVZoom;
        [Space]
        [SerializeField] private CinemachineVirtualCamera topDownCamera;
        [SerializeField] private CinemachineVirtualCamera threeDimensionalCamera;

        private bool canZoom = true;
        private bool canPan = true;

        private void Awake()
        {
            if (Game.ControlsManager == null)
                Game.ControlsManager = this;
        }

        private void Update()
        {
            if (canZoom)
                ScrollZoom();

            if (canPan)
                PanView();
        }

        private void ScrollZoom()
        {
            if (Input.mouseScrollDelta.y != 0)
                topDownCamera.m_Lens.FieldOfView = Mathf.Clamp(topDownCamera.m_Lens.FieldOfView - (Input.mouseScrollDelta.y * scrollScale), minMaxFOVZoom.x, minMaxFOVZoom.y);
        }

        private void PanView()
        {
            // Pan view code
        }

        public void SwitchCameras()
        {
            if (topDownCamera.Priority > threeDimensionalCamera.Priority)
                threeDimensionalCamera.Priority += 10;
            else if (topDownCamera.Priority < threeDimensionalCamera.Priority)
                threeDimensionalCamera.Priority -= 10;
        }
        
        #region Getters and Setters

        public void SetCanZoom(bool zoomAllowed)
        {
            canZoom = zoomAllowed;
        }
        
        public void SetCanPan(bool panAllowed)
        {
            canPan = panAllowed;
        }
        
        #endregion
    }
}