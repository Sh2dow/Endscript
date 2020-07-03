﻿using System;



namespace Endscript.Exceptions
{
	public sealed class InvalidVersionException : Exception
	{
		public InvalidVersionException() : base() { }

		public InvalidVersionException(int version)
			: base($"Endscript version was of invalid format, expected [VERSN{version}]") { }
	}
}
