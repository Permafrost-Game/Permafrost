﻿using System;
using System.Collections.Generic;
using System.IO;
using GeonBit.UI.Entities;
using GlobalWarmingGame.UI.Controllers;
using Microsoft.Xna.Framework;

namespace GlobalWarmingGame.UI.Menus
{
    class SettingsMenu : OverlaidOptionMenu
    {
        private const string name = "Settings";

        private void DecrementDisplayScale(float tag)
        {
            float newScale = SettingsManager.ResolutionScale - 0.25f;
            SettingsManager.ResolutionScale = Math.Max(newScale, 0.5f);
            InitaliseButtons();
        }

        private void IncrementDisplayScale(float tag)
        {
            float newScale = SettingsManager.ResolutionScale + 0.25f;
            SettingsManager.ResolutionScale = Math.Min(newScale, 1f);
            InitaliseButtons();
        }

        private float GetDisplayScale()
        {
            return SettingsManager.ResolutionScale;
        }

        private bool GetFullScreen()
        {
            return SettingsManager.Fullscreen;
        }
        private void SetFullScreen(bool value)
        {
            SettingsManager.Fullscreen = !value;
            InitaliseButtons();
        }

        private bool GetDevMode()
        {
            return GameUIController.DevMode;
        }
        private void SetDevMode(bool value)
        {
            SettingsManager.DevMode = !value;
            InitaliseButtons();
        }

        
        private void ApplyGameState(GameState value)
        {
            Game1.GameState = value;
        }


        public SettingsMenu()
            : base(name, anchor: Anchor.Center)
        {
            InitaliseButtons();
        }

        public void InitaliseButtons()
        {
            ClearButtons();
            AddButton(new ButtonHandler<bool>(GetFullScreen(), "FullScreen", SetFullScreen), true);

            AddButton(new ButtonHandler<bool>(GetDevMode(), "DevMode", SetDevMode), true);

            AddButton(new ButtonHandler<float>(GetDisplayScale(), "DisplayScale +", IncrementDisplayScale), true);
            AddButton(new ButtonHandler<float>(GetDisplayScale(), "DisplayScale -", DecrementDisplayScale), true);

            AddButton(new ButtonHandler<GameState>(GameState.Paused, "Done", ApplyGameState), false);
        }

    }
}
