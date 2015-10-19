﻿using System;
using System.Linq;
using Noobot.Domain.MessagingPipeline.Response;

namespace Noobot.Domain.MessagingPipeline.Request
{
    public class IncomingMessage
    {
        /// <summary>
        /// The Slack UserId of whoever sent the message
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Username of whoever sent the mssage
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The channel used to send a DirectMessage back to the user who sent the message. 
        /// Note: this might be empty if the Bot hasn't talked to them privately before, but Noobot will join the DM automatically if required.
        /// </summary>
        public string UserChannel { get; set; }

        /// <summary>
        /// Contains the untainted raw Text that comes in from Slack. This hasn't been URL decoded
        /// </summary>
        public string RawText { get; set; }

        /// <summary>
        /// Contains the URL decoded text from the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Contains the text minus as Bot targetting text (e.g. @Noobot: {blah})
        /// </summary>
        public string TargettedText { get; set; }

        /// <summary>
        /// The 'channel' the message occured on. This might be a DirectMessage channel.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Detects if the bot's name is mentioned anywhere in the text
        /// </summary>
        public bool BotIsMentioned { get; set; }

        /// <summary>
        /// The Bot's Slack name - this is configurable in Slack
        /// </summary>
        public string BotName { get; set; }

        /// <summary>
        /// The Bot's UserId
        /// </summary>
        public string BotId { get; set; }

        public string FormatTextTargettedAtBot()
        {
            string formattedText = Text ?? string.Empty;

            string[] myNames =
            {
                BotName + ":",
                BotName,
                string.Format("<@{0}>:", BotId),
                string.Format("<@{0}>", BotId),
                string.Format("@{0}:", BotName),
                string.Format("@{0}", BotName),
            };

            string handle = myNames.FirstOrDefault(x => formattedText.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(handle))
            {
                formattedText = formattedText.Substring(handle.Length).Trim();
            }

            return formattedText;
        }
        
        /// <summary>
        /// Will generate a message to be sent the current channel the message arrived from
        /// </summary>
        public ResponseMessage ReplyToChannel(string text)
        {
            return ResponseMessage.ChannelMessage(Channel, text);
        }
        
        /// <summary>
        /// Will send a DirectMessage reply to the use who sent the message
        /// </summary>
        public ResponseMessage ReplyDirectlyToUser(string text)
        {
            return ResponseMessage.DirectUserMessage(UserChannel, UserId, text);
        }
    }
}