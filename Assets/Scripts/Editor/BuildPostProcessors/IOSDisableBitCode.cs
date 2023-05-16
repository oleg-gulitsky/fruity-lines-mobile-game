using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Editor.BuildPostProcessors
{
  public static class IOSDisableBitCode {
    
    [PostProcessBuild]
    private static void OnPostProcessBuildIOSDisableBitCode(BuildTarget buildTarget, string pathToBuiltProject)
    {
      if (buildTarget != BuildTarget.iOS) return;

      string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

      PBXProject pbxProject = new();
      pbxProject.ReadFromFile(projPath);
        
      // Main
      string target = pbxProject.GetUnityMainTargetGuid();
      pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
      
      // Unity Tests
      target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
      pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

      // Unity Framework
      target = pbxProject.GetUnityFrameworkTargetGuid();
      pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
      
      pbxProject.WriteToFile(projPath);
    }
  }
}