// <copyright file="MessageReplyViewModel.cs" company="Kleida Haxhaj">
// Copyright (c) Kleida Haxhaj. All rights reserved.
// </copyright>

namespace Co_nnecto.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Co_nnecto.Models;

    public class MessageReplyViewModel
    {
        public Reply Reply { get; set; }

        public Message Message { get; set; }

        public List<MessageReply> Replies { get; set; } = new List<MessageReply>();

        public PagedList.IPagedList<Message> Messages { get; set; }

        public class MessageReply
        {
            public int Id { get; set; }

            public int MessageId { get; set; }

            public string MessageDetails { get; set; }

            public string ReplyFrom { get; set; }

            public string ReplyMessage { get; set; }

            public DateTime ReplyDateTime { get; set; }
        }
    }
}