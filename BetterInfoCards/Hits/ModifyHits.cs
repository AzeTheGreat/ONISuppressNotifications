﻿using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyHits
    {
        public static ModifyHits Instance { get; set; }

        private List<int> indexRedirect = new List<int>();
        private int localIndex = -1;
        private bool first = true;
        private List<MonoBehaviour> intersections = new List<MonoBehaviour>();
        private List<MonoBehaviour> priorSelectedComps = new List<MonoBehaviour>();

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static bool Prepare() => true;

            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));
            }

            static void Postfix(bool cycleSelection, ref KSelectable __result, ref int ___hitCycleCount, List<InterfaceTool.Intersection> ___intersections)
            {
                if (__result == null)
                    return;

                if (Instance.first)
                {
                    Instance.first = false;
                    Instance.intersections = ___intersections.Select(x => x.component).ToList();

                    Instance.SetNewIndex();
                }

                if (cycleSelection)
                {
                    Instance.localIndex++;
                    if (Instance.localIndex > Instance.indexRedirect.Count - 1)
                        Instance.localIndex = 0;
                }

                int targetIndex = 0;
                if (Instance.localIndex != -1)
                    targetIndex = Instance.indexRedirect[Instance.localIndex];

                __result = ___intersections[targetIndex].component as KSelectable;
            }
        }

        public void Reset(List<int> redirects)
        {
            if (indexRedirect.Count > 0 && localIndex != -1)
            {
                int index = indexRedirect[localIndex];
                priorSelectedComps = intersections.GetRange(index, GetRedirectCount(index));
            }

            indexRedirect = redirects;
            localIndex = -1;
            first = true;
        }

        private void SetNewIndex()
        {
            int index = -1;
            foreach (MonoBehaviour comp in priorSelectedComps)
            {
                index = intersections.FindIndex(x => ReferenceEquals(x, comp));
                if (index != -1)
                    break;
            }

            if (index != -1)
            {
                for (int i = 0; i < indexRedirect.Count; i++)
                {
                    int start = indexRedirect[i];
                    int end = start + GetRedirectCount(start);

                    if (index >= start && index < end)
                        localIndex = i;
                }
            }
            else
                localIndex = -1;

            priorSelectedComps.Clear();
        }

        private int GetRedirectCount(int index)
        {
            int count = 0;
            if (index + 1 >= indexRedirect.Count)
                count = intersections.Count - 2 - index;
            else
                count = indexRedirect[index + 1] - index;

            return count;
        }
    }

    
}
