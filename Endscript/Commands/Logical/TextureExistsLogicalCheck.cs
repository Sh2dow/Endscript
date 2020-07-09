﻿using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands.Logical
{
	public class TextureExistsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private uint _texture;

		public eLogicType LogicType => eLogicType.texture_exists;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'texture_exists'

			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[2];
			this._manager = splits[3];
			this._tpk = splits[4];

			if (splits[5].IsHexString()) this._texture = Convert.ToUInt32(splits[5], 16);
			else throw new Exception($"Value {splits[5]} cannot be converted to a hexadecimal key");
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var collection = map.GetCollection(this._filename, this._manager, this._tpk);
				var tpk = collection as TPKBlock;
				return !(tpk.FindTexture(this._texture, eKeyType.BINKEY) is null);

			}
			catch { return false; }
		}
	}
}