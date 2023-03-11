using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class RemoveMobileWarning : MonoBehaviour
{
   		[PostProcessBuild]
		public static void OnPostProcessBuild(BuildTarget target, string targetPath)
		{
			if (target != BuildTarget.WebGL)
			{ 
				return;
			}

			// warning msg
			DisableWarningMsg(targetPath);
		}

		// disable WebGl warning msg on start
		private static void DisableWarningMsg(string targetPath)
		{
			// warning for mobile 
			ChangeText(targetPath, "*.html", "unityShowBanner('WebGL builds are not supported on mobile devices.')",
				"// unityShowBanner('WebGL builds are not supported on mobile devices.')",
				"Removing mobile warning from ");
		}
		
		// change file text
		private static void ChangeText(string _targetPath, string _file, string _oldText, string _newText, string _log)
		{
			var info = new DirectoryInfo(_targetPath);
			var files = info.GetFiles(_file);
			// warning for mobile 
			for (var i = 0; i < files.Length; i++)
			{
				var file = files[i];
				var filePath = file.FullName;
				var text = File.ReadAllText(filePath);
				text = text.Replace(_oldText, _newText);
				Debug.Log(_log + filePath);
				File.WriteAllText(filePath, text);
			}
		}
}
