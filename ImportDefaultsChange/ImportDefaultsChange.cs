using System.Collections.Generic;
using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using CodeX;

namespace ImportDefaultsChange
{
    public class ImportDefaultsChange : NeosMod
    {
        public override string Name => "ImportDefaultsChange";
        public override string Author => "AlexW-578";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/AlexW-578/ImportDefaultsChange/";

        private static ModConfiguration Config;

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Enabled =
            new ModConfigurationKey<bool>("Enabled", "Enable/Disable the Mod", () => true);

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_1", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_1_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_1_1 ", $"<color=green>[Default 3D Model Preset]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<float> Scale =
            new ModConfigurationKey<float>("Scale", computeDefault: () => 1f);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> AutoScale =
            new ModConfigurationKey<bool>("Auto Scale", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<int> MaxTextureSize =
            new ModConfigurationKey<int>("Max Texture Size", computeDefault: () => -1);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<ModelImportDialog.MaterialType> Material =
            new ModConfigurationKey<ModelImportDialog.MaterialType>("Material",
                computeDefault: () => ModelImportDialog.MaterialType.PBS);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CalculateNormals =
            new ModConfigurationKey<bool>("Calculate Normals", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CalculateTangents =
            new ModConfigurationKey<bool>("Calculate Tangents", computeDefault: () => true);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportBones =
            new ModConfigurationKey<bool>("Import Bones", computeDefault: () => true);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CalculateTextureAlpha =
            new ModConfigurationKey<bool>("Calculate Texture Alpha", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportAlbedoColour =
            new ModConfigurationKey<bool>("Import Albedo Colour", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportEmissive =
            new ModConfigurationKey<bool>("Import Emissive", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Colliders =
            new ModConfigurationKey<bool>("Generate Colliders", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Animations =
            new ModConfigurationKey<bool>("Import Animations", computeDefault: () => true);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Timelapse =
            new ModConfigurationKey<bool>("Setup as Timelapse", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ExternalTextures =
            new ModConfigurationKey<bool>("Import External Textures", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Rig =
            new ModConfigurationKey<bool>("Import Skinned Meshes", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> SetupIk =
            new ModConfigurationKey<bool>("Setup IK", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> DebugRig =
            new ModConfigurationKey<bool>("Visualise Rig", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ForceTpose =
            new ModConfigurationKey<bool>("Force T-Pose", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> AsPointCloud =
            new ModConfigurationKey<bool>("As Point Cloud", computeDefault: () => false);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportImagesByName =
            new ModConfigurationKey<bool>("Import Images by Name", computeDefault: () => false);


        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Grabbable =
            new ModConfigurationKey<bool>("Make Grabbable", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scalable =
            new ModConfigurationKey<bool>("Make Scalable", computeDefault: () => true);


        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_2 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_2", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_2_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_2_1 ", $"<color=green>[Advanced Defaults]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_2_2 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_2_2 ",
                $"<color=gray>[Applies to All Model Imports Unless Overridden]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<TextureConversion> TextureConversion =
            new ModConfigurationKey<TextureConversion>("Image Format",
                computeDefault: () => FrooxEngine.TextureConversion.Auto);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PreferSpecular =
            new ModConfigurationKey<bool>("Prefer Specular", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportVertexColours =
            new ModConfigurationKey<bool>("Import Vertex Colours", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportLights =
            new ModConfigurationKey<bool>("Import Lights", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Snappable =
            new ModConfigurationKey<bool>("Setup as Snappable", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> MakeDualSided =
            new ModConfigurationKey<bool>("Make Dual Sided", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> MakeFlatShaded =
            new ModConfigurationKey<bool>("Make Flat Shaded", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> DeduplicateInstances =
            new ModConfigurationKey<bool>("Deduplicate Instances (slow)", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> OptimizeModel =
            new ModConfigurationKey<bool>("Optimize Model/Scene", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> SplitSubmeshes =
            new ModConfigurationKey<bool>("Split Submeshes", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> GenerateRandomColours =
            new ModConfigurationKey<bool>("Generate Random Colours", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> SpawnMaterialOrbs =
            new ModConfigurationKey<bool>("Spawn Material Orbs", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ForcePointFiltering =
            new ModConfigurationKey<bool>("Force Point Filtering", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ForceNoMipMaps =
            new ModConfigurationKey<bool>("Force No MipMaps", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ForceUncompressed =
            new ModConfigurationKey<bool>("Force Uncompressed", computeDefault: () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<ModelImportDialog.AlignmentAxis> ImportImageAlignment =
            new ModConfigurationKey<ModelImportDialog.AlignmentAxis>("Align Axis",
                computeDefault: () => ModelImportDialog.AlignmentAxis.PosX);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ImportAtOrigin =
            new ModConfigurationKey<bool>("Position At Origin", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> AssetsOnObject =
            new ModConfigurationKey<bool>("Place Assets On Object", computeDefault: () => false);

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_2_4 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_2", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_2_3 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_3_1 ", $"<color=green>[Separable Defaults]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Separable_Snappable =
            new ModConfigurationKey<bool>("Setup as Snappable ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_3 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_3", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_3_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_3_1 ", $"<color=green>[3D Scan Defaults]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_AsPointCloud =
            new ModConfigurationKey<bool>("As Point Cloud ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_ImportImagesByName =
            new ModConfigurationKey<bool>("Import Images By Name ", computeDefault: () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<ModelImportDialog.MaterialType> Scan_Material =
            new ModConfigurationKey<ModelImportDialog.MaterialType>("Material ",
                computeDefault: () => ModelImportDialog.MaterialType.Unlit);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<float> Scan_Scale =
            new ModConfigurationKey<float>("Scale ", computeDefault: () => 1f);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_AutoScale =
            new ModConfigurationKey<bool>("Auto Scale ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_Colliders =
            new ModConfigurationKey<bool>("Generate Colliders ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_Animations =
            new ModConfigurationKey<bool>("Import Animations ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_Rig =
            new ModConfigurationKey<bool>("Import Skinned Meshes ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_SetupIk =
            new ModConfigurationKey<bool>("Setup IK ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_DebugRig =
            new ModConfigurationKey<bool>("Visualise Rig ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_Timelapse =
            new ModConfigurationKey<bool>("Setup as Timelapse ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_ExternalTextures =
            new ModConfigurationKey<bool>("Import External Textures ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Scan_ImportAlbedoColour =
            new ModConfigurationKey<bool>("Import Albedo Colour ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_4 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_4", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_4_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_4_1 ", $"<color=green>[CAD Defaults]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_Scalable =
            new ModConfigurationKey<bool>("Scalable  ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_AsPointCloud =
            new ModConfigurationKey<bool>("As Point Cloud ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_ImportImagesByName =
            new ModConfigurationKey<bool>("Import Images By Name  ", computeDefault: () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<ModelImportDialog.MaterialType> CAD_Material =
            new ModConfigurationKey<ModelImportDialog.MaterialType>("Material  ",
                computeDefault: () => ModelImportDialog.MaterialType.PBS_DualSided);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<float> CAD_Scale =
            new ModConfigurationKey<float>("Scale  ", computeDefault: () => 1f);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_AutoScale =
            new ModConfigurationKey<bool>("Auto Scale  ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_Colliders =
            new ModConfigurationKey<bool>("Generate Colliders  ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_Animations =
            new ModConfigurationKey<bool>("Import Animations  ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_Rig =
            new ModConfigurationKey<bool>("Import Skinned Meshes  ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_SetupIk =
            new ModConfigurationKey<bool>("Setup IK  ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_DebugRig =
            new ModConfigurationKey<bool>("Visualise Rig  ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_Timelapse =
            new ModConfigurationKey<bool>("Setup as Timelapse  ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> CAD_ExternalTextures =
            new ModConfigurationKey<bool>("Import External Textures  ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_5 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_5", "<size=0></size>", () => new dummy());

        [AutoRegisterConfigKey] private static ModConfigurationKey<dummy> DUMMY_SEP_5_1 =
            new ModConfigurationKey<dummy>("DUMMY_SEP_5_1 ", $"<color=green>[Point Cloud Defaults]</color>",
                () => new dummy());

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_AsPointCloud =
            new ModConfigurationKey<bool>("As Point Cloud   ", computeDefault: () => true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<ModelImportDialog.MaterialType> PC_Material =
            new ModConfigurationKey<ModelImportDialog.MaterialType>("Material   ",
                computeDefault: () => ModelImportDialog.MaterialType.UnlitBillboard);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<float> PC_Scale =
            new ModConfigurationKey<float>("Scale   ", computeDefault: () => 1f);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_AutoScale =
            new ModConfigurationKey<bool>("Auto Scale   ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_Colliders =
            new ModConfigurationKey<bool>("Generate Colliders   ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_Animations =
            new ModConfigurationKey<bool>("Import Animations   ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_Rig =
            new ModConfigurationKey<bool>("Import Skinned Meshes   ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_SetupIK =
            new ModConfigurationKey<bool>("Setup IK   ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_DebugRig =
            new ModConfigurationKey<bool>("Visualise Rig   ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_Timelapse =
            new ModConfigurationKey<bool>("Setup as Timelapse   ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_ExternalTextures =
            new ModConfigurationKey<bool>("Import External Textures   ", computeDefault: () => false);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_ImportVertexColours =
            new ModConfigurationKey<bool>("Import Vertex Colours   ", computeDefault: () => true);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> PC_ImportAlbedoColor =
            new ModConfigurationKey<bool>("Import Albedo Colour   ", computeDefault: () => false);

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("co.uk.AlexW-578.ImportDefaultsChange");
            harmony.PatchAll();
        }

        public class Defaults
        {
            public static void setDefaults(ModelImportDialog __instance)
            {
                __instance._asPointCloud.Value = Config.GetValue(AsPointCloud);
                __instance._importImagesByName.Value = Config.GetValue(ImportImagesByName);
                __instance._material.Value = Config.GetValue(Material);
                __instance._scale.Value = Config.GetValue(Scale);
                __instance._autoScale.Value = Config.GetValue(AutoScale);
                __instance._colliders.Value = Config.GetValue(Colliders);
                __instance._animations.Value = Config.GetValue(Animations);
                __instance._rig.Value = Config.GetValue(Rig);
                __instance._setupIK.Value = Config.GetValue(SetupIk);
                __instance._calculateNormals.Value = Config.GetValue(CalculateNormals);
                __instance._calculateTangents.Value = Config.GetValue(CalculateTangents);
                __instance._calculateTextureAlpha.Value = Config.GetValue(CalculateTextureAlpha);
                __instance._importAlbedoColor.Value = Config.GetValue(ImportAlbedoColour);
                __instance._importEmissive.Value = Config.GetValue(ImportEmissive);
                __instance._importBones.Value = Config.GetValue(ImportBones);
                __instance._grabbable.Value = Config.GetValue(Grabbable);
                __instance._scalable.Value = Config.GetValue(Scalable);
                __instance._debugRig.Value = Config.GetValue(DebugRig);
                __instance._timelapse.Value = Config.GetValue(Timelapse);
                __instance._externalTextures.Value = Config.GetValue(ExternalTextures);
                __instance._forceTpose.Value = Config.GetValue(ForceTpose);
                __instance._maxTextureSize.Value = Config.GetValue(MaxTextureSize);

                __instance._textureConversion.Value = Config.GetValue(TextureConversion);
                __instance._preferSpecular.Value = Config.GetValue(PreferSpecular);
                __instance._importVertexColors.Value = Config.GetValue(ImportVertexColours);
                __instance._importLights.Value = Config.GetValue(ImportLights);
                __instance._snappable.Value = Config.GetValue(Snappable);
                __instance._makeDualSided.Value = Config.GetValue(MakeDualSided);
                __instance._makeFlatShaded.Value = Config.GetValue(MakeFlatShaded);
                __instance._deduplicateInstances.Value = Config.GetValue(DeduplicateInstances);
                __instance._optimizeModel.Value = Config.GetValue(OptimizeModel);
                __instance._splitSubmeshes.Value = Config.GetValue(SplitSubmeshes);
                __instance._generateRandomColors.Value = Config.GetValue(GenerateRandomColours);
                __instance._spawnMaterialOrbs.Value = Config.GetValue(SpawnMaterialOrbs);
                __instance._forcePointFiltering.Value = Config.GetValue(ForcePointFiltering);
                __instance._forceNoMipMaps.Value = Config.GetValue(ForceNoMipMaps);
                __instance._forceUncompressed.Value = Config.GetValue(ForceUncompressed);
                __instance._importImageAlignment.Value = Config.GetValue(ImportImageAlignment);
                __instance._importAtOrigin.Value = Config.GetValue(ImportAtOrigin);
                __instance._assetsOnObject.Value = Config.GetValue(AssetsOnObject);
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("DefaultPreset")]
        public class DefaultPreset_Patch
        {
            static bool Prefix(ModelImportDialog __instance)
            {
                if (!Config.GetValue(Enabled))
                {
                    return true;
                }

                ImportDefaultsChange.Defaults.setDefaults(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("Preset_3DScan")]
        public class Preset_3DScan_Patch
        {
            static void Postfix(ModelImportDialog __instance)
            {
                if (Config.GetValue(Enabled))
                {
                    __instance._asPointCloud.Value = Config.GetValue(Scan_AsPointCloud);
                    __instance._importImagesByName.Value = Config.GetValue(Scan_ImportImagesByName);
                    __instance._material.Value = Config.GetValue(Scan_Material);
                    __instance._scale.Value = Config.GetValue(Scan_Scale);
                    __instance._autoScale.Value = Config.GetValue(Scan_AutoScale);
                    __instance._colliders.Value = Config.GetValue(Scan_Colliders);
                    __instance._animations.Value = Config.GetValue(Scan_Animations);
                    __instance._rig.Value = Config.GetValue(Scan_Rig);
                    __instance._setupIK.Value = Config.GetValue(Scan_SetupIk);
                    __instance._debugRig.Value = Config.GetValue(Scan_DebugRig);
                    __instance._timelapse.Value = Config.GetValue(Scan_Timelapse);
                    __instance._externalTextures.Value = Config.GetValue(Scan_ExternalTextures);
                    __instance._importAlbedoColor.Value = Config.GetValue(Scan_ImportAlbedoColour);
                }
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("Preset_CADModel")]
        public class Preset_CADModel_Patch
        {
            static void Postfix(ModelImportDialog __instance)
            {
                if (Config.GetValue(Enabled))
                {
                    __instance._scalable.Value = Config.GetValue(CAD_Scalable);
                    __instance._asPointCloud.Value = Config.GetValue(CAD_AsPointCloud);
                    __instance._importImagesByName.Value = Config.GetValue(CAD_ImportImagesByName);
                    __instance._material.Value = Config.GetValue(CAD_Material);
                    __instance._scale.Value = Config.GetValue(CAD_Scale);
                    __instance._autoScale.Value = Config.GetValue(CAD_AutoScale);
                    __instance._colliders.Value = Config.GetValue(CAD_Colliders);
                    __instance._animations.Value = Config.GetValue(CAD_Animations);
                    __instance._rig.Value = Config.GetValue(CAD_Rig);
                    __instance._setupIK.Value = Config.GetValue(CAD_SetupIk);
                    __instance._debugRig.Value = Config.GetValue(CAD_DebugRig);
                    __instance._timelapse.Value = Config.GetValue(CAD_Timelapse);
                    __instance._externalTextures.Value = Config.GetValue(CAD_ExternalTextures);
                }
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("Preset_PointCloud")]
        public class Preset_PointCloud_Patch
        {
            static void Postfix(ModelImportDialog __instance)
            {
                if (Config.GetValue(Enabled))
                {
                    __instance._asPointCloud.Value = Config.GetValue(PC_AsPointCloud);
                    __instance._material.Value = Config.GetValue(PC_Material);
                    __instance._scale.Value = Config.GetValue(PC_Scale);
                    __instance._autoScale.Value = Config.GetValue(PC_AutoScale);
                    __instance._colliders.Value = Config.GetValue(PC_Colliders);
                    __instance._animations.Value = Config.GetValue(PC_Animations);
                    __instance._rig.Value = Config.GetValue(PC_Rig);
                    __instance._setupIK.Value = Config.GetValue(PC_SetupIK);
                    __instance._debugRig.Value = Config.GetValue(PC_DebugRig);
                    __instance._timelapse.Value = Config.GetValue(PC_Timelapse);
                    __instance._externalTextures.Value = Config.GetValue(PC_ExternalTextures);
                    __instance._importVertexColors.Value = Config.GetValue(PC_ImportVertexColours);
                    __instance._importAlbedoColor.Value = Config.GetValue(PC_ImportAlbedoColor);
                }
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("Preset_Separable3DModel")]
        public class Preset_Separable3DModel_Patch
        {
            static void Postfix(ModelImportDialog __instance)
            {
                if (Config.GetValue(Enabled))
                {
                    __instance._snappable.Value = Config.GetValue(Separable_Snappable);
                }
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("Preset_Regular3DModel")]
        public class Preset_Regular3DModel_Patch
        {
            static void Postfix(ModelImportDialog __instance)
            {
                if (Config.GetValue(Enabled))
                {
                    __instance._snappable.Value = Config.GetValue(Snappable);
                }
            }
        }

        [HarmonyPatch(typeof(ModelImportDialog))]
        [HarmonyPatch("OpenCustom")]
        public class OpenCustom_Patch
        {
            static bool Prefix(ModelImportDialog __instance)
            {
                if (!Config.GetValue(Enabled))
                {
                    return true;
                }

                ImportDefaultsChange.Defaults.setDefaults(__instance);

                return true;
            }
        }
    }
}