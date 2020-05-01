﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using WarWolfWorks.Internal;
using WarWolfWorks.NyuEntities;
using WarWolfWorks.NyuEntities.ProjectileSystem;
using WarWolfWorks.IO.CTS;
using WarWolfWorks.Utility;

[assembly: InternalsVisibleTo("WarWolfWorks.EditorBase.Services")]
[assembly: InternalsVisibleTo("WarWolfWorks.EditorBase")]
namespace WarWolfWorks
{
    /// <summary>
    /// All constant and readonly values of the <see cref="WarWolfWorks"/> library.
    /// </summary>
    public static class Constants
    {
        #region Streaming
        internal const int STREAMING_FILE_ENCRYPTION_JUMPER = 85;

        /// <summary>
        /// Path to personal preferences of this library.
        /// </summary>
        public static readonly string SV_Path_Preferences =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"WarWolfWorks\",
                Application.productName,
                "Preferences.cts");
        /// <summary>
        /// Path to the WWWSettings.cts file.
        /// </summary>
        public static readonly string SV_Path_Settings = Hooks.Streaming.GetStreamingAssetsFilePath("WWWSettings.cts");
        /// <summary>
        /// Path to DefaultKeys.cts.
        /// </summary>
        internal static readonly string SV_Path_DefaultKeys = Path.Combine(Application.streamingAssetsPath.Replace("/", "\\"), "DefaultKeys.cts");
        /// <summary>
        /// Category name for keys.
        /// </summary>
        internal const string SVCN_KEYS = "Keys";
        /// <summary>
        /// Default variable split character for saving.
        /// </summary>
        public const char SV_DEFAULT_SPLIT = ';';
        #endregion

        #region Regex Expressions
        /// <summary>
        /// Matches any string value that starts with "s_" or "S_".
        /// </summary>
        public static readonly Regex Expression_NoS = new Regex(@"^[sS]_");
        #endregion

        #region WarWolfWorks Settings
        /// <summary>
        /// Name of the category given to the <see cref="Settings.UtilityCanvas"/> Settings in WWWSettings.ini.
        /// </summary>
        internal const string SVCN_CORECANVAS = "ユチリテイキャンバス";
        internal const string VN_DEFAULT_CANVAS = "WWWCanvas";
        internal const string SVCN_OTHER = "その他";
        internal const string SVCN_NYUENTITIES = "ニューエンチチズ";
        internal const string SVN_SETTINGS_LANGUAGE = "言語";
        /// <summary>
        /// Name of the category given to <see cref="AdvancedDebug"/> settings in WWWSettings.ini.
        /// </summary>
        internal const string SVCN_DEBUG = "デバッグ";
        internal static readonly string[] SVS_Layers_State = new string[] { " With Active State " };
        internal const string SVN_AD_LAYER = "レイヤー";
        internal const string SVN_AD_STYLE = "STYLE";
        internal const string SVN_AD_COLOR_LOG = "COLOR_LOG";
        internal const string SVN_AD_COLOR_WARNING = "COLOR_WARNING";
        internal const string SVN_AD_COLOR_ERROR = "COLOR_ERROR";

        internal const string SVN_UC_TYPE = "UTILITY_CANVAS_TYPE";
        internal const string SVN_UC_RESOURCES_PATH = "UTILITY_CANVAS_RESOURCES_PATH";
        internal const string SVN_UC_NAME_LOAD = "SVARN_UC_NAME_LOAD";
        #endregion

        #region Language Strings
        internal static readonly LanguageString LS_Unknown = new LanguageString(
            "Unknown",
            ("Nieznany", SystemLanguage.Polish),
            ("不明", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Add = new LanguageString(
            "Add",
            ("Dodaj", SystemLanguage.Polish),
            ("追加", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Remove = new LanguageString(
            "Remove",
            ("Usuń", SystemLanguage.Polish),
            ("削除", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_RemoveExpanded = new LanguageString(
            "Remove Expanded",
            ("Usuń Rozwinięte", SystemLanguage.Polish),
            ("拡したを削除", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Value = new LanguageString(
           "Value",
           ("Waluta", SystemLanguage.Polish),
           ("バリュー", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Stat_Affections = new LanguageString(
           "Affections",
           ("Afekty", SystemLanguage.Polish),
           ("アフェクションズ", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Stat_Stacking = new LanguageString(
           "Stacking",
           ("Stakowanie", SystemLanguage.Polish),
           ("スタッキング", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Confirm = new LanguageString(
            "Confirm",
            ("Potwierdz", SystemLanguage.Polish),
            ("追認", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Previous = new LanguageString("Previous", ("Poprzedni", SystemLanguage.Polish), ("前に", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Next = new LanguageString("Next", ("Następny", SystemLanguage.Polish), ("次に", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Type = new LanguageString("Type", ("Typ", SystemLanguage.Polish), ("タイプ", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Check = new LanguageString("Check", ("Sprawdź", SystemLanguage.Polish), ("確認", SystemLanguage.Japanese));
        internal static readonly LanguageString LS_Reset = new LanguageString("Reset", ("Resetuj", SystemLanguage.Polish), ("リセット", SystemLanguage.Japanese));
        #endregion

        #region Compile Time
        internal const string V_SCATALOGSAVE_OBSOLETE_MESSAGE = "The Streaming.Catalog system is replaced by WarWolfWorks.Streaming.CTS and will be removed in the very near future; Consider switching to WarWolfWorks.Streaming.CTS.";
        internal const bool V_SCATALOGSAVE_OBSOLETE_ISERROR = false;
        #endregion

        #region Unity Stuff
        /// <summary>
        /// An instance of <see cref="WaitForFixedUpdate"/>.
        /// </summary>
        public static readonly WaitForFixedUpdate FixedUpdateWaiter = new WaitForFixedUpdate();
        /// <summary>
        /// An instance of <see cref="WaitForEndOfFrame"/>.
        /// </summary>
        public static readonly WaitForEndOfFrame LateUpdateWaiter = new WaitForEndOfFrame();
        #endregion

        #region Projectiles
        /// <summary>
        /// Name of the <see cref="NyuProjectile"/> game object.
        /// </summary>
        public const string VN_PROJECTILE_HOLDER = "Projectiles";

        /// <summary>
        /// Name of projectile objects in the scene.
        /// </summary>
        public const string VN_PROJECTILE = "Projectile_";
        #endregion

        #region Nyu Entities
        /// <summary>
        /// Name of the <see cref="NyuManager"/>.
        /// </summary>
        public const string VN_NYUMANAGER = "NyuManager";
        #endregion

        #region Misc
        internal const int V_AUDIOMANAGER_MIN_POOLSIZE = 15;
        internal const int V_TRANSITIONMANAGER_TRANSITIONS_SIZE = 32;
        #endregion

        #region CTS
        internal static readonly Catalog CTS_Settings_CoreCanvas = new Catalog(SV_Path_Settings, SVCN_CORECANVAS);
        internal static readonly Catalog CTS_Settings_AdvancedDebug = new Catalog(SV_Path_Settings, SVCN_DEBUG);
        internal static readonly Catalog CTS_Settings_Misc = new Catalog(SV_Path_Settings, SVCN_OTHER);
        internal static readonly Catalog CTS_Preferences_AdvancedDebug = new Catalog(SV_Path_Preferences, SVCN_DEBUG);
        internal static readonly Catalog CTS_Preferences_Misc = new Catalog(SV_Path_Preferences, SVCN_DEBUG);

        internal static readonly Catalog CTS_DefaultKeys = new Catalog(SV_Path_DefaultKeys, SVCN_KEYS);
        #endregion
    }
}
