/*
	KCToolkit:AssetBundleLoader
	
	A simple asset bundle loader designed to make loading assets exported
	via the Unity Editor's bundle tagging system easy to use while still
	allowing bundle dependancies to be loaded if they exist.
	
	Created by Michael Peddicord
	12/15/2019
*/

using UnityEngine;

namespace KCToolkit{

	public class AssetBundleLoader
	{
		//Return: AssetBundle if a bundle containing [bundlename] was found. Else null.
		public static AssetBundle LoadAssetBundle(string modPath, string bundlename)
		{
			var assetBundleName = "win64";
			if (Application.platform == RuntimePlatform.LinuxPlayer)
			{
				assetBundleName = "linux";
			}
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				assetBundleName = "osx";
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer)
			{
				if (SystemInfo.operatingSystem.Contains("64"))
				{
					assetBundleName = "win64";
				}
				else
				{
					assetBundleName = "win32";
				}
			}
			
			var assetBundlePath = modPath + "/" + assetBundleName + "/";
			
			var manifestAssetBundle = AssetBundle.LoadFromFile(assetBundlePath + "/" + assetBundleName);
			AssetBundleManifest manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			
			foreach(string bundle in manifest.GetAllAssetBundles()){
				//Looking for a bundle that matches given name
				if(bundle.Contains(bundlename)){
					foreach(string dependancy in manifest.GetAllDependencies(bundle)){
						AssetBundle.LoadFromFile(assetBundlePath + "/" + dependancy);
					}

					manifestAssetBundle.Unload(false);
					return AssetBundle.LoadFromFile(assetBundlePath + "/" + bundle);
				}
			}

			manifestAssetBundle.Unload(false);

			return null;
		}
	}
}
