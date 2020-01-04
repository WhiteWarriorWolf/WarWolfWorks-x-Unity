﻿using System;
using WarWolfWorks.EntitiesSystem;

namespace WarWolfWorks.Interfaces
{
    /// <summary>
    /// Interface used for parent-like behaviour for entities and their components.
    /// </summary>
    public interface IEntityParentable
    {
        /// <summary>
        /// The entity's parent.
        /// </summary>
        Entity Parent { get; set; }

        /// <summary>
        /// Invokes when the Parent is set. T1 is child, T2 is Parent.
        /// </summary>
        event Action<Entity, Entity> OnParentSet;
    }
}