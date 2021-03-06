﻿using Discord;
using Discord.WebSocket;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Frequency2.Source;

namespace Frequency2.Types.Messages
{
	public class PaginationService<T> where T : BaseSocketClient
	{
		internal static void Paginate(ulong message, PageCollection pages)
		{
			Instance._messages.Add(message, pages);
		}
		private readonly T _client;
		private readonly Dictionary<ulong, PageCollection> _messages;

		public static IReadOnlyList<Emoji> Emojis = new List<Emoji>()
		{
			new Emoji("⏪"),
			new Emoji("\u25C0"),
			new Emoji("\u25B6"),
			new Emoji("\u23F9")
		};
		public static bool ContainsEmoji(string name)
		=> Emojis.Where(x => x.Name == name).FirstOrDefault() != null;

		public static PaginationService<T> Instance { get; private set; }

		public PaginationService(T client)
		{
			if (Instance != null)
				throw new InvalidOperationException("Cannot create another instance of a singleton");
			Instance = this;
			_messages = new Dictionary<ulong, PageCollection>();
			_client = client;
			_client.ReactionAdded += _client_ReactionAdded;
		}

		private async Task _client_ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			if (!_messages.ContainsKey(message.Id))
				return;
			if (!ContainsEmoji(reaction.Emote.Name))
				return;
			if (_client.CurrentUser.Id != message.Value.Author.Id)
				return;
			if (reaction.UserId == _client.CurrentUser.Id)
				return;

			var collection = _messages[message.Id];
			string emote = reaction.Emote.Name;

			if(Frequency2Client.Instance._commands._userTimeouts.AddOrUpdate(reaction.User.Value.Id, 1, (ulong id, int i) => { return i + 1; }) == 5)
			{
				Frequency2Client.Instance._commands._userTimeouts[reaction.User.Value.Id]--;
				return;
			}

			var cache = await message.GetOrDownloadAsync();

			if (emote == Emojis[0].Name)
			{
				await collection.PageAtAsync(0);
			}
			else if(emote == Emojis[1].Name)
			{
				int page = collection.CurrentPage - 1;
				if (collection.CurrentPage == 0)
					page = collection.Count - 1;
				await collection.PageAtAsync(page);

			}
			else if (emote == Emojis[2].Name)
			{
				int page = collection.CurrentPage + 1;
				if (collection.CurrentPage == collection.Count - 1)
					page = 0;
				await collection.PageAtAsync(page);

			}
			else if (emote == Emojis[3].Name)
			{
				await cache.DeleteAsync();
				_messages.Remove(cache.Id);
			}


		}
	}
}
