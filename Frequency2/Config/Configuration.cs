﻿using Newtonsoft.Json;
using System.IO;
using System;

namespace Frequency2.Config
{
	public abstract class Configuration
	{
		public abstract class LavaLinkConfiguration
		{
			public abstract string Host { get; }
			public abstract int Port { get; }
			public abstract string Password { get; }

			internal class lavalinkconfig : LavaLinkConfiguration
			{
				[JsonIgnore]
				public override string Host => _Host;

				[JsonIgnore]
				public override int Port => _Port;

				[JsonIgnore]
				public override string Password => _Password;

				[JsonProperty("Host")]
				private readonly string _Host = "";

				[JsonProperty("Port")]
				private readonly int _Port = 0;

				[JsonProperty("Password")]
				private readonly string _Password = "";
 			}
		}
		public abstract ulong OwnerId { get; }
		public abstract string RichPresence { get; }
		public abstract string Token { get; }
		public abstract string ApexApiKey { get; }
		public abstract string FortniteApiKey { get; }
		public abstract string WolframApiKey { get; }

		public abstract LavaLinkConfiguration LavaLinkSettings { get; }

		private static Configuration config = null;

		public static Configuration Config
		{
			get
			{
				if(config == null)
				{
					config = GetConfig();
				}
				return config;
			}
		}

		private static Configuration GetConfig()
		{
			string a = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "/Configuration.json";

			return JsonConvert.DeserializeObject<_config>(File.ReadAllText(a));
		}
		public static Configuration ReloadConfig()
		{
			config = GetConfig();
			return config;
		}

		private class _config : Configuration
		{
			[JsonIgnore]
			public override ulong OwnerId { get => OwnerId_; }

			[JsonIgnore]
			public override string RichPresence { get => RichPresence_; }

			[JsonIgnore]
			public override string Token { get => Token_; }

			[JsonIgnore]
			public override string FortniteApiKey { get => FortniteApiKey_; }

			[JsonIgnore]
			public override string ApexApiKey { get => ApexApiKey_; }

			[JsonIgnore]
			public override string WolframApiKey { get => WolframApiKey_; }

			[JsonIgnore]
			public override LavaLinkConfiguration LavaLinkSettings { get => Lavalinkconfig_; }

			[JsonProperty("Token")]
			private readonly string Token_ = "";

			[JsonProperty("RichPresence")]
			private readonly string RichPresence_ = "";

			[JsonProperty("OwnerId")]
			private readonly ulong OwnerId_ = 0;

			[JsonProperty("FortniteApiKey")]
			private readonly string FortniteApiKey_ = "";

			[JsonProperty("WolframApiKey")]
			private readonly string WolframApiKey_ = "";

			[JsonProperty("ApexApiKey")]
			private readonly string ApexApiKey_ = "";

			[JsonProperty("LavaLinkSettings")]
			private readonly LavaLinkConfiguration.lavalinkconfig Lavalinkconfig_ = null;

		}

	}
}
