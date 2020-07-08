using System;
using System.Collections.Generic;
using System.IO;

namespace QuestMaker.Code.Attributes {
	public class FileAttribute : Attribute {
		public string directoryPath;
		public string filename;
		public string fullPath;

		/// <summary>
		/// Gets used when generating the files
		/// </summary>
		public FileAttribute(string path, string extension) {
			string[] splittedPath = path.Split('/');
			List<string> splittedDirectoryPath = new List<string> { Project.Path };

			for (int i = 0;i < splittedPath.Length - 1;i++) {
				splittedDirectoryPath.Add(splittedPath[i]);
				directoryPath = Path.Combine(splittedDirectoryPath.ToArray());
				Directory.CreateDirectory(directoryPath);
			}

			this.directoryPath = Path.Combine(splittedDirectoryPath.ToArray());
			this.filename = splittedPath[splittedPath.Length - 1] + "." + extension;
			this.fullPath = Path.Combine(directoryPath, filename);
		}
	}
}