﻿using CodeStream.VisualStudio.Extensions;
using CodeStream.VisualStudio.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Drawing;

using EnvironmentColors = Microsoft.VisualStudio.PlatformUI.EnvironmentColors;
using FontFamily = System.Windows.Media.FontFamily;
using VSColorTheme = Microsoft.VisualStudio.PlatformUI.VSColorTheme;
// ReSharper disable RedundantArgumentDefaultValue

namespace CodeStream.VisualStudio.Services
{
    public class ColorInfo
    {
        public string Key { get; set; }
        public ThemeResourceKey VisualStudioKey { get; set; }
        public Func<System.Drawing.Color, string> Modifier { get; set; }
        public string Value { get; set; }
    }

    public class ThemeInfo
    {
        public List<ColorInfo> ColorInfo { get; set; }
        public bool IsDark { get; set; }
    }

    public static class ThemeManager
    {
        private static readonly ThemeResourceKey BackgroundThemeResourceKey = EnvironmentColors.ToolWindowBackgroundColorKey;
        private static int DefaultFontSize = 12;

        private static List<ColorInfo> GetColorInfoBase()
        {
            return new List<ColorInfo>
                {
                    new ColorInfo { Key = "app-background-color", VisualStudioKey = BackgroundThemeResourceKey },

                    new ColorInfo { Key = "background-color",                       VisualStudioKey = BackgroundThemeResourceKey},

                    new ColorInfo { Key = "vscode-button-hoverBackground",          VisualStudioKey = EnvironmentColors.ToolWindowButtonDownBorderColorKey},
                    new ColorInfo { Key = "vscode-sideBarSectionHeader-background", VisualStudioKey = EnvironmentColors.ToolWindowContentGridColorKey},
                    new ColorInfo { Key = "vscode-sideBarSectionHeader-foreground", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey},

                    new ColorInfo { Key = "scrollbar-color",                        VisualStudioKey =  EnvironmentColors.SystemScrollBarColorKey},
                    new ColorInfo { Key = "scrollbar-color-hover",                  VisualStudioKey = EnvironmentColors.ScrollBarThumbMouseOverBackgroundColorKey }
                    
                    //new ColorInfo { Key = "vs-accent-color",                        VisualStudioKey = EnvironmentColors.ToolWindowButtonInactiveColorKey},

                    //new ColorInfo { Key = "vs-btn-background",                      VisualStudioKey = EnvironmentColors.ToolWindowButtonInactiveGlyphColorKey},
                    //new ColorInfo { Key = "vs-btn-color",                           VisualStudioKey = EnvironmentColors.ToolWindowButtonHoverActiveGlyphColorKey},

                    //new ColorInfo { Key = "vs-btn-primary-background",              VisualStudioKey = EnvironmentColors.ToolWindowButtonDownColorKey},
                    //new ColorInfo { Key = "vs-btn-primary-color",                   VisualStudioKey = EnvironmentColors.ToolWindowButtonDownActiveGlyphColorKey},
                    
                    //new ColorInfo { Key = "vs-background-color",                    VisualStudioKey = EnvironmentColors.ToolWindowButtonHoverInactiveColorKey},
                    //new ColorInfo { Key = "vs-input-background",                    VisualStudioKey = EnvironmentColors.ToolWindowButtonHoverInactiveColorKey},
                    //new ColorInfo { Key = "vs-background-accent",                   VisualStudioKey = EnvironmentColors.ToolWindowTabMouseOverBackgroundBeginColorKey},
                    //new ColorInfo { Key = "vs-input-outline",                       VisualStudioKey = EnvironmentColors.ToolWindowButtonHoverActiveBorderColorKey},
                    //new ColorInfo { Key = "vs-post-separator",                      VisualStudioKey = EnvironmentColors.ToolWindowBorderColorKey},
                };
        }

        public static ThemeInfo Generate()
        {
            var backgroundColor = VSColorTheme.GetThemedColor(BackgroundThemeResourceKey);
            // assume this theme is 'dark' if the ToolWindow background is dark

            var isDark = backgroundColor.IsDark();
            var colorInfos = GetColorInfoBase();

            if (isDark)
            {
                colorInfos.Add(new ColorInfo { Key = "app-background-color-darker", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Darken(0.04f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "app-background-color-hover", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Lighten(0.03f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "app-background-image-color", Value = "#fff" });

                colorInfos.Add(new ColorInfo { Key = "base-background-color", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Lighten(0.04f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "base-border-color", VisualStudioKey = EnvironmentColors.ToolWindowBorderColorKey, Modifier = (b) => b.Lighten(0.08f).ToHex() });

                colorInfos.Add(new ColorInfo { Key = "color", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey });

                colorInfos.Add(new ColorInfo { Key = "link-color", VisualStudioKey = EnvironmentColors.StartPageTextControlLinkSelectedColorKey });

                colorInfos.Add(new ColorInfo { Key = "text-color", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.ToArgb(80) });
                colorInfos.Add(new ColorInfo { Key = "text-color-info", VisualStudioKey = EnvironmentColors.StartPageTextControlLinkSelectedColorKey });
                colorInfos.Add(new ColorInfo { Key = "text-color-info-muted", VisualStudioKey = EnvironmentColors.ToolWindowButtonDownBorderColorKey, Modifier = (b) => b.Darken(0.1f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "text-color-subtle", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.ToArgb(70) });
                colorInfos.Add(new ColorInfo { Key = "text-color-subtle-extra", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.Lighten(0.5f).ToArgb(60) });

                colorInfos.Add(new ColorInfo { Key = "tool-panel-background-color", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Lighten(0.1f).ToHex() });

                colorInfos.Add(new ColorInfo { Key = "vscode-button-background", VisualStudioKey = EnvironmentColors.ToolWindowButtonDownColorKey });
            }
            else
            {
                // for the light themes -- VS uses a harder-to-read background on the active ToolWindow, 
                // that makes for a harder-to-read link/button. So for lighter themes, we se the ToolWindowButtonInactiveColorKey instead

                colorInfos.Add(new ColorInfo { Key = "app-background-color-darker", VisualStudioKey = BackgroundThemeResourceKey });
                colorInfos.Add(new ColorInfo { Key = "app-background-color-hover", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Darken(0.15f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "app-background-image-color", Value = "#000" });

                colorInfos.Add(new ColorInfo { Key = "base-background-color", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Darken(0.1f).ToHex() });
                colorInfos.Add(new ColorInfo { Key = "base-border-color", VisualStudioKey = EnvironmentColors.ToolWindowBorderColorKey, Modifier = (b) => b.Darken(0.1f).ToHex() });

                colorInfos.Add(new ColorInfo { Key = "color", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey });

                colorInfos.Add(new ColorInfo { Key = "link-color", VisualStudioKey = EnvironmentColors.ToolWindowButtonInactiveColorKey });

                colorInfos.Add(new ColorInfo { Key = "text-color", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.ToArgb(90) });
                colorInfos.Add(new ColorInfo { Key = "text-color-info", VisualStudioKey = EnvironmentColors.StartPageTextControlLinkSelectedColorKey });
                colorInfos.Add(new ColorInfo { Key = "text-color-info-muted", VisualStudioKey = EnvironmentColors.ToolWindowButtonDownBorderColorKey });
                colorInfos.Add(new ColorInfo { Key = "text-color-subtle", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.ToArgb(70) });
                colorInfos.Add(new ColorInfo { Key = "text-color-subtle-extra", VisualStudioKey = EnvironmentColors.ToolWindowTextColorKey, Modifier = (b) => b.Darken(0.5f).ToArgb(60) });

                colorInfos.Add(new ColorInfo { Key = "tool-panel-background-color", VisualStudioKey = BackgroundThemeResourceKey, Modifier = (b) => b.Darken(0.1f).ToHex() });

                colorInfos.Add(new ColorInfo { Key = "vscode-button-background", VisualStudioKey = EnvironmentColors.ToolWindowButtonInactiveColorKey });
            }

            var fontFamilyString = "Arial, Consolas, sans-serif";
            var fontFamily = System.Windows.Application.Current.FindResource(VsFonts.EnvironmentFontFamilyKey) as FontFamily;
            if (fontFamily != null)
            {
                fontFamilyString = fontFamily.ToString();
                if (fontFamilyString.Contains(" "))
                {
                    fontFamilyString = $"\"{fontFamilyString}\"";
                }
            }

            colorInfos.Add(new ColorInfo { Key = "vscode-editor-font-family", Value = fontFamilyString });
            colorInfos.Add(new ColorInfo { Key = "font-family", Value = fontFamilyString });

            var metrics = CreateEditorMetrics(null);
            var fontSize = metrics == null ?
                DefaultFontSize.ToString() :
                metrics.FontSize.ToIntSafe(DefaultFontSize).ToString();

            colorInfos.Add(new ColorInfo { Key = "font-size", Value = fontSize });

            return new ThemeInfo
            {
                ColorInfo = colorInfos,
                IsDark = isDark
            };
        }

        public static EditorMetrics CreateEditorMetrics(IWpfTextView textView)
        {
            return new EditorMetrics
            {
                LineHeight = textView?.LineHeight.ToInt(),
                FontSize = System.Windows.Application.Current.FindResource(VsFonts.EnvironmentFontSizeKey).ToIntSafe(DefaultFontSize),
                EditorMargins = new EditorMargins
                {
                    //TODO figure out the real value here...
                    Top = 21
                }
            };
        }

        private static Color DefaultColor = Color.FromArgb(0, 110, 183);

        public static System.Drawing.Color GetColorSafe(string colorName)
        {
            if (colorName.IsNullOrWhiteSpace()) return DefaultColor;

            if (ColorMap.TryGetValue(colorName, out Color value))
            {
                return value;
            }

            return DefaultColor;
        }

        public static Dictionary<string, System.Drawing.Color> ColorMap = new Dictionary<string, System.Drawing.Color>
        {
            { "blue", DefaultColor},
            { "green", Color.FromArgb(88, 181, 71)},
            { "yellow", Color.FromArgb(240, 208, 5)},
            { "orange", Color.FromArgb(255, 147, 25)},
            { "red",  Color.FromArgb(232, 78, 62)},
            { "purple", Color.FromArgb(187, 108, 220)},
            { "aqua", Color.FromArgb(0, 186, 220)},
            { "gray", Color.FromArgb(127, 127, 127)}
        };

        /*
            /// <summary>
            /// this is some helper code to generate a theme color palette from the current VS theme
            /// </summary>
            /// <returns></returns>
            private static string GenerateVisualStudioColorTheme()
            {
                var d = new System.Collections.Generic.Dictionary<string, string>();
                Type type = typeof(EnvironmentColors); // MyClass is static class with static properties
                foreach (var p in type.GetProperties().Where(_ => _.Name.StartsWith("ToolWindow")))
                {
                    var val = typeof(EnvironmentColors).GetProperty(p.Name, BindingFlags.Public | BindingFlags.Static);
                    var v = val.GetValue(null);
                    var trk = v as ThemeResourceKey;
                    if (trk != null)
                    {
                        var color = VSColorTheme.GetThemedColor(trk);
                        d.Add(p.Name, color.ToHex());
                    }

                    // d.Add(p.Name, ((System.Drawing.Color)val).ToHex());
                }

                string s = "";
                foreach (var kvp in d)
                {
                    s += $@"<div>";
                    s += $@"<span style='display:inline-block; height:50ps; width: 50px; background:{kvp.Value}; padding-right:5px; margin-right:5px;'>&nbsp;</span>";
                    s += $@"<span>{kvp.Value} - {kvp.Key}</span>";
                    s += "</div>";
                }

                return null;
            }
        */
    }

}