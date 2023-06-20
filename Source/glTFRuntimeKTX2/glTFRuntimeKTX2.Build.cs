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

        string ThirdPartyDirectory = System.IO.Path.Combine(ModuleDirectory, "..", "ThirdParty");
        string ThirdPartyDirectoryIncludePath = ThirdPartyDirectory;

        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            string ThirdPartyDirectoryWin64 = System.IO.Path.Combine(ThirdPartyDirectory, "libktx2_win_x64");
            ThirdPartyDirectoryIncludePath = System.IO.Path.Combine(ThirdPartyDirectoryWin64, "include");
            PublicAdditionalLibraries.Add(System.IO.Path.Combine(ThirdPartyDirectoryWin64, "ktx.lib"));
            RuntimeDependencies.Add("$(BinaryOutputDir)/ktx.dll", System.IO.Path.Combine(ThirdPartyDirectoryWin64, "ktx.dll"));
        }

        PrivateIncludePaths.Add(ThirdPartyDirectoryIncludePath);
    }
}
