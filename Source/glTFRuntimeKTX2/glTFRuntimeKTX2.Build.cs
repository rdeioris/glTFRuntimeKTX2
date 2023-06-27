// Copyright Roberto De Ioris

using UnrealBuildTool;

public class glTFRuntimeKTX2 : ModuleRules
{
	public glTFRuntimeKTX2(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				// ... add other public dependencies that you statically link with here ...
			}
			);
			
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
				"glTFRuntime"
			}
			);
		
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);

        string ThirdPartyDirectory = System.IO.Path.Combine(ModuleDirectory, "..", "ThirdParty", "libktx2");
        string ThirdPartyDirectoryIncludePath = System.IO.Path.Combine(ThirdPartyDirectory, "include");

        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            string ThirdPartyDirectoryWin64 = System.IO.Path.Combine(ThirdPartyDirectory, "lib", "win_x64");
            PublicAdditionalLibraries.Add(System.IO.Path.Combine(ThirdPartyDirectoryWin64, "ktx.lib"));
            RuntimeDependencies.Add("$(BinaryOutputDir)/ktx.dll", System.IO.Path.Combine(ThirdPartyDirectoryWin64, "ktx.dll"));
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            string ThirdPartyDirectoryMacArm64 = System.IO.Path.Combine(ThirdPartyDirectory, "lib", "mac_arm64");
            PublicAdditionalLibraries.Add(System.IO.Path.Combine(ThirdPartyDirectoryMacArm64, "libktx.dylib"));
            RuntimeDependencies.Add("$(BinaryOutputDir)/libktx.dylib", System.IO.Path.Combine(ThirdPartyDirectoryMacArm64, "libktx.dylib"));
        }
        else if (Target.Platform == UnrealTargetPlatform.Linux)
        {
            string ThirdPartyDirectoryMacArm64 = System.IO.Path.Combine(ThirdPartyDirectory, "lib", "linux_x64");
            PublicAdditionalLibraries.Add(System.IO.Path.Combine(ThirdPartyDirectoryMacArm64, "libktx.so"));
            RuntimeDependencies.Add("$(BinaryOutputDir)/libktx.so.4", System.IO.Path.Combine(ThirdPartyDirectoryMacArm64, "libktx.so"));
        }

        PrivateIncludePaths.Add(ThirdPartyDirectoryIncludePath);
    }
}
