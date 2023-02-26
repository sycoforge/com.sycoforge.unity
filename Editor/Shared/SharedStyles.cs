using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Shared
{
    public static class SharedStyles
    {        
        //------------------------------
        // Public Static
        //------------------------------
        public static GUIStyle LayerStyle;
        public static GUIStyle LayerStyleTransparent;
        public static GUIStyle LayerStyleDrag;
        public static GUIStyle LayerStyleBlue;

        public static GUIStyle BoxStyle;
        public static GUIStyle BoxMarginStyle;

        public static GUIStyle NotificationBoxStyle;


        public static GUIStyle ListItemStyle;
        public static GUIStyle TabItemStyle;
        public static GUIStyle VisibilityToggleStyle;
        //public static GUIStyle VisibilityToggleStyleSmall;
        public static GUIStyle VisibilityToggleToolbarStyle;
        public static GUIStyle ToolbarItemStyle;

        public static GUIStyle SideToolbarStyle;
        public static GUIStyle SideToolbarItemStyle;

        public static GUIStyle SoloToggleStyle;
        public static GUIStyle SoloToggleSmallStyle;

        public static GUIStyle LockToggleStyle;
        public static GUIStyle AspectToggleStyle;
        public static GUIStyle ColorChannelToggleStyle;


        public static GUIStyle RadioToggleStyle;
        public static GUIStyle SwitchToggleStyle;

        public static GUIStyle ExpanderStyle;


        public static GUIStyle LabelSmallWhite;
        public static GUIStyle LabelSmallWhiteRight;

        public static GUIStyle DefaultTitleLabel;
        public static GUIStyle DefaultTitleLabelBold;

        public static GUIStyle DefaultRatingLabel;

        public static GUIStyle DefaultLabel;
        public static GUIStyle DefaultLabelWhite;
        public static GUIStyle DefaultLabelBold;
        public static GUIStyle DefaultLabelWrap;

        public static GUIStyle WaitBoxStyle;

        public static GUIStyle TransparentButton;
        public static GUIStyle RatingButton;
        public static GUIStyle PaginationButton;
        public static GUIStyle PaginationActiveButton;

        public static GUIStyle LinkLabel;
        public static GUIStyle LinkLabelLight;


        public static GUIStyle DefaultCheckbox;


        private const int SmallFontSize = 8; 
        //------------------------------
        // Private Static
        //------------------------------


        internal static void Initialize()
        {
            #region --- Checkbox ---
            DefaultCheckbox = new GUIStyle();
            DefaultCheckbox.normal.textColor = SharedColors.DefaultTextColor;
            DefaultCheckbox.onNormal.textColor = SharedColors.DefaultTextColor;
            DefaultCheckbox.alignment = TextAnchor.MiddleLeft;
            DefaultCheckbox.padding = new RectOffset(5, 0, 0, 0);
            DefaultCheckbox.stretchWidth = true;

            #endregion

            #region --- Styles ---

            LayerStyle = new GUIStyle();
            LayerStyle.border = new RectOffset(2, 2, 2, 2);
            LayerStyle.padding = new RectOffset(2, 3, 4, 2);
            LayerStyle.fontSize = 10;
            LayerStyle.normal.background = SharedGraphics.LayerBackground;
            LayerStyle.normal.textColor = Color.white;
            LayerStyle.hover.background = SharedGraphics.LayerBackgroundTransparent;

            LayerStyleTransparent = new GUIStyle();
            LayerStyleTransparent.border = new RectOffset(2, 2, 2, 2);
            LayerStyleTransparent.padding = new RectOffset(2, 3, 4, 2);
            LayerStyleTransparent.fontSize = 10;
            LayerStyleTransparent.normal.background = SharedGraphics.LayerBackgroundTransparent;
            LayerStyleTransparent.normal.textColor = Color.white;

            LayerStyleDrag = new GUIStyle();
            LayerStyleDrag.border = new RectOffset(2, 2, 2, 2);
            LayerStyleDrag.padding = new RectOffset(20, 3, 4, 2);
            LayerStyleDrag.fontSize = 10;
            LayerStyleDrag.normal.background = SharedGraphics.LayerBackgroundLightTransparent;
            LayerStyleDrag.normal.textColor = Color.white;
            

            LayerStyleBlue = new GUIStyle();
            LayerStyleBlue.border = new RectOffset(2, 2, 2, 2);
            LayerStyleBlue.padding = new RectOffset(2, 3, 4, 2);
            LayerStyleBlue.fontSize = 10;
            LayerStyleBlue.normal.background = SharedGraphics.LayerBlueBackground;
            LayerStyleBlue.normal.textColor = Color.white;

            BoxStyle = new GUIStyle();
            BoxStyle.border = new RectOffset(3, 3, 3, 3);
            BoxStyle.padding = new RectOffset(2, 3, 4, 2);
            BoxStyle.fontSize = 10;
            BoxStyle.normal.background = SharedGraphics.BoxBackground;
            BoxStyle.onNormal.background = SharedGraphics.BoxActiveBackground;
            BoxStyle.normal.textColor = Color.white;

            BoxMarginStyle = new GUIStyle(BoxStyle);
            BoxMarginStyle.margin = new RectOffset(20, 20, 20, 20);
            BoxMarginStyle.padding = new RectOffset(10, 10, 10, 10);

            NotificationBoxStyle = new GUIStyle();
            NotificationBoxStyle.padding = new RectOffset(15, 15, 15, 15);

            ListItemStyle = new GUIStyle();
            ListItemStyle.border = new RectOffset(2, 2, 2, 2);
            ListItemStyle.padding = new RectOffset(10, 5, 4, 2);
            ListItemStyle.fontSize = 10;
            ListItemStyle.alignment = TextAnchor.MiddleLeft;
            ListItemStyle.fixedHeight = 24;
            ListItemStyle.normal.background = SharedGraphics.ListItemBackground;
            ListItemStyle.normal.textColor = Color.white;
            ListItemStyle.hover.textColor = SharedColors.SkyBlue;
            ListItemStyle.onHover.textColor = SharedColors.SkyBlue;
            ListItemStyle.focused.textColor = SharedColors.SkyBlue;

            #region --- tabitem ---
            TabItemStyle = new GUIStyle();
            TabItemStyle.border = new RectOffset(6, 25, 10, 2);
            TabItemStyle.padding = new RectOffset(10, 2, 4, 2);
            TabItemStyle.fontSize = 10;
            TabItemStyle.alignment = TextAnchor.MiddleLeft;
            TabItemStyle.fixedHeight = 34;
            TabItemStyle.normal.background = SharedGraphics.TabItemBackground;
            TabItemStyle.normal.textColor = Color.white;


            TabItemStyle.onNormal.background = SharedGraphics.TabItemHoverBackground;
            TabItemStyle.onNormal.textColor = SharedColors.SkyBlue;

            //
            TabItemStyle.hover.background = SharedGraphics.TabItemBackground;
            TabItemStyle.hover.textColor = SharedColors.SkyBlue;
            #endregion

            #region --- toolbar ---
            ToolbarItemStyle = new GUIStyle();
            ToolbarItemStyle.border = new RectOffset(1, 1, 1, 1);
            ToolbarItemStyle.padding = new RectOffset(2, 2, 2, 2);
            ToolbarItemStyle.fontSize = 10;
            ToolbarItemStyle.alignment = TextAnchor.MiddleLeft;
            ToolbarItemStyle.fixedHeight = 18;

            ToolbarItemStyle.normal.background = SharedGraphics.ToolbarItemHoverBackground;
            ToolbarItemStyle.normal.textColor = Color.white;

            ToolbarItemStyle.onNormal.background = SharedGraphics.ToolbarItemHoverBackground;
            ToolbarItemStyle.onNormal.textColor = SharedColors.SkyBlue;
            #endregion
            
            #region --- visibility toggle toolbar ---
            VisibilityToggleToolbarStyle = new GUIStyle();
            VisibilityToggleToolbarStyle.border = new RectOffset();
            VisibilityToggleToolbarStyle.padding = new RectOffset();
            VisibilityToggleToolbarStyle.alignment = TextAnchor.MiddleCenter;
            VisibilityToggleToolbarStyle.fixedHeight = 18;
            VisibilityToggleToolbarStyle.fixedWidth = 18;

            VisibilityToggleToolbarStyle.normal.background = SharedGraphics.IconInvisibleSmallBack;
            VisibilityToggleToolbarStyle.onNormal.background = SharedGraphics.IconVisibleSmallBack;

            #endregion

            #region --- visibility toggle ---
            VisibilityToggleStyle = new GUIStyle();
            VisibilityToggleStyle.border = new RectOffset();
            VisibilityToggleStyle.padding = new RectOffset();
            VisibilityToggleStyle.alignment = TextAnchor.MiddleLeft;
            VisibilityToggleStyle.fixedHeight = 16;
            VisibilityToggleStyle.fixedWidth = 16;
            VisibilityToggleStyle.stretchWidth = false;
            VisibilityToggleStyle.contentOffset = new Vector2(16, VisibilityToggleStyle.contentOffset.y);

            VisibilityToggleStyle.normal.background = SharedGraphics.IconInvisibleSmall;
            VisibilityToggleStyle.onNormal.background = SharedGraphics.IconVisibleSmall;

            #endregion

            //#region --- visibility toggle small ---
            //VisibilityToggleStyleSmall = new GUIStyle();
            //VisibilityToggleStyleSmall.border = new RectOffset();
            //VisibilityToggleStyleSmall.padding = new RectOffset();
            //VisibilityToggleStyleSmall.alignment = TextAnchor.MiddleLeft;
            //VisibilityToggleStyleSmall.fixedHeight = 16;
            //VisibilityToggleStyleSmall.fixedWidth = 16;
            //VisibilityToggleStyleSmall.stretchWidth = false;
            //VisibilityToggleStyleSmall.contentOffset = new Vector2(16, VisibilityToggleStyle.contentOffset.y);

            //VisibilityToggleStyleSmall.normal.background = SharedGraphics.IconInvisibleSmall;
            //VisibilityToggleStyleSmall.onNormal.background = SharedGraphics.IconVisibleSmall;

            //#endregion

            #region --- solo toggle ---
            SoloToggleStyle = new GUIStyle();
            SoloToggleStyle.border = new RectOffset();
            SoloToggleStyle.padding = new RectOffset();
            SoloToggleStyle.alignment = TextAnchor.MiddleLeft;
            SoloToggleStyle.fixedHeight = 32;
            SoloToggleStyle.fixedWidth = 32;
            SoloToggleStyle.stretchWidth = false;
            SoloToggleStyle.contentOffset = new Vector2(32, VisibilityToggleStyle.contentOffset.y);

            SoloToggleStyle.normal.background = SharedGraphics.IconSoloInactive;
            SoloToggleStyle.onNormal.background = SharedGraphics.IconSoloActive;

            #endregion


            #region --- lock toggle ---
            LockToggleStyle = new GUIStyle();
            LockToggleStyle.border = new RectOffset();
            LockToggleStyle.padding = new RectOffset();
            LockToggleStyle.margin = new RectOffset(0, 0, 0, 3);
            LockToggleStyle.alignment = TextAnchor.UpperCenter;
            LockToggleStyle.fixedHeight = 14;
            LockToggleStyle.fixedWidth = 14;
            LockToggleStyle.stretchWidth = false;
            //LockToggleStyle.contentOffset = new Vector2(32, VisibilityToggleStyle.contentOffset.y);

            LockToggleStyle.normal.background = SharedGraphics.IconUnlocked_18;
            LockToggleStyle.onNormal.background = SharedGraphics.IconLocked_18;

            #endregion



            #region --- aspect toggle ---
            AspectToggleStyle = new GUIStyle();
            AspectToggleStyle.border = new RectOffset();
            AspectToggleStyle.padding = new RectOffset();
            AspectToggleStyle.margin = new RectOffset(0, 0, 0, 3);
            AspectToggleStyle.alignment = TextAnchor.UpperCenter;
            AspectToggleStyle.fixedHeight = 14;
            AspectToggleStyle.fixedWidth = 14;
            AspectToggleStyle.stretchWidth = false;
            //LockToggleStyle.contentOffset = new Vector2(32, VisibilityToggleStyle.contentOffset.y);

            AspectToggleStyle.normal.background = SharedGraphics.IconAspect_18;
            AspectToggleStyle.onNormal.background = SharedGraphics.IconAspectActive_18;

            #endregion

            #region --- color channel toggle ---
            ColorChannelToggleStyle = new GUIStyle();
            ColorChannelToggleStyle.border = new RectOffset();
            ColorChannelToggleStyle.padding = new RectOffset();
            ColorChannelToggleStyle.margin = new RectOffset(0, 0, 0, 3);
            ColorChannelToggleStyle.alignment = TextAnchor.UpperCenter;
            ColorChannelToggleStyle.fixedHeight = 14;
            ColorChannelToggleStyle.fixedWidth = 14;
            ColorChannelToggleStyle.stretchWidth = false;
            //LockToggleStyle.contentOffset = new Vector2(32, VisibilityToggleStyle.contentOffset.y);

            ColorChannelToggleStyle.normal.background = SharedGraphics.IconChannel_Color_18;
            ColorChannelToggleStyle.onNormal.background = SharedGraphics.IconChannel_ColorAlpha_18;

            #endregion

            #region --- Solo Toggle Small ---
            SoloToggleSmallStyle = new GUIStyle();
            SoloToggleSmallStyle.border = new RectOffset();
            SoloToggleSmallStyle.padding = new RectOffset();
            SoloToggleSmallStyle.alignment = TextAnchor.MiddleLeft;
            SoloToggleSmallStyle.fixedHeight = 16;
            SoloToggleSmallStyle.fixedWidth = 16;
            SoloToggleSmallStyle.stretchWidth = false;
            SoloToggleSmallStyle.contentOffset = new Vector2(16, VisibilityToggleStyle.contentOffset.y);

            SoloToggleSmallStyle.normal.background = SharedGraphics.IconSoloInactiveSmall;
            SoloToggleSmallStyle.onNormal.background = SharedGraphics.IconSoloActiveSmall;

            #endregion

            #region --- radio toggle ---
            RadioToggleStyle = new GUIStyle();
            RadioToggleStyle.border = new RectOffset();
            RadioToggleStyle.padding = new RectOffset();
            RadioToggleStyle.alignment = TextAnchor.MiddleLeft;
            RadioToggleStyle.fixedHeight = 18;
            RadioToggleStyle.fixedWidth = 18;
            RadioToggleStyle.stretchWidth = false;
            RadioToggleStyle.contentOffset = new Vector2(18, RadioToggleStyle.contentOffset.y);

            RadioToggleStyle.normal.background = SharedGraphics.IconRadioButtonOff;
            RadioToggleStyle.onNormal.background = SharedGraphics.IconRadioButtonOn;

            #endregion


            #region --- switch toggle ---
            SwitchToggleStyle = new GUIStyle();
            SwitchToggleStyle.border = new RectOffset();
            SwitchToggleStyle.padding = new RectOffset();
            SwitchToggleStyle.alignment = TextAnchor.MiddleLeft;
            SwitchToggleStyle.fixedHeight = 12;
            SwitchToggleStyle.fixedWidth = 29;
            SwitchToggleStyle.stretchWidth = false;
            SwitchToggleStyle.contentOffset = new Vector2(12, 32);

            SwitchToggleStyle.normal.background = SharedGraphics.IconSwitchButtonOff;
            SwitchToggleStyle.onNormal.background = SharedGraphics.IconSwitchButtonOn;

            #endregion

            #region --- expander ---
            ExpanderStyle = new GUIStyle();
            ExpanderStyle.border = new RectOffset();
            ExpanderStyle.padding = new RectOffset();
            ExpanderStyle.alignment = TextAnchor.MiddleLeft;
            ExpanderStyle.fixedHeight = 12;
            ExpanderStyle.fixedWidth = 29;
            ExpanderStyle.stretchWidth = false;
            ExpanderStyle.contentOffset = new Vector2(12, 32);

            ExpanderStyle.normal.background = SharedGraphics.IconExpandUp;
            ExpanderStyle.onNormal.background = SharedGraphics.IconExpandDown;

            #endregion




            #endregion

            #region --- Labels ---

            DefaultLabel = new GUIStyle();
            DefaultLabel.normal.textColor = SharedColors.DefaultTextColor;
            DefaultLabel.onNormal.textColor = SharedColors.DefaultTextColor;
            DefaultLabel.alignment = TextAnchor.MiddleLeft;
            DefaultLabel.padding = new RectOffset(5, 0, 0, 0);
            DefaultLabel.stretchWidth = true;

            DefaultLabelWhite = new GUIStyle(DefaultLabel);
            DefaultLabelWhite.normal.textColor = Color.white;
            DefaultLabelWhite.onNormal.textColor = Color.white;


            DefaultLabelBold = new GUIStyle();
            DefaultLabelBold.normal.textColor = SharedColors.DefaultTextColor;
            DefaultLabelBold.onNormal.textColor = SharedColors.DefaultTextColor;
            DefaultLabelBold.fontStyle = FontStyle.Bold;
            DefaultLabelBold.alignment = TextAnchor.MiddleLeft;
            DefaultLabelBold.padding = new RectOffset(5, 0, 0, 0);
            DefaultLabelBold.stretchWidth = true;

            DefaultLabelWrap = new GUIStyle();
            DefaultLabelWrap.normal.textColor = SharedColors.DefaultTextColor;
            DefaultLabelWrap.onNormal.textColor = SharedColors.DefaultTextColor;
            DefaultLabelWrap.wordWrap = true;
            DefaultLabelWrap.alignment = TextAnchor.MiddleLeft;
            DefaultLabelWrap.padding = new RectOffset(5, 0, 0, 0);
            DefaultLabelWrap.stretchWidth = true;

            DefaultTitleLabel = new GUIStyle(DefaultLabel);
            DefaultTitleLabel.fontSize = 24;


            DefaultTitleLabelBold = new GUIStyle(DefaultLabelBold);
            DefaultTitleLabelBold.fontSize = 24;

            DefaultRatingLabel = new GUIStyle(DefaultLabel);
            DefaultRatingLabel.fontSize = 16;
            DefaultRatingLabel.richText = true;

            LinkLabel = new GUIStyle();
            LinkLabel.normal.textColor = SharedColors.UIBlue;
            LinkLabel.hover.textColor = SharedColors.DefaultTextColor;
            LinkLabel.alignment = TextAnchor.MiddleLeft;
            LinkLabel.padding = new RectOffset(5, 0, 0, 0);
            LinkLabel.stretchWidth = true;

            LinkLabelLight = new GUIStyle();
            LinkLabelLight.normal.textColor = SharedColors.SkyBlue;
            LinkLabelLight.hover.textColor = SharedColors.DefaultTextColor;
            LinkLabelLight.alignment = TextAnchor.MiddleLeft;
            LinkLabelLight.padding = new RectOffset(5, 0, 0, 0);
            LinkLabelLight.stretchWidth = true;


            LabelSmallWhite = new GUIStyle();
            LabelSmallWhite.normal.textColor = Color.white;
            LabelSmallWhite.fontSize = SmallFontSize;

            LabelSmallWhiteRight = new GUIStyle(LabelSmallWhite);
            LabelSmallWhiteRight.alignment = TextAnchor.MiddleRight;

            #endregion

            #region --- Side Toolbar ---
            SideToolbarStyle = new GUIStyle();
            SideToolbarStyle.normal.background = SharedGraphics.LayerBackground;
            SideToolbarStyle.fixedWidth = 50;
            SideToolbarStyle.border = new RectOffset(2, 2, 2, 2);


            SideToolbarItemStyle = new GUIStyle();
            SideToolbarItemStyle.border = new RectOffset(4, 4, 4, 4);
            SideToolbarItemStyle.padding = new RectOffset();
            SideToolbarItemStyle.margin = new RectOffset(9, 9, 2, 2);
            SideToolbarItemStyle.fontSize = 10;
            SideToolbarItemStyle.alignment = TextAnchor.MiddleCenter;
            SideToolbarItemStyle.fixedWidth = 32;
            SideToolbarItemStyle.fixedHeight = 32;

            SideToolbarItemStyle.normal.background = SharedGraphics.ToolbarItemHoverBackground;
            SideToolbarItemStyle.normal.textColor = Color.white;

            SideToolbarItemStyle.onNormal.background = SharedGraphics.ToolbarItemActiveBackground;
            SideToolbarItemStyle.onNormal.textColor = SharedColors.SkyBlue;

            SideToolbarItemStyle.active.background = SharedGraphics.ToolbarItemActiveBackground;
            SideToolbarItemStyle.active.textColor = SharedColors.SkyBlue;

            #endregion

            #region --- Buttons ---

            TransparentButton = new GUIStyle();
            TransparentButton.normal.textColor = Color.white;
            TransparentButton.onNormal.textColor = SharedColors.UIBlue;
            TransparentButton.alignment = TextAnchor.MiddleCenter;
            TransparentButton.padding = new RectOffset(0, 0, 0, 0);


            PaginationButton = new GUIStyle();
            PaginationButton.normal.textColor = Color.white;
            PaginationButton.normal.background = SharedGraphics.BoxBackground;
            PaginationButton.fontSize = 10;
            PaginationButton.alignment = TextAnchor.MiddleCenter;
            PaginationButton.margin = new RectOffset(5, 0, 5, 5);
            PaginationButton.fixedWidth = 25;
            PaginationButton.fixedHeight = 25;

            PaginationActiveButton = new GUIStyle(PaginationButton);
            PaginationActiveButton.normal.background = SharedGraphics.BoxActiveBackground;

            RatingButton = new GUIStyle();
            RatingButton.normal.textColor = Color.white;
            RatingButton.fontSize = 18;
            RatingButton.alignment = TextAnchor.MiddleCenter;
            RatingButton.padding = new RectOffset(0, 0, 0, 0);
            RatingButton.richText = true;
            RatingButton.fixedWidth = 15;




            #endregion

            WaitBoxStyle = new GUIStyle();
            WaitBoxStyle.normal.background = SharedGraphics.IconWait_32;

        }
    }
}
