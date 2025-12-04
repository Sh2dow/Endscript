using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Endscript.Profiles;
using Nikki.Core;
using System;



namespace Endscript.Commands
{
    /// <summary>
    /// Command of type 'move_collection_last [filename] [manager] [collection]'.
    /// </summary>
    public class MoveCollectionLastCommand : BaseCommand, ISingleParsable
    {
        private string _filename;
        private string _manager;
        private string _collection;

        public override eCommandType Type => eCommandType.move_collection_last;

        public override void Prepare(string[] splits)
        {
            if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

            this._filename = splits[1].ToUpperInvariant();
            this._manager = splits[2];
            this._collection = splits[3];
        }

        public override void Execute(CollectionMap map)
        {
            var sdb = map.Profile[this._filename];

            if (sdb is null)
            {

                throw new LookupFailException($"File {this._filename} was never loaded");

            }

            var manager = sdb.Database.GetManager(this._manager);

            if (manager is null)
            {

                throw new LookupFailException($"Manager named {this._manager} does not exist");

            }

            var index1 = manager.FindIndex(this._collection);

            if (index1 < 0)
            {

                throw new Exception($"Collection named {this._collection} does not exist in {this._filename}, {this._manager}.");

            }

            for (int index2 = index1 + 1; index2 < manager.Count; index1++, index2++) manager.Switch(index1, index2);
        }

        public void SingleExecution(BaseProfile profile)
        {
            var sdb = profile[this._filename];

            if (sdb is null)
            {

                throw new LookupFailException($"File {this._filename} was never loaded");

            }

            var manager = sdb.Database.GetManager(this._manager);

            if (manager is null)
            {

                throw new LookupFailException($"Manager named {this._manager} does not exist");

            }

            var index1 = manager.FindIndex(this._collection);

            if (index1 < 0)
            {

                throw new Exception($"Collection named {this._collection} does not exist in {this._filename}, {this._manager}.");

            }

            for (int index2 = index1 + 1; index2 < manager.Count; index1++, index2++) manager.Switch(index1, index2);
        }
    }
}
