using BepInEx;
using BepInEx.Bootstrap;
using TMPro;
using RoR2;
using RoR2.UI;
using R2API.Utils;
using UnityEngine;
using System.Reflection;
using MonoMod.RuntimeDetour;
using MaterialHud;



namespace RoR2ThaiFont
{
    [BepInDependency("bubbet.riskui" , BepInDependency.DependencyFlags.SoftDependency)]

    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]

    [BepInPlugin("com.thunninoi.ror2thai" , "RoR2Thai", "1.0.0")]

    public class RoR2ThaiFontSupport : BaseUnityPlugin
    {
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("RoR2ThaiFont.thaifont_assets"));
        
        public static TMP_FontAsset fontRSU;
        public static TMP_FontAsset fontNotoThai;

        public static TMP_FontAsset fontBombDefault;
        public static TMP_FontAsset fontCostDefault;
        

        //riskui stuff
        RiskUIPlugin RiskUiInstance = (RiskUIPlugin)Chainloader.PluginInfos["bubbet.riskui"].Instance;

        public static TMP_FontAsset riskui_chatfont; //Montserrat

        public void Awake()
        {
            fontBombDefault = Resources.Load<TMP_FontAsset>("TmpFonts/Bombardier/tmpBombDropShadow");
            fontCostDefault = Resources.Load<TMP_FontAsset>("TmpFonts/Traceroute/tmpTRACER_SDFDAMAGENUMBER.asset");

            fontRSU = assetBundle.LoadAsset<TMP_FontAsset>("Assets/ThaiFont/RSU_modified.asset");
            fontNotoThai = assetBundle.LoadAsset<TMP_FontAsset>("Assets/ThaiFont/NotoSansThai.asset");

            riskui_chatfont = RiskUiInstance.assetBundle.LoadAsset<TMP_FontAsset>("Montserrat-Medium SDF");

            On.RoR2.UI.HGTextMeshProUGUI.OnCurrentLanguageChanged += HGTextMeshProUGUI_OnCurrentLanguageChanged;

            new Hook(typeof(TextMeshProUGUI).GetMethod("LoadFontAsset", (BindingFlags)(-1)), new System.Action<System.Action<TextMeshProUGUI>, TextMeshProUGUI>((orig, self) =>
            {
                orig(self);
                if (self.font == fontBombDefault) self.font = fontRSU;
                if (self.font == fontCostDefault) self.font = fontRSU;
                if (self.font == riskui_chatfont) self.font = fontNotoThai;
            }));
        }

          
        public void HGTextMeshProUGUI_OnCurrentLanguageChanged(On.RoR2.UI.HGTextMeshProUGUI.orig_OnCurrentLanguageChanged orig)
        {
            orig();
            if (fontRSU) HGTextMeshProUGUI.defaultLanguageFont = fontRSU;
        }
    }
}