var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

Task("BuildAssembler")
    .Does(() => {
        DotNetBuild("./Assembler/Assembler.sln", new DotNetBuildSettings()
        {
            Configuration = configuration
        });
    });

Task("BuildJackCompiler")
    .Does(() => {
        DotNetBuild("./JackCompiler/JackCompiler.sln", new DotNetBuildSettings()
        {
            Configuration = configuration
        });
    });

Task("BuildVMtranslator")
    .Does(() => {
        DotNetBuild("./VMtranslator/VMtranslator.sln", new DotNetBuildSettings()
        {
            Configuration = configuration
        });
    });


Task("Build")
    .IsDependentOn("BuildAssembler")
    .IsDependentOn("BuildJackCompiler")
    .IsDependentOn("BuildVMtranslator")
    .Does(() => {

    });


Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetTest("./Assembler/Assembler.sln", new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true,
        });
        DotNetTest("./JackCompiler/JackCompiler.sln", new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true,
        });
        DotNetTest("./VMtranslator/VMtranslator.sln", new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true,
        });
    });

RunTarget(target);