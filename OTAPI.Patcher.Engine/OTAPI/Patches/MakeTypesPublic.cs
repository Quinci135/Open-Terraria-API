﻿using NDesk.Options;
using OTAPI.Patcher.Engine.Extensions;
using OTAPI.Patcher.Engine.Modification;
using OTAPI.Patcher.Engine.Modifications.Helpers;
using System;

namespace OTAPI.Patcher.Engine.Modifications.Patches
{
	/// <summary>
	/// Makes all types public in the OTAPI assembly dll.
	/// </summary>
	public class MakeTypesPublic : ModificationBase
	{
		public override string Description => "Making all types public...";
		public override void Run(OptionSet options)
		{
			foreach (var type in SourceDefinition.MainModule.Types)
				type.MakePublic(false);
		}
	}
}