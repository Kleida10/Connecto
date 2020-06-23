// <copyright file="Reply.cs" company="Kleida Haxhaj">
// Copyright (c) Kleida Haxhaj. All rights reserved.
// </copyright>

namespace Co_nnecto.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class Reply
    {
        [Key]
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string ReplyFrom { get; set; }

        [Required]
        public string ReplyMessage { get; set; }

        public DateTime ReplyDateTime { get; set; }
    }
}