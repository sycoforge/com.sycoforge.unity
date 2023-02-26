
using System.Collections.Generic;
using UnityEngine;

namespace ch.sycoforge.Unity.Runtime
{

    /// <summary>
    /// Extension methods for UnityEngine.Transform.
    /// </summary>
    public static class TransformExtensions
    {
        public static bool IsNullOrDestroyed(Object obj)
        {
            return obj == null || obj.Equals(null);
        }

        /// <summary>
        /// Finds a child transform by name. Breadth-first search.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public static Transform FindRecursive(this Transform transform, string name)
        {
            var result = transform.Find(name);
            if (result != null) { return result; }

            foreach (Transform child in transform)
            {
                result = child.FindRecursive(name);
                if (result != null) { return result; }
            }

            return null;
        }

        /// <summary>
        /// Makes the given game objects children of the transform.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="children">Game objects to make children.</param>
        public static void AddChildren(this Transform transform, GameObject[] children)
        {
            System.Array.ForEach(children, child => child.transform.parent = transform);
        }

        /// <summary>
        /// Makes the game objects of given components children of the transform.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="children">Components of game objects to make children.</param>
        public static void AddChildren(this Transform transform, Component[] children)
        {
            System.Array.ForEach(children, child => child.transform.parent = transform);
        }

        /// <summary>
        /// Sets the position of a transform's children to zero.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="recursive">Also reset ancestor positions?</param>
        public static void ResetChildPositions(this Transform transform, bool recursive = false)
        {
            foreach (Transform child in transform)
            {
                child.position = Vector3.zero;

                if (recursive)
                {
                    child.ResetChildPositions(recursive);
                }
            }
        }

        /// <summary>
        /// Sets the layer of the transform's children.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="layerName">Name of layer.</param>
        /// <param name="recursive">Also set ancestor layers?</param>
        public static void SetChildLayers(this Transform transform, string layerName, bool recursive = false)
        {
            var layer = LayerMask.NameToLayer(layerName);
            SetChildLayersHelper(transform, layer, recursive);
        }

        static void SetChildLayersHelper(Transform transform, int layer, bool recursive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = layer;

                if (recursive)
                {
                    SetChildLayersHelper(child, layer, recursive);
                }
            }
        }

        /// <summary>
        /// Returns the distance to another transform.
        /// </summary>
        /// <param name="transform">The transform of the object.</param>
        /// <param name="target">The transform to get the distance to.</param>
        /// <returns>Returns the distance to another transform.</returns>
        public static float GetDistance(this Transform transform, Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }

        /// <summary>
        /// Returns the nearest transfrom in a list for a given transform.
        /// </summary>
        /// <param name="transform">The transform to get the nearest of.</param>
        /// <param name="list">The list to compare transform with.</param>
        /// <returns>The nearest transfrom.</returns>
        public static Transform GetNearest(this Transform transform, IList<Transform> list)
        {
            Transform nearest = null;
            float minDist = Mathf.Infinity;

            foreach (Transform t in list)
            {
                float dist = Vector3.Distance(transform.position, t.position);

                if (dist < minDist)
                {
                    nearest = t;
                    minDist = dist;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Returns the nearest T in a list for a given transform.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform">The transform to get the nearest of.</param>
        /// <param name="list">The list to compare transform with.</param>
        /// <returns>The nearest transfrom.</returns>
        public static T GetNearest<T>(this Transform transform, IList<T> list) where T : MonoBehaviour
        {
            T nearest = null;
            float minDist = Mathf.Infinity;

            foreach (T t in list)
            {
                float dist = Vector3.Distance(transform.position, t.transform.position);

                if (dist < minDist && dist > 0.001f)
                {
                    nearest = t;
                    minDist = dist;
                }
            }

            return nearest;
        }

        public static T FindComponentByName<T>(this Transform transform, string name) where T : MonoBehaviour
        {
            T result = default(T);

            var t = transform.FindRecursive(name);

            if (t != null) { result = t.GetComponentInChildren<T>(); }

            return result;
        }

        public static T FindInParent<T>(this Transform transform) where T : MonoBehaviour
        {
            return transform.parent.GetComponent<T>();
        }
    }
}
