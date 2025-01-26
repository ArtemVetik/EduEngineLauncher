namespace EduEngineLauncher
{
    internal static class ProjectTemplate
    {
        public readonly static string ProjectFile = 
            @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0-windows7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include=""AssetManager"">
      <HintPath>D:\Users\Artem\repos\EduEngine\x64\Release\AssetManager.dll</HintPath>
    </Reference>
    <Reference Include=""CoreInterop"">
      <HintPath>D:\Users\Artem\repos\EduEngine\x64\Release\CoreInterop.dll</HintPath>
    </Reference>
    <Reference Include=""GameplayFramework"">
      <HintPath>D:\Users\Artem\repos\EduEngine\x64\Release\GameplayFramework.dll</HintPath>
    </Reference>
    <Reference Include=""ImGui.NET"">   
      <HintPath>D:\Users\Artem\repos\EduEngine\x64\Release\ImGui.NET.dll</HintPath>
    </Reference>
  </ItemGroup>

</Project>
";

        public static string ScriptTemplate(string projectName, string fileName) => @$"using EduEngine;

namespace {projectName}
{{
    public class {fileName} : Component
    {{
        public {fileName}(GameObject parent) : base(parent)
        {{ }}

        public override void OnGameStart()
        {{

        }}

        public override void Update()
        {{

        }}
    }}
}}
";
    }
}
