﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WarWolfWorks.Constants;
using WarWolfWorks.Interfaces.NyuEntities;
using WarWolfWorks.Security;
using WarWolfWorks.Utility;
using WarWolfWorks.NyuEntities.Statistics;

namespace WarWolfWorks.NyuEntities
{
    /// <summary>
    /// Manages all entities, as well as provides a substantial amount of utility methods to retrieve entities.
    /// </summary>
    public static class NyuManager
    {
        #region Multi-Threading
        private class MonoUpdateSimulate : MonoBehaviour
        {
            [HideInInspector]
            private bool GeneratesError = true;

            private void Awake()
            {
                Application.quitting += Event_StopErrorGenerate;
            }

            private void Event_StopErrorGenerate()
            {
                GeneratesError = false;
                Application.quitting -= Event_StopErrorGenerate;
            }

            private void OnDestroy()
            {
                if (GeneratesError)
                    throw new NyuEntityException(6);
            }
        }

        private static MonoUpdateSimulate UpdateInvoker;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            GameObject gUpdateInvoker = new GameObject(VARN_NYUMANAGER);
            UpdateInvoker = gUpdateInvoker.AddComponent<MonoUpdateSimulate>();
            UnityEngine.Object.DontDestroyOnLoad(UpdateInvoker);
            UpdateInvoker.StartCoroutine(IC_Update());
            UpdateInvoker.StartCoroutine(IC_FixedUpdate());
            UpdateInvoker.StartCoroutine(IC_LateUpdate());
        }

        /// <summary>
        /// Invokes the update method of all entities.
        /// </summary>
        private static IEnumerator IC_Update()
        {
            while (true)
            {
                yield return null;

                for (int i = 0; i < AllEntities.Count; i++)
                {
                    if (!AllEntities[i].enabled)
                        continue;

                    if (AllEntities[i] is INyuUpdate entityUpdate)
                        entityUpdate.NyuUpdate();

                    for (int j = 0; j < AllEntities[i].hs_Components.Count; j++)
                    {
                        if (AllEntities[i].hs_Components[j] is INyuUpdate nyuUpdate)
                            nyuUpdate.NyuUpdate();
                    }
                }
            }
        }
        /// <summary>
        /// Invokes the FixedUpdate method of all entities.
        /// </summary>
        /// <returns></returns>
        private static IEnumerator IC_FixedUpdate()
        {
            while (true)
            {
                yield return FixedUpdateWaiter;

                for (int i = 0; i < AllEntities.Count; i++)
                {
                    if (!AllEntities[i].enabled)
                        continue;

                    if (AllEntities[i] is INyuFixedUpdate entityFixed)
                        entityFixed.NyuFixedUpdate();

                    for (int j = 0; j < AllEntities[i].hs_Components.Count; j++)
                    {
                        if (AllEntities[i].hs_Components[j] is INyuFixedUpdate nyuFixed)
                            nyuFixed.NyuFixedUpdate();
                    }
                }
            }
        }
        /// <summary>
        /// Invokes the LateUpdate method of all entities.
        /// </summary>
        /// <returns></returns>
        private static IEnumerator IC_LateUpdate()
        {
            while (true)
            {
                yield return LateUpdateWaiter;

                for (int i = 0; i < AllEntities.Count; i++)
                {
                    if (!AllEntities[i].enabled)
                        continue;

                    if (AllEntities[i] is INyuLateUpdate entityLate)
                        entityLate.NyuLateUpdate();

                    for (int j = 0; j < AllEntities[i].hs_Components.Count; j++)
                    {
                        if (AllEntities[i].hs_Components[j] is INyuLateUpdate nyuLate)
                            nyuLate.NyuLateUpdate();
                    }
                }
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Finds an entity by match.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static Nyu Find(Predicate<Nyu> match)
        {
            for (int i = 0; i < AllEntities.Count; i++)
                if (match(AllEntities[i]))
                    return AllEntities[i];

            return null;
        }

        /// <summary>
        /// Finds all entities that match the given condition.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<Nyu> FindAll(Predicate<Nyu> match)
        {
            List<Nyu> toReturn = new List<Nyu>(AllEntities.Count);
            for (int i = 0; i < AllEntities.Count; i++)
                if (match(AllEntities[i]))
                    toReturn.Add(AllEntities[i]);

            return toReturn;
        }

        #region Get Visible
        /// <summary>
        /// Gets all visible <see cref="Nyu"/> entities to a given camera.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static List<Nyu> GetAllVisible(Camera to)
        {
            List<Nyu> toReturn = new List<Nyu>(AllEntities.Count);
            for (int i = 0; i < AllEntities.Count; i++)
            {
                Renderer[] renderers = AllEntities[i].GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    if (Hooks.Rendering.IsVisibleFrom(renderer, to))
                    {
                        toReturn.Add(AllEntities[i]);
                        break;
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets all visible <see cref="Nyu"/> entities to a given camera within the specified max distance.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="within"></param>
        /// <returns></returns>
        public static List<Nyu> GetAllVisible(Camera to, float within)
        {
            List<Nyu> toReturn = new List<Nyu>(AllEntities.Count);
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (Vector3.Distance(to.transform.position, AllEntities[i].Position) < within)
                {
                    Renderer[] renderers = AllEntities[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        if (Hooks.Rendering.IsVisibleFrom(renderer, to))
                        {
                            toReturn.Add(AllEntities[i]);
                            break;
                        }
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets all visible <see cref="Nyu"/> entities of T type to a given camera.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to"></param>
        /// <returns></returns>
        public static List<T> GetAllVisible<T>(Camera to) where T : Nyu
        {
            List<T> toReturn = new List<T>(AllEntities.Count);
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i] is T tNyu)
                {
                    Renderer[] renderers = AllEntities[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        if (Hooks.Rendering.IsVisibleFrom(renderer, to))
                        {
                            toReturn.Add(tNyu);
                            break;
                        }
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets all visible <see cref="Nyu"/> entities of T type to a given camera within a specified max range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to"></param>
        /// <param name="within"></param>
        /// <returns></returns>
        public static List<T> GetAllVisible<T>(Camera to, float within) where T : Nyu
        {
            List<T> toReturn = new List<T>(AllEntities.Count);
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i] is T tNyu && Vector3.Distance(to.transform.position, tNyu.Position) <= within)
                {
                    Renderer[] renderers = AllEntities[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        if (Hooks.Rendering.IsVisibleFrom(renderer, to))
                        {
                            toReturn.Add(tNyu);
                            break;
                        }
                    }
                }
            }

            return toReturn;
        }
        #endregion

        #region Non-Generic Get Closest Methods
        /// <summary>
        /// Returns the closest entity to a given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Nyu GetClosest(Vector3 position)
        {
            int index = -1;
            float lastDist = float.PositiveInfinity;
            for(int i = 0; i < AllEntities.Count; i++)
            {
                float curDist = Vector3.Distance(AllEntities[i].Position, position);
                if (curDist < lastDist)
                    index = i;
            }

            return index < 0 ? null : AllEntities[index];
        }

        /// <summary>
        /// Gets the closest <see cref="Nyu"/> to a given position within a specified max range.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="within"></param>
        /// <returns></returns>
        public static Nyu GetClosest(Vector3 position, float within)
        {
            int index = -1;
            float lastDist = within;
            for (int i = 0; i < AllEntities.Count; i++)
            {
                float curDist = Vector3.Distance(AllEntities[i].Position, position);
                if (curDist < lastDist)
                    index = i;
            }

            return index < 0 ? null : AllEntities[index];
        }

        /// <summary>
        /// Gets the closest <see cref="Nyu"/> of given type to a given position within a specified max range.
        /// (Note: <see cref="Nyu"/> entities assignable from the given type are also counted.)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="within"></param>
        /// <param name="compareType"></param>
        /// <returns></returns>
        public static Nyu GetClosest(Vector3 position, float within, Type compareType)
        {
            int index = -1;
            float lastDist = within;
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i].GetType().IsAssignableFrom(compareType))
                {
                    float curDist = Vector3.Distance(AllEntities[i].Position, position);
                    if (curDist < lastDist)
                        index = i;
                }
            }

            return index < 0 ? null : AllEntities[index];
        }

        /// <summary>
        /// Gets the closest <see cref="Nyu"/> of given types to the given position within a specified max range.
        /// (Note: <see cref="Nyu"/> entities assignable from the given types are also counted.)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="within"></param>
        /// <param name="compareTypes"></param>
        /// <returns></returns>
        public static Nyu GetClosest(Vector3 position, float within, params Type[] compareTypes)
        {
            int index = -1;
            float lastDist = within;
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (Array.Exists(compareTypes, t => AllEntities[i].GetType().IsAssignableFrom(t)))
                {
                    float curDist = Vector3.Distance(AllEntities[i].Position, position);
                    if (curDist < lastDist)
                        index = i;
                }
            }

            return index < 0 ? null : AllEntities[index];
        }
        #endregion

        #region Generic Get Closest Methods
        /// <summary>
        /// Returns the closest <see cref="Nyu"/> of T type to a given position.
        /// </summary>
        /// <typeparam name="T"><see cref="Nyu"/>'s type searched.</typeparam>
        /// <param name="position"></param>
        /// <returns></returns>
        public static T GetClosest<T>(Vector3 position) where T : Nyu
        {
            T toReturn = null;
            float lastDist = float.PositiveInfinity;
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i] is T tNyu)
                {
                    float curDist = Vector3.Distance(tNyu.Position, position);
                    if (curDist < lastDist)
                        toReturn = tNyu;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the closest <see cref="Nyu"/> of T type to a given position within a specified max distance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="within"></param>
        /// <returns></returns>
        public static T GetClosest<T>(Vector3 position, float within) where T : Nyu
        {
            T toReturn = null;
            float lastDist = within;
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i] is T tNyu)
                {
                    float curDist = Vector3.Distance(tNyu.Position, position);
                    if (curDist < lastDist)
                        toReturn = tNyu;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets all <see cref="Nyu"/> enitities of given T type within the given range of position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="within"></param>
        /// <returns></returns>
        public static List<T> GetAllWithin<T>(Vector3 position, float within) where T : Nyu
        {
            List<T> toReturn = new List<T>();
            for (int i = 0; i < AllEntities.Count; i++)
            {
                if (AllEntities[i] is T tNyu)
                {
                    float curDist = Vector3.Distance(tNyu.Position, position);
                    if (curDist <= within)
                        toReturn.Add(tNyu);
                }
            }

            return toReturn;
        }
        #endregion

        /// <summary>
        /// Returns the oldest parent of an Entity with <see cref="INyuEntityParentable"/> interface.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="includeNonParentableParent"></param>
        /// <returns></returns>
        public static Nyu OldestOf(Nyu entity, bool includeNonParentableParent)
        {
            Nyu toReturn = entity;
            while (toReturn is INyuEntityParentable)
            {
                INyuEntityParentable par = (INyuEntityParentable)toReturn;
                if (par.NyuParent == null || (!includeNonParentableParent && !(par.NyuParent is INyuEntityParentable)))
                    break;

                toReturn = par.NyuParent;
            }

            return toReturn;
        }
        #endregion

        #region Instantiating / Destroying
        internal static List<Nyu> AllEntities = new List<Nyu>();

        /// <summary>
        /// Invoked when an entity is instantiated through any <see cref="NyuManager"/>.New method.
        /// </summary>
        public static event Action<Nyu> OnEntityBegin;

        /// <summary>
        /// Invoked when an entity is destroyed through <see cref="Destroy(Nyu)"/>. 
        /// (Does not get invoked when the entity is destroyed unofficially.)
        /// </summary>
        public static event Action<Nyu> OnEntityEnd;

        internal static void CallEntityBegin(Nyu of) => OnEntityBegin?.Invoke(of);

        /// <summary>
        /// Official method to instantiate a new <see cref="Nyu"/>.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Nyu New(Nyu prefab, Vector3 position, Quaternion rotation)
        {
            Nyu toReturn = UnityEngine.Object.Instantiate(prefab, position, rotation);

            toReturn.Stats = new Stats(toReturn);
            toReturn.hs_Components = new List<INyuComponent>(toReturn.GetComponents<INyuComponent>());

            toReturn.CallInit();
            AllEntities.Add(toReturn);

            OnEntityBegin?.Invoke(toReturn);

            return toReturn;
        }

        /// <summary>
        /// Official method to instantiate a new <see cref="Nyu"/>.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static T New<T>(T prefab, Vector3 position, Quaternion rotation) where T : Nyu
        {
            T toReturn = UnityEngine.Object.Instantiate(prefab, position, rotation);

            toReturn.Stats = new Stats(toReturn);
            toReturn.hs_Components = new List<INyuComponent>(toReturn.GetComponents<INyuComponent>());

            toReturn.CallInit();
            AllEntities.Add(toReturn);

            OnEntityBegin?.Invoke(toReturn);

            return toReturn;
        }

        /// <summary>
        /// Checks if an entity implements <see cref="INyuOnEnable"/> and/or <see cref="INyuOnDisable"/> and throws and exception if it does.
        /// </summary>
        /// <param name="entity"></param>
        internal static void Exception3Check(Nyu entity)
        {
            if (entity is INyuOnEnable || entity is INyuOnDisable)
                throw new NyuEntityException(3);
        }

        /// <summary>
        /// Official method to destroy an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Destroy(Nyu entity)
        {
            if (entity == null)
                return false;

            if (entity is INyuOnDestroyQueued entityDestroyQueued)
                entityDestroyQueued.NyuOnDestroyQueued();

            for (int i = 0; i < entity.hs_Components.Count; i++)
            {
                if (entity.hs_Components[i] is INyuOnDestroyQueued queue)
                    queue.NyuOnDestroyQueued();
            }


            AllEntities.Remove(entity);

            for (int i = entity.hs_Components.Count - 1; i >= 0; i--)
            {
                if (entity.hs_Components[i] is INyuOnDestroy destroy)
                    destroy.NyuOnDestroy();

                entity.hs_Components.RemoveAt(i);
            }

            if (entity is INyuOnDestroy entityDestroy)
                entityDestroy.NyuOnDestroy();
            entity.ns_DestroyedCorrectly = true;
            OnEntityEnd?.Invoke(entity);
            UnityEngine.Object.Destroy(entity.gameObject);

            return true;
        }
        #endregion
    }
}