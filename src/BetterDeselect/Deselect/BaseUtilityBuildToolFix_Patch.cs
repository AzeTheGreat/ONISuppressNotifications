﻿using Harmony;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(BaseUtilityBuildTool), "OnDeactivateTool")]
    public class BaseUtilityBuildToolFix_Patch
    {
        static void Prefix()
        {
            SelectTool.Instance.Activate();
        }
    }
}
