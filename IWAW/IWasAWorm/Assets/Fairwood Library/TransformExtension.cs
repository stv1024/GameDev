using UnityEngine;

namespace Fairwood
{
    public static class TransformExtension
    {
        public static void ResetToDefault(this Transform tra)
        {
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one;
        }

        public static void ResetToDefault(this Transform tra, Transform parent)
        {
            tra.parent = parent;
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one;
        }

        /// <summary>
        /// 使用target的parent
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="target"></param>
        public static void ResetAs(this Transform tra, Transform target)
        {
            tra.parent = target.parent;
            tra.localPosition = target.localPosition;
            tra.localRotation = target.localRotation;
            tra.localScale = target.localScale;
        }

        /// <summary>
        /// 使用指定的parent
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        public static void ResetAs(this Transform tra, Transform target, Transform parent)
        {
            tra.parent = parent;
            tra.localPosition = target.localPosition;
            tra.localRotation = target.localRotation;
            tra.localScale = target.localScale;
        }

        public static void SetLayer(Transform tra, int layer)
        {
            tra.gameObject.layer = layer;
            for (int i = 0, imax = tra.GetChildCount(); i < imax; i++)
            {
                SetLayer(tra.GetChild(i), layer);
            }
        }
    }
}