﻿using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(PlanScreen), "ConfigurePanelSize")]
    class DisableScrollbar_Patch
    {
        private static void Postfix(PlanScreen __instance, int ___buildGrid_maxRowsBeforeScroll)
        {
            int buttons = __instance.GroupsTransform.childCount;
            int rows = Mathf.CeilToInt(buttons / 3f);

            __instance.BuildingGroupContentsRect.GetComponent<ScrollRect>().vertical = rows >= ___buildGrid_maxRowsBeforeScroll + 1;

            if (rows < ___buildGrid_maxRowsBeforeScroll + 1)
                // Couldn't find any relevant numbers in the source so I guess I too get to use magic numbers...
                __instance.buildingGroupsRoot.sizeDelta += new Vector2(-13f, 0f);
        }
    }

    [HarmonyPatch(typeof(PlanScreen), MethodType.Constructor)]
    class FixHeight_Patch
    {
        private static void Postfix(ref float ___buildGrid_bg_borderHeight)
        {
            // I have no idea why * 1.5 looks right, logically it should be * 2...
            ___buildGrid_bg_borderHeight *= 1.5f;
        }
    }
}