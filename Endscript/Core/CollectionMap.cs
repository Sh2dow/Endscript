﻿using System;
using System.Collections.Generic;

using Endscript.Profiles;
using Endscript.Exceptions;

using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;



namespace Endscript.Core
{
	public sealed class CollectionMap
	{
		private const string delim = "|";
		private readonly Dictionary<string, Collectable> _map;
		private readonly BaseProfile _profile;

		public CollectionMap(BaseProfile profile)
		{
			this._profile = profile;
			this._map = new Dictionary<string, Collectable>(this.FastEstimateCapacity());
			this.LoadMapFromProfile();
		}

		private int FastEstimateCapacity()
		{
			int result = 10;
			foreach (var sdb in this._profile)
			{

				foreach (var manager in sdb.Database.Managers)
				{

					result += manager.Count;

				}

			}

			return result;
		}

		private void LoadMapFromProfile()
		{
			foreach (var sdb in this._profile)
			{

				foreach (var manager in sdb.Database.Managers)
				{

					foreach (Collectable collection in manager)
					{

						var path = sdb.Filename + delim + manager.Name + delim + collection.CollectionName;
						this._map.Add(path, collection);

					}

				}

			}
		}

		public SynchronizedDatabase GetSynchronizedDatabase(string filename, bool errorthrow)
		{
			var result = this._profile[filename];

			if (result is null && errorthrow)
			{

				throw new LookupFailException($"File {filename} was never loaded");

			}

			return result;
		}

		public IManager GetManager(string filename, string manager, bool errorthrow)
		{
			var sdb = this.GetSynchronizedDatabase(filename, errorthrow);
			if (sdb is null) return null;

			var result = sdb.Database.GetManager(manager);

			if (result is null && errorthrow)
			{

				throw new LookupFailException($"Manager named {manager} does not exist");

			}

			return result;
		}
	
		public Collectable GetCollection(string filename, string manager, string cname, bool errorthrow)
		{
			var path = filename + delim + manager + delim + cname;
			if (this._map.TryGetValue(path, out var result)) return result;
			else if (errorthrow) throw new LookupFailException($"Collection named {cname} does not exist");
			else return null;
		}
	
		public bool ContainsCollection(string filename, string manager, string cname)
		{
			var path = filename + delim + manager + delim + cname;
			return this._map.ContainsKey(path);
		}
	}
}