# Contribution Guide

## Project Setup

In the [`./FeralCompany`](./FeralCompany) directory, create a file named `FeralCompany.csproj.user`. This file is
used to create user-specific configurations to the project.

### Example

The following contents are what I use, and achieves the following:

1. Locally refers to dependencies.
2. After a project build, deploys the DLL to the specified [r2modmanPlus](https://github.com/ebkr/r2modmanPlus) profile.
3. Makes [`FeralCompany_Unity/Assets/Editor`](./FeralCompany_Unity/Assets/Editor) scripts available.

```xml
<Project>
    <PropertyGroup Label="Developer Properties">
        <GameRoot>C:/Program Files (x86)/Steam/steamapps/common/Lethal Company</GameRoot>
        <BepInEx>$(APPDATA)/r2modmanPlus-local/LethalCompany/profiles/FeralCompany/BepInEx</BepInEx>
        <BepInExCore>$(BepInEx)/core</BepInExCore>
        <BepInExPlugins>$(BepInEx)/plugins</BepInExPlugins>
        <UnityEditor>C:/Program Files/Unity/Hub/Editor/2022.3.9f1/Editor/Data/Managed</UnityEditor>
    </PropertyGroup>
    <ItemGroup Label="Game Dependency">
        <Reference Include="$(GameRoot)/Lethal Company_Data/Managed/Unity.*.dll"/>
        <Reference Include="$(GameRoot)/Lethal Company_Data/Managed/UnityEngine.*.dll"/>
        <Reference Include="$(GameRoot)/Lethal Company_Data/Managed/Assembly-*.dll"/>
        <Reference Include="$(GameRoot)/Lethal Company_Data/Managed/Newtonsoft.Json.dll"/>
    </ItemGroup>
    <ItemGroup Label="BepInEx Dependencies">
        <Reference Include="$(BepInExCore)/0Harmony.dll"/>
        <Reference Include="$(BepInExCore)/BepInEx.Harmony.dll"/>
        <Reference Include="$(BepInExCore)/BepInEx.dll"/>
    </ItemGroup>
    <ItemGroup Label="Plugin Dependencies">
        <Reference Include="$(BepInExPlugins)/Rune580-LethalCompany_InputUtils/LethalCompanyInputUtils/LethalCompanyInputUtils.dll"/>
    </ItemGroup>
    <Target Name="Deploy Development Environment" AfterTargets="PostBuildEvent">
        <ItemGroup Label="Targets">
            <Targets Include="$(TargetPath)"/>
            <Locales Include="$(RootDir)/assets/locales/*.json"/>
        </ItemGroup>
        <Copy SourceFiles="@(Targets)" DestinationFolder="$(BepInExPlugins)/Ferus-FeralCompany/FeralCompany"/>
        <Copy SourceFiles="@(Locales)" DestinationFolder="$(BepInExPlugins)/Ferus-FeralCompany/FeralCompany/locales"/>
    </Target>
</Project>
```

## Unity Setup

In the [`./FeralCompany_Unity`](./FeralCompany_Unity) directory, create a file named `user_settings.json`. Similarly to
the project setup, this file will be used to create user-specific configurations.

### Example

```json
{
  "assetBundlesDestination": "C:/Users/Ferus/AppData/Roaming/r2modmanPlus-local/LethalCompany/profiles/FeralCompany/BepInEx/plugins/Ferus-FeralCompany/FeralCompany"
}
```

### Deploying AssetBundles

When you're ready to test the project, from the Unity editor find the "FeralCompany" tab and select "Deploy AssetBundles".
This will compile and deploy them to the folder above.

## Pre-Commit

This project has a [`pre-commit`](https://pre-commit.com/) configuration used for some consistency within the project.

For a detailed guide on how to setup and use `pre-commit`, you can view their
[Quick start](https://pre-commit.com/#quick-start) guide.
