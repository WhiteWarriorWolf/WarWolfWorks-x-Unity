﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WarWolfWorks.Utility;

namespace WarWolfWorks.UI.MenusSystem.Assets
{
    /// <summary>
    /// Premade <see cref="IndexEvent"/> with basic utilities.
    /// </summary>
    public abstract class BasicIndexEvent : IndexEvent
    {
        /// <summary>
        /// Graphics assigned through the inspector.
        /// </summary>
        [SerializeField]
        public List<MaskableGraphic> Graphics;

        /// <summary>
        /// The speed at which the color transition is performed.
        /// </summary>
        [Range(0.1f, 10f)]
        public float ColorTransitionSpeed = 2;
        /// <summary>
        /// The speed at which the size transition is performed.
        /// </summary>
        [Range(0.1f, 10f)]
        public float SizeTransitionSpeed = 2;

        [SerializeField]
        private bool s_TColor, s_TAnchors;
       
        /// <summary>
        /// Color used with <see cref="OnFocused"/>.
        /// </summary>
        public Color FocusedColor = default;
        /// <summary>
        /// Color used with <see cref="OnUnfocused"/>.
        /// </summary>
        public Color UnfocusedColor = default;
        /// <summary>
        /// The size used with <see cref="OnFocused"/>.
        /// </summary>
        public Vector4 FocusedAnchors = default;
        /// <summary>
        /// The size used with <see cref="OnUnfocused"/>
        /// </summary>
        public Vector4 UnfocusedAnchors = default;
        
        /// <summary>
        /// Returns true if the mouse is currently inside this <see cref="IndexEvent"/>'s graphic.
        /// </summary>
        public bool MouseIsInside { get; private set; }

        /// <summary>
        /// Returns true if this <see cref="IndexEvent"/> is currently focused by it's parent though <see cref="IndexMenu.MenuIndex"/>.
        /// </summary>
        public bool Focused { get; private set; }

        /// <summary>
        /// The color towards which the graphs will go to.
        /// </summary>
        public Color DestinationColor { get; private set; }

        /// <summary>
        /// The size towards which the graphs will go to.
        /// </summary>
        public Vector4 DestinationAnchors { get; private set; }

        /// <summary>
        /// The color transition active state.
        /// </summary>
        public bool UsesColorTransition { get => s_TColor; set => s_TColor = value; }
        /// <summary>
        /// The anchors transition active state.
        /// </summary>
        public bool UsesAnchorsTransition { get => s_TAnchors; set => s_TAnchors = value; }

        /// <summary>
        /// Invokes this <see cref="IndexEvent"/>'s activation.
        /// </summary>
        protected override void EventOnPointerClick()
        {
            Activate(0);
        }

        /// <summary>
        /// When overriding, make sure to include "base.Awake();" as it sets the <see cref="DestinationColor"/> to an appropriate color.
        /// </summary>
        protected virtual void Awake()
        {
            DestinationColor = UnfocusedColor;
            DestinationAnchors = UnfocusedAnchors;
        }

        /// <summary>
        /// Sets the index of the parent to this <see cref="IndexEvent"/>'s index; Make sure to include base.EventOnPointerEnter() at the start of the method when overriding
        /// to make this work properly.
        /// </summary>
        protected override void EventOnPointerEnter()
        {
            MouseIsInside = true;
            Parent.MenuIndex = IndexInMenu;
        }

        /// <summary>
        /// Make sure to include base.EventOnPointerExit() at the start of the method when overriding
        /// to make this work properly.
        /// </summary>
        protected override void EventOnPointerExit()
        {
            MouseIsInside = false;
        }

        /// <summary>
        /// Calls <see cref="OnFocused"/> or <see cref="OnUnfocused"/> based on the Parent's <see cref="IndexMenu.MenuIndex"/>.
        /// </summary>
        protected override void OnIndexChanged()
        {
            if (IndexInMenu == Parent.MenuIndex && !Focused)
                OnFocused();
            else if(Focused && IndexInMenu != Parent.MenuIndex) OnUnfocused();
        }

        /// <summary>
        /// When overriding, make sure to include "base.Update();" as it takes care of color and size lerping.
        /// </summary>
        protected virtual void Update()
        {
            for (int i = 0; i < Graphics.Count; i++)
            {
                if (s_TColor)
                {
                    Graphics[i].color = Hooks.Colors.MoveTowards(Graphics[i].color, DestinationColor, ColorTransitionSpeed * Time.deltaTime);
                }
                if (s_TAnchors)
                {
                    Graphics[i].rectTransform.SetAnchoredUI(Vector4.MoveTowards(Graphics[i].rectTransform.GetAnchoredPosition(),
        DestinationAnchors, SizeTransitionSpeed * Time.deltaTime));
                }
            }
        }

        /// <summary>
        /// Invoked when this <see cref="IndexEvent"/> is first focused.
        /// </summary>
        protected virtual void OnFocused()
        {
            DestinationColor = FocusedColor;
            DestinationAnchors = FocusedAnchors;
            Focused = true;
        }

        /// <summary>
        /// Invoked when this <see cref="IndexEvent"/> looses focus.
        /// </summary>
        protected virtual void OnUnfocused()
        {
            DestinationColor = UnfocusedColor;
            DestinationAnchors = UnfocusedAnchors;
            Focused = false;
        }
    }
}
