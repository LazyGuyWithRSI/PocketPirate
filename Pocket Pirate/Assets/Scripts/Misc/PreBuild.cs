#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class PreBuild : MonoBehaviour//, IPreprocessBuildWithReport
{
    public StringReference VersionReference;

    //public int callbackOrder { get { return 0; } }

    //public void OnPreprocessBuild(BuildReport report)
    //{
        //VersionReference.Value = Application.version;
        //Debug.Log("Setting version ref to " + VersionReference.Value);
    //}

    void Awake()
    {
        VersionReference.Value = Application.version;
        Debug.Log("Setting version ref to " + VersionReference.Value);
    }
}
#endif