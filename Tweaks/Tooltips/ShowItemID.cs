﻿using Dalamud.Game.Chat.SeStringHandling.Payloads;
using FFXIVClientStructs.FFXIV.Component.GUI;
using SimpleTweaksPlugin.GameStructs;
using SimpleTweaksPlugin.GameStructs.Client.UI;
using SimpleTweaksPlugin.Helper;
using static SimpleTweaksPlugin.Tweaks.TooltipTweaks.ItemTooltip.TooltipField;
namespace SimpleTweaksPlugin.Tweaks.Tooltips {
    public class ShowItemID : TooltipTweaks.SubTweak {
        public override string Name => "Show ID";
        public override void OnItemTooltip(TooltipTweaks.ItemTooltip tooltip, InventoryItem itemInfo) {
            var seStr = tooltip[ItemUiCategory];
            if (seStr == null) return;
            var id = Plugin.PluginInterface.Framework.Gui.HoveredItem;
            if (id < 2000000) id %= 500000;
            seStr.Payloads.Add(new UIForegroundPayload(PluginInterface.Data, 3));
            seStr.Payloads.Add(new TextPayload($"   [{id}]"));
            seStr.Payloads.Add(new UIForegroundPayload(PluginInterface.Data, 0));
            tooltip[ItemUiCategory] = seStr;
        }

        public override unsafe void OnActionTooltip(AddonActionDetail* addon, TooltipTweaks.HoveredAction action) {
            if (addon->AtkUnitBase.ULDData.NodeList == null || addon->AtkUnitBase.ULDData.NodeListCount < 29) return;
            var categoryText = (AtkTextNode*) addon->AtkUnitBase.ULDData.NodeList[28];
            if (categoryText == null) return;
            var seStr = Plugin.Common.ReadSeString(categoryText->NodeText.StringPtr);
            if (seStr.Payloads.Count <= 1) {
                if (seStr.Payloads.Count >= 1) {
                    seStr.Payloads.Add(new TextPayload("   "));
                }
                seStr.Payloads.Add(new UIForegroundPayload(PluginInterface.Data, 3));
                seStr.Payloads.Add(new TextPayload($"[{action.Id}]"));
                seStr.Payloads.Add(new UIForegroundPayload(PluginInterface.Data, 0));
                UiHelper.SetText(categoryText, seStr);
            }
            
        }
    }
}