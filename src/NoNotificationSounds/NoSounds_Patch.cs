﻿using HarmonyLib;
using PeterHan.PLib.Options;

namespace NoNotificationSounds
{
    [HarmonyPatch(typeof(NotificationScreen), "PlayDingSound")]
    public class NoSounds_Patch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}
