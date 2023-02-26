using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Shared
{
    [InitializeOnLoad]
    public static class SharedGraphics
    {
        //------------------------------
        // Public Static
        //------------------------------

        public static Texture2D RibbonHot { get { if (ribbonHot == null) { ribbonHot = GraphicsData.Get("preset_browser_ribbons_hot"); } return ribbonHot; } }
        public static Texture2D RibbonNew { get { if (ribbonNew == null) { ribbonNew = GraphicsData.Get("preset_browser_ribbons_new"); } return ribbonNew; } }

        public static Texture2D LayerBackground { get { if (layerBackground == null) { layerBackground = GraphicsData.Get("layerbar"); } return layerBackground; } }
        public static Texture2D LayerBackgroundTransparent { get { if (layerBackgroundTransparent == null) { layerBackgroundTransparent = GraphicsData.Get("layerbar_transparent"); } return layerBackgroundTransparent; } }
        public static Texture2D LayerBackgroundLightTransparent { get { if (layerBackgroundLightTransparent == null) { layerBackgroundLightTransparent = GraphicsData.Get("layerbar_light_transparent"); } return layerBackgroundLightTransparent; } }
        public static Texture2D LayerBlueBackground { get { if (layerBlueBackground == null) { layerBlueBackground = GraphicsData.Get("layerbar_blue"); } return layerBlueBackground; } }

        public static Texture2D BoxBackground { get { if (boxBackground == null) { boxBackground = GraphicsData.Get("box"); } return boxBackground; } }
        public static Texture2D BoxActiveBackground { get { if (boxActiveBackground == null) { boxActiveBackground = GraphicsData.Get("box_active"); } return boxActiveBackground; } }

        public static Texture2D ListItemBackground { get { if (listItemBackground == null) { listItemBackground = GraphicsData.Get("list_item"); } return listItemBackground; } }
        public static Texture2D TabItemBackground { get { if (tabItemBackground == null) { tabItemBackground = GraphicsData.Get("tabitem"); } return tabItemBackground; } }
        public static Texture2D TabItemHoverBackground { get { if (tabItemHoverBackground == null) { tabItemHoverBackground = GraphicsData.Get("tabitem_hover"); } return tabItemHoverBackground; } }
        public static Texture2D ToolbarItemHoverBackground { get { if (toolbarItemHoverBackground == null) { toolbarItemHoverBackground = GraphicsData.Get("toolbar"); } return toolbarItemHoverBackground; } }
        public static Texture2D ToolbarItemActiveBackground { get { if (toolbarItemActiveBackground == null) { toolbarItemActiveBackground = GraphicsData.Get("tool_active_background"); } return toolbarItemActiveBackground; } }

        public static Texture2D IconEditSmall { get { if (iconEditSmall == null) { iconEditSmall = GraphicsData.Get("ui_edit_small"); } return iconEditSmall; } }

        public static Texture2D IconCancle { get { if (iconCancle == null) { iconCancle = GraphicsData.Get("ui_cancle"); } return iconCancle; } }
        public static Texture2D IconCancleSmall { get { if (iconCancleSmall == null) { iconCancleSmall = GraphicsData.Get("ui_cancle_small"); } return iconCancleSmall; } }

        public static Texture2D IconAdd { get { if (iconAdd == null) { iconAdd = GraphicsData.Get("ui_add"); } return iconAdd; } }
        public static Texture2D IconAddSmall { get { if (iconAddSmall == null) { iconAddSmall = GraphicsData.Get("ui_add_small"); } return iconAddSmall; } }
        public static Texture2D IconAddBoxedSmall { get { if (iconAddBoxedSmall == null) { iconAddBoxedSmall = GraphicsData.Get("ui_add_boxed_small"); } return iconAddBoxedSmall; } }
        public static Texture2D IconAddGreen { get { if (iconAddGreen == null) { iconAddGreen = GraphicsData.Get("ui_add_green"); } return iconAddGreen; } }
        public static Texture2D IconAddGreenSmall { get { if (iconAddGreenSmall == null) { iconAddGreenSmall = GraphicsData.Get("ui_add_green_small"); } return iconAddGreenSmall; } }

        public static Texture2D IconRadioButtonOn { get { if (iconRadioButtonOn == null) { iconRadioButtonOn = GraphicsData.Get("ui_radion_button_on"); } return iconRadioButtonOn; } }
        public static Texture2D IconRadioButtonOff { get { if (iconRadioButtonOff == null) { iconRadioButtonOff = GraphicsData.Get("ui_radion_button_off"); } return iconRadioButtonOff; } }

        public static Texture2D IconSwitchButtonOn { get { if (iconSwitchButtonOn == null) { iconSwitchButtonOn = GraphicsData.Get("ui_switch_on"); } return iconSwitchButtonOn; } }
        public static Texture2D IconSwitchButtonOff { get { if (iconSwitchButtonOff == null) { iconSwitchButtonOff = GraphicsData.Get("ui_switch_off"); } return iconSwitchButtonOff; } }

        public static Texture2D IconExpandUp { get { if (iconExpandUp == null) { iconExpandUp = GraphicsData.Get("ui_expand_up"); } return iconExpandUp; } }
        public static Texture2D IconExpandDown { get { if (iconExpandDown == null) { iconExpandDown = GraphicsData.Get("ui_expand_down"); } return iconExpandDown; } }

        public static Texture2D IconDelete { get { if (iconDelete == null) { iconDelete = GraphicsData.Get("ui_delete"); } return iconDelete; } }
        public static Texture2D IconDeleteSmall { get { if (iconDeleteSmall == null) { iconDeleteSmall = GraphicsData.Get("ui_delete_small"); } return iconDeleteSmall; } }
        public static Texture2D IconTrash { get { if (iconTrash == null) { iconTrash = GraphicsData.Get("ui_trash"); } return iconTrash; } }
        public static Texture2D IconTrashSmall { get { if (iconTrashSmall == null) { iconTrashSmall = GraphicsData.Get("ui_trash_small"); } return iconTrashSmall; } }
        public static Texture2D IconRemove { get { if (iconRemove == null) { iconRemove = GraphicsData.Get("ui_remove"); } return iconRemove; } }
        public static Texture2D IconRemoveSmall { get { if (iconRemoveSmall == null) { iconRemoveSmall = GraphicsData.Get("ui_remove_small"); } return iconRemoveSmall; } }
        public static Texture2D IconStop { get { if (iconStop == null) { iconStop = GraphicsData.Get("ui_stop"); } return iconStop; } }
        public static Texture2D IconStopSmall { get { if (iconStopSmall == null) { iconStopSmall = GraphicsData.Get("ui_stop_small"); } return iconStopSmall; } }

        public static Texture2D IconVisible { get { if (iconVisible == null) { iconVisible = GraphicsData.Get("ui_visible"); } return iconVisible; } }
        public static Texture2D IconVisibleSmall { get { if (iconVisibleSmall == null) { iconVisibleSmall = GraphicsData.Get("ui_visible_small"); } return iconVisibleSmall; } }
        public static Texture2D IconInvisible { get { if (iconInvisible == null) { iconInvisible = GraphicsData.Get("ui_invisible"); } return iconInvisible; } }
        public static Texture2D IconInvisibleSmall { get { if (iconInvisibleSmall == null) { iconInvisibleSmall = GraphicsData.Get("ui_invisible_small"); } return iconInvisibleSmall; } }
        public static Texture2D IconVisibleSmallBack { get { if (iconVisibleSmallBack == null) { iconVisibleSmallBack = GraphicsData.Get("ui_visible_boxed_small"); } return iconVisibleSmallBack; } }
        public static Texture2D IconInvisibleSmallBack { get { if (iconInvisibleSmallBack == null) { iconInvisibleSmallBack = GraphicsData.Get("ui_invisible_boxed_small"); } return iconInvisibleSmallBack; } }

        public static Texture2D IconMuteEnabled { get { if (iconMuteEnabled == null) { iconMuteEnabled = GraphicsData.Get("ui_mute_enabled"); } return iconMuteEnabled; } }
        public static Texture2D IconMuteEnabledSmall { get { if (iconMuteEnabledSmall == null) { iconMuteEnabledSmall = GraphicsData.Get("ui_mute_enabled_small"); } return iconMuteEnabledSmall; } }
        public static Texture2D IconMuteDisabled { get { if (iconMuteDisabled == null) { iconMuteDisabled = GraphicsData.Get("ui_mute_disabled"); } return iconMuteDisabled; } }
        public static Texture2D IconMuteDisabledSmall { get { if (iconMuteDisabledSmall == null) { iconMuteDisabledSmall = GraphicsData.Get("ui_mute_disabled_small"); } return iconMuteDisabledSmall; } }

        public static Texture2D IconSoloActive { get { if (iconSoloActive == null) { iconSoloActive = GraphicsData.Get("ui_solo_active"); } return iconSoloActive; } }
        public static Texture2D IconSoloInactive { get { if (iconSoloInactive == null) { iconSoloInactive = GraphicsData.Get("ui_solo_inactive"); } return iconSoloInactive; } }
        public static Texture2D IconSoloActiveSmall { get { if (iconSoloActiveSmall == null) { iconSoloActiveSmall = GraphicsData.Get("ui_solo_active_small"); } return iconSoloActiveSmall; } }
        public static Texture2D IconSoloInactiveSmall { get { if (iconSoloInactiveSmall == null) { iconSoloInactiveSmall = GraphicsData.Get("ui_solo_inactive_small"); } return iconSoloInactiveSmall; } }

        public static Texture2D IconInput_32 { get { if (iconInput_32 == null) { iconInput_32 = GraphicsData.Get("ui_input"); } return iconInput_32; } }
        public static Texture2D IconInput_16 { get { if (iconInput_16 == null) { iconInput_16 = GraphicsData.Get("ui_input_small"); } return iconInput_16; } }
        public static Texture2D IconInput_10 { get { if (iconInput_10 == null) { iconInput_10 = GraphicsData.Get("ui_input_10"); } return iconInput_10; } }

        public static Texture2D IconCombine_18 { get { if (iconCombine_18 == null) { iconCombine_18 = GraphicsData.Get("ui_combine_small"); } return iconCombine_18; } }
        public static Texture2D IconSeparate_18 { get { if (iconSeparate_18 == null) { iconSeparate_18 = GraphicsData.Get("ui_separate_small"); } return iconSeparate_18; } }

        public static Texture2D IconDialogInfo { get { if (iconDialogInfo == null) { iconDialogInfo = GraphicsData.Get("dialog_icons_info"); } return iconDialogInfo; } }
        public static Texture2D IconDialogSuccess { get { if (iconDialogSuccess == null) { iconDialogSuccess = GraphicsData.Get("dialog_icons_success"); } return iconDialogSuccess; } }
        public static Texture2D IconDialogWarning { get { if (iconDialogWarning == null) { iconDialogWarning = GraphicsData.Get("dialog_icons_warning"); } return iconDialogWarning; } }
        public static Texture2D IconDialogEvent { get { if (iconDialogEvent == null) { iconDialogEvent = GraphicsData.Get("dialog_icons_event"); } return iconDialogEvent; } }
        public static Texture2D IconDialogError { get { if (iconDialogError == null) { iconDialogError = GraphicsData.Get("dialog_icons_error"); } return iconDialogError; } }

        public static Texture2D IconLocked_18 { get { if (iconLocked_18 == null) { iconLocked_18 = GraphicsData.Get("ui_locked"); } return iconLocked_18; } }
        public static Texture2D IconUnlocked_18 { get { if (iconUnlocked_18 == null) { iconUnlocked_18 = GraphicsData.Get("ui_unlocked"); } return iconUnlocked_18; } }

        public static Texture2D IconAspect_18 { get { if (iconAspect_18 == null) { iconAspect_18 = GraphicsData.Get("ui_aspect_small"); } return iconAspect_18; } }
        public static Texture2D IconAspectActive_18 { get { if (iconAspectActive_18 == null) { iconAspectActive_18 = GraphicsData.Get("ui_aspect_active_small"); } return iconAspectActive_18; } }

        public static Texture2D IconWait_32 { get { if (iconWait_32 == null) { iconWait_32 = GraphicsData.Get("wait_icon"); } return iconWait_32; } }
        public static Texture2D IconTick_32 { get { if (iconTick_32 == null) { iconTick_32 = GraphicsData.Get("ui_tick_32"); } return iconTick_32; } }

        public static Texture2D IconChannel_Color_18 { get { if (toolIconHand_32 == null) { toolIconHand_32 = GraphicsData.Get("ui_channel_color"); } return toolIconHand_32; } }
        public static Texture2D IconChannel_ColorAlpha_18 { get { if (iconChannel_ColorAlpha_18 == null) { iconChannel_ColorAlpha_18 = GraphicsData.Get("ui_channel_color_alpha"); } return iconChannel_ColorAlpha_18; } }

        public static Texture2D ToolIconArrow_32 { get { if (toolIconHand_32 == null) { toolIconHand_32 = GraphicsData.Get("ui_tool_arrow"); } return toolIconHand_32; } }
        public static Texture2D ToolIconPointer_32 { get { if (toolIconPointer_32 == null) { toolIconPointer_32 = GraphicsData.Get("ui_tool_pointer"); } return toolIconPointer_32; } }
        public static Texture2D ToolIconGrab_32 { get { if (toolIconGrab_32 == null) { toolIconGrab_32 = GraphicsData.Get("ui_tool_hand_grab"); } return toolIconGrab_32; } }
        public static Texture2D ToolIconHand_32 { get { if (toolIconHand_32 == null) { toolIconHand_32 = GraphicsData.Get("ui_tool_hand"); } return toolIconHand_32; } }
        public static Texture2D ToolIconSelection_32 { get { if (toolIconSelection_32 == null) { toolIconSelection_32 = GraphicsData.Get("ui_tool_selection"); } return toolIconSelection_32; } }
        public static Texture2D ToolIconDenied_32 { get { if (toolIconDenied_32 == null) { toolIconDenied_32 = GraphicsData.Get("ui_tool_denied"); } return toolIconDenied_32; } }

        public static Texture2D AlphaBackground { get { if (alphaBackground == null) { alphaBackground = GraphicsData.Get("alphabg"); } return alphaBackground; } }

        //------------------------------
        // Private Static
        //------------------------------

        private static string BasePath = "Sycoforge/Shared/";
        private static string BasePathUI_Background = BasePath + "UI/Background/";
        private static string BasePathUI_Icons = BasePath + "UI/Icons/";

        private static Texture2D ribbonHot;
        private static Texture2D ribbonNew;

        private static Texture2D layerBackground;
        private static Texture2D layerBackgroundTransparent;
        private static Texture2D layerBackgroundLightTransparent;
        private static Texture2D layerBlueBackground;

        private static Texture2D boxBackground;
        private static Texture2D boxActiveBackground;

        private static Texture2D listItemBackground;
        private static Texture2D tabItemBackground;
        private static Texture2D tabItemHoverBackground;
        private static Texture2D toolbarItemHoverBackground;
        private static Texture2D toolbarItemActiveBackground;

        private static Texture2D iconEditSmall;

        private static Texture2D iconCancle;
        private static Texture2D iconCancleSmall;

        private static Texture2D iconAdd;
        private static Texture2D iconAddSmall;
        private static Texture2D iconAddBoxedSmall;

        private static Texture2D iconAddGreen;
        private static Texture2D iconAddGreenSmall;

        private static Texture2D iconRadioButtonOn;
        private static Texture2D iconRadioButtonOff;

        private static Texture2D iconSwitchButtonOn;
        private static Texture2D iconSwitchButtonOff;

        private static Texture2D iconExpandUp;
        private static Texture2D iconExpandDown;

        private static Texture2D iconDelete;
        private static Texture2D iconDeleteSmall;
        private static Texture2D iconTrash;
        private static Texture2D iconTrashSmall;
        private static Texture2D iconRemove;
        private static Texture2D iconRemoveSmall;
        private static Texture2D iconStop;
        private static Texture2D iconStopSmall;

        private static Texture2D iconVisible;
        private static Texture2D iconVisibleSmall;
        private static Texture2D iconInvisible;
        private static Texture2D iconInvisibleSmall;
        private static Texture2D iconVisibleSmallBack;
        private static Texture2D iconInvisibleSmallBack;

        private static Texture2D iconMuteEnabled;
        private static Texture2D iconMuteEnabledSmall;
        private static Texture2D iconMuteDisabled;
        private static Texture2D iconMuteDisabledSmall;

        private static Texture2D iconSoloActive;
        private static Texture2D iconSoloInactive;

        private static Texture2D iconSoloActiveSmall;
        private static Texture2D iconSoloInactiveSmall;

        private static Texture2D iconInput_32;
        private static Texture2D iconInput_16;
        private static Texture2D iconInput_10;

        private static Texture2D iconCombine_18;
        private static Texture2D iconSeparate_18;

        private static Texture2D iconDialogInfo;
        private static Texture2D iconDialogSuccess;
        private static Texture2D iconDialogWarning;
        private static Texture2D iconDialogEvent;
        private static Texture2D iconDialogError;

        private static Texture2D iconLocked_18;
        private static Texture2D iconUnlocked_18;

        private static Texture2D iconAspect_18;
        private static Texture2D iconAspectActive_18;

        private static Texture2D iconWait_32;
        private static Texture2D iconTick_32;

        private static Texture2D iconChannel_Color_18;
        private static Texture2D iconChannel_ColorAlpha_18;

        private static Texture2D toolIconArrow_32;
        private static Texture2D toolIconPointer_32;
        private static Texture2D toolIconGrab_32;
        private static Texture2D toolIconHand_32;
        private static Texture2D toolIconSelection_32;
        private static Texture2D toolIconDenied_32;

        private static Texture2D alphaBackground;

        static SharedGraphics()
        {
            Initialize();
        }

        public static void Initialize()
        {
            ribbonHot = GraphicsData.Get("preset_browser_ribbons_hot");
            ribbonNew = GraphicsData.Get("preset_browser_ribbons_new");

            layerBackground = GraphicsData.Get("layerbar");
            layerBackgroundTransparent = GraphicsData.Get("layerbar_transparent");
            layerBackgroundLightTransparent = GraphicsData.Get("layerbar_light_transparent");
            layerBlueBackground = GraphicsData.Get("layerbar_blue");

            boxBackground = GraphicsData.Get("box");
            boxActiveBackground = GraphicsData.Get("box_active");

            listItemBackground = GraphicsData.Get("list_item");
            tabItemBackground = GraphicsData.Get("tabitem");
            tabItemHoverBackground = GraphicsData.Get("tabitem_hover");
            toolbarItemHoverBackground = GraphicsData.Get("toolbar");
            toolbarItemActiveBackground = GraphicsData.Get("tool_active_background");

            iconEditSmall = GraphicsData.Get("ui_edit_small");

            iconTick_32 = GraphicsData.Get("ui_tick_32");

            iconWait_32 = GraphicsData.Get("wait_icon");

            iconCancle = GraphicsData.Get("ui_cancle");
            iconCancleSmall = GraphicsData.Get("ui_cancle_small");

            iconAdd = GraphicsData.Get("ui_add");
            iconAddSmall = GraphicsData.Get("ui_add_small");
            iconAddBoxedSmall = GraphicsData.Get("ui_add_boxed_small");
            iconAddGreen = GraphicsData.Get("ui_add_green");
            iconAddGreenSmall = GraphicsData.Get("ui_add_green_small");

            iconRadioButtonOn = GraphicsData.Get("ui_radion_button_on");
            iconRadioButtonOff = GraphicsData.Get("ui_radion_button_off");

            iconSwitchButtonOn = GraphicsData.Get("ui_switch_on");
            iconSwitchButtonOff = GraphicsData.Get("ui_switch_off");

            iconDelete = GraphicsData.Get("ui_delete");
            iconDeleteSmall = GraphicsData.Get("ui_delete_small");

            iconTrash = GraphicsData.Get("ui_trash");
            iconTrashSmall = GraphicsData.Get("ui_trash_small");

            iconRemove = GraphicsData.Get("ui_remove");
            iconRemoveSmall = GraphicsData.Get("ui_remove_small");

            iconStop = GraphicsData.Get("ui_stop");
            iconStopSmall = GraphicsData.Get("ui_stop_small");

            iconVisible = GraphicsData.Get("ui_visible");
            iconVisibleSmall = GraphicsData.Get("ui_visible_small");
            iconInvisible = GraphicsData.Get("ui_invisible");
            iconInvisibleSmall = GraphicsData.Get("ui_invisible_small");
            iconVisibleSmallBack = GraphicsData.Get("ui_visible_boxed_small");
            iconInvisibleSmallBack = GraphicsData.Get("ui_invisible_boxed_small");

            iconCombine_18 = GraphicsData.Get("ui_combine_small");

            iconSeparate_18 = GraphicsData.Get("ui_separate_small");

            iconMuteEnabled = GraphicsData.Get("ui_mute_enabled");
            iconMuteEnabledSmall = GraphicsData.Get("ui_mute_enabled_small");
            iconMuteDisabled = GraphicsData.Get("ui_mute_disabled");
            iconMuteDisabledSmall = GraphicsData.Get("ui_mute_disabled_small");

            iconSoloActive = GraphicsData.Get("ui_solo_active");
            iconSoloInactive = GraphicsData.Get("ui_solo_inactive");
            iconSoloActiveSmall = GraphicsData.Get("ui_solo_active_small");
            iconSoloInactiveSmall = GraphicsData.Get("ui_solo_inactive_small");

            iconDialogInfo = GraphicsData.Get("dialog_icons_info");
            iconDialogSuccess = GraphicsData.Get("dialog_icons_success");
            iconDialogEvent = GraphicsData.Get("dialog_icons_event");
            iconDialogWarning = GraphicsData.Get("dialog_icons_warning");
            iconDialogError = GraphicsData.Get("dialog_icons_error");

            iconExpandUp = GraphicsData.Get("ui_expand_up");
            iconExpandDown = GraphicsData.Get("ui_expand_down");

            iconInput_32 = GraphicsData.Get("ui_input");
            iconInput_16 = GraphicsData.Get("ui_input_small");
            iconInput_10 = GraphicsData.Get("ui_input_10");

            iconLocked_18 = GraphicsData.Get("ui_locked");
            iconUnlocked_18 = GraphicsData.Get("ui_unlocked");

            iconAspect_18 = GraphicsData.Get("ui_aspect_small");
            iconAspectActive_18 = GraphicsData.Get("ui_aspect_active_small");

            iconChannel_Color_18 = GraphicsData.Get("ui_channel_color");
            iconChannel_ColorAlpha_18 = GraphicsData.Get("ui_channel_color_alpha");

            toolIconArrow_32 = GraphicsData.Get("ui_tool_arrow");
            toolIconPointer_32 = GraphicsData.Get("ui_tool_pointer");
            toolIconGrab_32 = GraphicsData.Get("ui_tool_hand_grab");
            toolIconHand_32 = GraphicsData.Get("ui_tool_hand");
            toolIconSelection_32 = GraphicsData.Get("ui_tool_selection");
            toolIconDenied_32 = GraphicsData.Get("ui_tool_denied");

            alphaBackground = GraphicsData.Get("alphabg");

            SharedColors.Initialize();
            SharedStyles.Initialize();
        }
    }
}
