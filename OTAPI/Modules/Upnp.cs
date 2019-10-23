﻿using System;
using System.Linq;
using System.Reflection;
using Mod.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace OTAPI.Modules
{
	[Module("Adding windows checks to Upnp", "death")]
	public class Upnp : RunnableModule
	{
		private ModFramework _framework;

		public Upnp(ModFramework framework)
		{
			_framework = framework;
		}

		public override void Run()
		{
			// this adds "if(Platform.IsWindows)" around the upnp code.
			// the osx build omits the additional code so it seems ok to do this.
			var terraria = this.Assemblies
				.Single(x => x.Name.Name.IndexOf("Terraria", StringComparison.CurrentCultureIgnoreCase) > -1);
			var relogic = this.Assemblies
				.Single(x => x.Name.Name.IndexOf("ReLogic", StringComparison.CurrentCultureIgnoreCase) > -1);

			var il = terraria.Type("Terraria.Netplay").Method("OpenPort").Body.GetILProcessor();

			var target = il.Body.Instructions.First(x => (x.Operand is FieldReference) && (x.Operand as FieldReference).Name == "portForwardPort");

			var p_isWindows = relogic.Type("ReLogic.OS.Platform").Property("IsWindows");
			il.InsertAfter(
				target,
				new { OpCodes.Call, Operand = il.Body.Method.Module.ImportReference(p_isWindows.GetMethod) },
				new { OpCodes.Brtrue_S, target.Next },
				new { OpCodes.Ret }
			);
		}
	}
}