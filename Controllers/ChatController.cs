using Co_nnecto.Models;
using Co_nnecto.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using SendGrid.SmtpApi;
using System.Net;
using System.Net.Mail;
using SendGrid;
using PagedList;
using SendGrid.Helpers.Mail;

namespace Co_nnecto.Controllers
{
    public class ChatController : ApplicationBaseController
    {
        ApplicationDbContext context;
        public ChatController()
        {
            context = new ApplicationDbContext();
        }
       
            public ActionResult Index(int? Id, int? page)
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

                if (Id != null)
                {

                    var replies = context.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
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


                    ViewBag.MessageId = Id.Value;
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
            string fullName = "";
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
            var username = User.Identity.Name;
            string fullName = "";
            if (!string.IsNullOrEmpty(username))
            {
                var user = context.Users.SingleOrDefault(u => u.UserName == username);
                fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
            }
            if (vm.Reply.ReplyMessage != null)
            {
                Reply _reply = new Reply();
                _reply.ReplyDateTime = DateTime.Now;
                _reply.MessageId = messageId;
                _reply.ReplyFrom = fullName;
                _reply.ReplyMessage = vm.Reply.ReplyMessage;
                context.Replies.Add(_reply);
                context.SaveChanges();
            }

            var messageOwner = context.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            var users = from user in context.Users
                        orderby user.FirstName
                        select new
                        {
                            FullName = user.FirstName + " " + user.LastName,
                            UserEmail = user.Email
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
            Message _messageToDelete = context.Messages.Find(messageId);
            context.Messages.Remove(_messageToDelete);
            context.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = context.Replies.Where(i => i.MessageId == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    context.Replies.Remove(rep);
                    context.SaveChanges();
                }
            }


            return RedirectToAction("Index", "Chat");
        }
    }
    }

