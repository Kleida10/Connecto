// <copyright file="ChatController.cs" company="Kleida Haxhaj">
// Copyright (c) Kleida Haxhaj. All rights reserved.
// </copyright>

namespace Co_nnecto.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Mvc;
    using Co_nnecto.Models;
    using Co_nnecto.ViewModels;
    using PagedList;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using SendGrid.SmtpApi;

    public class ChatController : ApplicationBaseController
    {
        ApplicationDbContext context;

        /// Initializes a new instance of the <see cref="ChatController"/> class.
        public ChatController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index(int? id, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = context.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = context.Messages
                                       .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (id != null)
            {
                var replies = context.Replies.Where(x => x.MessageId == id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = context.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }
                }
                else
                {
                    vm.Replies.Add(null);
                }

                ViewBag.MessageId = id.Value;
            }

            return View(vm);
        }

        public ActionResult Create()
        {
            MessageReplyViewModel vm = new MessageReplyViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult PostMessage(MessageReplyViewModel vm)
        {
            var username = User.Identity.Name;
            string fullName = string.Empty;
            int msgid = 0;
            if (!string.IsNullOrEmpty(username))
            {
                var user = context.Users.SingleOrDefault(u => u.UserName == username);
                fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
            }

            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = fullName;

                context.Messages.Add(messagetoPost);
                context.SaveChanges();
                msgid = messagetoPost.Id;
            }

            return RedirectToAction("Index", "Chat", new { Id = msgid });
        }

        [HttpPost]
        public ActionResult ReplyMessage(MessageReplyViewModel vm, int messageId)
        {
            var username = this.User.Identity.Name;
            string fullName = string.Empty;
            if (!string.IsNullOrEmpty(username))
            {
                var user = context.Users.SingleOrDefault(u => u.UserName == username);
                fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
            }

            if (vm.Reply.ReplyMessage != null)
            {
                Reply reply = new Reply();
                reply.ReplyDateTime = DateTime.Now;
                reply.MessageId = messageId;
                reply.ReplyFrom = fullName;
                reply.ReplyMessage = vm.Reply.ReplyMessage;
                context.Replies.Add(reply);
                context.SaveChanges();
            }

            var messageOwner = context.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            var users = from user in context.Users
                        orderby user.FirstName
                        select new
                        {
                            FullName = user.FirstName + " " + user.LastName,
                            UserEmail = user.Email,
                        };

            var uEmail = users.Where(x => x.FullName == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
            SendGridMessage replyMessage = new SendGridMessage();
            replyMessage.From = new EmailAddress(username);
            replyMessage.Subject = "Reply for your message :" + context.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault();
            replyMessage.PlainTextContent = vm.Reply.ReplyMessage;
            replyMessage.AddTo(uEmail);
            var apiKey = "SG.Q6xPa - GrS - 6P9ksaGM0G7Q.WPjkVCm40fCVMmV7MLNdrcE_EqlAx0_fgezOBTksSp4";

            var client = new SendGridClient(apiKey);
            client.SendEmailAsync(replyMessage);

            return RedirectToAction("Index", "Chat", new { Id = messageId });
        }

        [HttpPost]
        public ActionResult DeleteMessage(int messageId)
        {
            Message messageToDelete = context.Messages.Find(messageId);
            context.Messages.Remove(messageToDelete);
            context.SaveChanges();

            // also delete the replies related to the message
            var repliesToDelete = context.Replies.Where(i => i.MessageId == messageId).ToList();
            if (repliesToDelete != null)
            {
                foreach (var rep in repliesToDelete)
                {
                    context.Replies.Remove(rep);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Chat");
        }
    }
}