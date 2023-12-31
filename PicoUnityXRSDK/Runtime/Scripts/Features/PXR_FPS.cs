﻿/************************************************************************************
 【PXR SDK】
 Copyright 2015-2020 Pico Technology Co., Ltd. All Rights Reserved.

************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Unity.XR.PXR
{
    public class PXR_FPS : MonoBehaviour
    {
        private Text fpsText;

        private float updateInterval = 0.5f;
        private float accum = 0.0f;
        private int frames = 0;
        private float timeLeft = 0.0f;
        private string strFps = null;

        void Awake()
        {
            fpsText = GetComponent<Text>();
        }

        void Update()
        {
            if (fpsText != null)
            {
                ShowFps();
            }
        }

        private void ShowFps()
        {
            timeLeft -= Time.unscaledDeltaTime;
            accum += Time.unscaledDeltaTime;

            if (timeLeft <= 0.0)
            {
                PXR_Plugin.System.UPxr_GetIntConfig((int)GlobalIntConfigs.RenderFPS, ref frames);
                strFps = string.Format("FPS: {0:f0}", frames);
                fpsText.text = strFps;

                timeLeft += updateInterval;
                accum = 0.0f;
            }
        }
    }
}