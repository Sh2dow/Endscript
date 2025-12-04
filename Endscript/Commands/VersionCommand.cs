using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'version [#.#.#.#] (forkname)'.
	/// </summary>
	public class VersionCommand : BaseCommand
	{
		private System.Version _version;

		public override eCommandType Type => eCommandType.version;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2 && splits.Length != 3) throw new InvalidArgsNumberException(splits.Length, 2, 3);

			this.CheckValidVersion(splits[1]);

            // Totally copied from Binercover so we throw a better exception when version command has the fork name included
            if (splits.Length == 3 && splits[2].ToLower() != "binarius") throw new Exception($"Endscript version declares that this mod is made for {splits[2]}. Binarius cannot process endscripts made for other forks of Binary.");

            if (Version.Value.CompareTo(this._version) >= 0) return;
			else throw new Exception($"Endscript version {this._version} is higher than executable {Version.Value}");
		}

		public override void Execute(CollectionMap map)
		{
		}

		private void CheckValidVersion(string version)
		{
			try
			{

				this._version = new System.Version(version);

			}
			catch
			{

				throw new Exception($"Version stated is of invalid format");

			}
		}
	}
}
