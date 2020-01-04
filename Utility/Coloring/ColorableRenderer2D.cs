﻿using System;
using UnityEngine;
using WarWolfWorks.Interfaces;

namespace WarWolfWorks.Utility.Coloring
{
    /// <summary>
    /// To use with <see cref="ColorManager"/> for application of color(s) to sprite renderers.
    /// </summary>
    [RequireComponent(typeof(ColorManager))]
    public sealed class ColorableRenderer2D : MonoBehaviour, IColorable
    {
        [SerializeField]
        private SpriteRenderer[] renderers;

        Color IColorable.ColorApplier { set => Array.ForEach(renderers, r => r.color = value); }

        /// <summary>
        /// Reference of all <see cref="SpriteRenderer"/> assigned to be changed.
        /// </summary>
        public SpriteRenderer[] Renderers
        {
            get => renderers;
            set => renderers = value;
        }

        private void Awake()
        {
            GetComponent<ColorManager>().AddColorable(this);
        }
    }
}