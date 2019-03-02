using Sitecore.Modules.EmailCampaign.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Kernel;
using Sitecore.Data.Items;
using Sitecore.Modules.EmailCampaign.Core;
using Sitecore.Modules.EmailCampaign.Factories;
using Sitecore.Modules.EmailCampaign.Services;

//Enhanced from Sitecore 8 version from https://mortenengel.blogspot.com/2018/11/sending-sms-messages-with-sitecore-exm.html by Morten Engel

namespace FlaminGalahs.Foundation.Mail.Extensions
{
    public class SMSMessageType : TextMail
    {
        private readonly TextMailSource _curSource;
        public IMessageItemSourceFactory _messageItemSourceFactory;
        public IManagerRootService _managerRootService;

        protected SMSMessageType(Item item, IMessageItemSourceFactory messageItemSourceFactory, IManagerRootService managerRootService) 
                            : base(item, messageItemSourceFactory, managerRootService)
        {
            this._curSource = (base.Source as TextMailSource);
            this._messageItemSourceFactory = messageItemSourceFactory;
            this._managerRootService = managerRootService;
        }

        public new static bool IsCorrectMessageItem(Item item)
        {
            return ItemUtilExt.IsTemplateDescendant(item, "{EE205EB1-616C-40C2-BF0B-EA907E94F15F}");
        }

        public static SMSMessageType FromItemEx(Item item, IMessageItemSourceFactory messageItemSourceFactory, IManagerRootService managerRootService)
        {
            if (!SMSMessageType.IsCorrectMessageItem(item))
            {
                return null;
            }
            return new SMSMessageType(item, messageItemSourceFactory,managerRootService);
        }

        public override string GetMessageBody(bool preview)
        {
            return _curSource.Body;
        }

        public override object Clone()
        {
            SMSMessageType sms = new SMSMessageType(base.InnerItem, _messageItemSourceFactory, _managerRootService);
            CloneFields(sms);
            return sms;
        }
    }

}