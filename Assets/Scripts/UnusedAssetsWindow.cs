using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FindUnusedAssets : MonoBehaviour
{
    static ListRequest Request;

    [MenuItem("Tools/Find Unused Assets")]
    static void Find()
    {
        Request = Client.List();
        EditorApplication.update += Progress;
    }

    static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                var allAssets = Directory.GetFiles("Assets", "*", SearchOption.AllDirectories);
                var allAssetPaths = new HashSet<string>(allAssets.Select(x => x.Replace("\\", "/")));

                foreach (var package in Request.Result)
                {
                    var packageAssets = Directory.GetFiles(package.assetPath, "*", SearchOption.AllDirectories);
                    foreach (var packageAsset in packageAssets)
                    {
                        allAssetPaths.Remove(packageAsset.Replace("\\", "/"));
                    }
                }

                var dependencies = new HashSet<string>();

                foreach (var asset in allAssetPaths)
                {
                    var assetDependencies = AssetDatabase.GetDependencies(asset);
                    dependencies.UnionWith(assetDependencies);
                }

                var unusedAssets = allAssetPaths.Except(dependencies).ToArray();

                foreach (var unusedAsset in unusedAssets)
                {
                    Debug.Log("Unused Asset: " + unusedAsset);
                }
            }
            else if (Request.Status >= StatusCode.Failure)
            {
                Debug.Log("Error getting package list: " + Request.Error.message);
            }

            EditorApplication.update -= Progress;
        }
    }
}