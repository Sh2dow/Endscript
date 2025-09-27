using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;



namespace Endscript.Commands
{
    /// <summary>
    /// Command of type 'move_folder [movetype] [pathfromtype] [pathtotype] [pathfrom] [pathto]'.
    /// </summary>
    public class MoveFolderCommand : BaseCommand
	{
		private eImportType _import;
		private ePathType _type_from;
		private ePathType _type_to;
		private string _path_from;
		private string _path_to;

		public override eCommandType Type => eCommandType.move_folder;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			if (!Enum.TryParse(splits[1], out this._import))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._type_from = EnumConverter.StringToPathType(splits[2]);
			if (this._type_from == ePathType.Invalid) throw new Exception($"Path type {splits[2]} is an invalid type");

			this._type_to = EnumConverter.StringToPathType(splits[3]);
			if (this._type_to == ePathType.Invalid) throw new Exception($"Path type {splits[3]} is an invalid type");

			this._path_from = splits[4];
			this._path_to = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			this._path_from = this._type_from == ePathType.Relative
				? Path.Combine(map.Directory, this._path_from)
				: Path.Combine(map.Profile.Directory, this._path_from);

			this._path_to = this._type_to == ePathType.Relative
				? Path.Combine(map.Directory, this._path_to)
				: Path.Combine(map.Profile.Directory, this._path_to);

			if (!Directory.Exists(this._path_from))
			{

				throw new DirectoryNotFoundException($"Directory with path {this._path_from} does not exist");

			}

            CopyDirectory(this._path_from, this._path_to, true);
        }

		public void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);

				if (this._import == eImportType.negate && File.Exists(targetFilePath)) continue; // Skip existing files if negate

                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
