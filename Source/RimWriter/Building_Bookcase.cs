﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWriter
{
    public class Building_Bookcase : Building_InternalStorage
    {
        public override string GetInspectString()
        {
            var s = new StringBuilder();
            var baseStr = base.GetInspectString();
            if (baseStr != "")
            {
                s.AppendLine(baseStr);
            }

            if (innerContainer.Count > 0)
            {
                s.AppendLine("RimWriter_ContainsXBooks".Translate(innerContainer.Count));
            }

            s.AppendLine("RimWriter_XSlotsForBooks".Translate(CompStorageGraphic.Props.countFullCapacity));
            return s.ToString().TrimEndNewlines();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }

            if (innerContainer.Count > 0)
            {
                yield return new Command_Action()
                {
                  defaultLabel = "RimWriter_RetrieveBook".Translate(),
                  icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true),
                  defaultDesc = "RimWriter_RetrieveBookDesc".Translate(),
                  action = delegate
                  {
                      ProcessInput();
                  }
                    
    
                };
            }
        }

        public void ProcessInput()
        {

            var list = new List<FloatMenuOption>();
            Map map = Map;
            if (innerContainer.Count != 0)
            {
                foreach (ThingBook current in innerContainer)
                {
                    var text = current.Label;
                    if (current.TryGetComp<CompArt>() is CompArt compArt)
                    {
                        text = TranslatorFormattedStringExtensions.Translate("RimWriter_BookTitle", compArt.Title, compArt.AuthorName);
                    }

                    List<FloatMenuOption> arg_121_0 = list;
                    bool extraPartOnGUI(Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, current);
                    arg_121_0.Add(new FloatMenuOption(text, delegate
                    {
                        TryDrop(current);
                    }, MenuOptionPriority.Default, null, null, 29f, extraPartOnGUI, null));
                }
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }
    }
}
