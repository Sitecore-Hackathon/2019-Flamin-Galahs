using Sitecore.Data.Items;
using Sitecore.Modules.EmailCampaign.Core;
using Sitecore.Modules.EmailCampaign.Core.HostnameMapping;
using Sitecore.Modules.EmailCampaign.Factories;
using Sitecore.Modules.EmailCampaign.Messages;
using Sitecore.Modules.EmailCampaign.Services;

namespace FlaminGalahs.Foundation.Mail.Extensions
{
    public class TypeResolver : Sitecore.Modules.EmailCampaign.Core.TypeResolver
    {
        private IMessageItemSourceFactory _messageItemSourceFactory;
        private IManagerRootService _managerRootService;

        public TypeResolver(IHostnameMappingService hostnameMappingService, 
            IMessageItemSourceFactory messageItemSourceFactory, 
            IManagerRootService managerRootService, 
            IMultiVariateTestStrategyFactory multiVariateTestStrategyFactory, 
            IAbnTestService abnTestService, 
            PipelineHelper pipelineHelper) :base(hostnameMappingService,messageItemSourceFactory,managerRootService,multiVariateTestStrategyFactory,abnTestService,pipelineHelper)
        {
            _messageItemSourceFactory = messageItemSourceFactory;
            _managerRootService = managerRootService;
        }

        public override MessageItem GetCorrectMessageObject(Item item)
        {
            if (SMSMessageType.IsCorrectMessageItem(item))
            {
                var obj = SMSMessageType.FromItemEx(item, _messageItemSourceFactory, _managerRootService);
                return SMSMessageType.FromItemEx(item, _messageItemSourceFactory, _managerRootService);
            }
            return base.GetCorrectMessageObject(item);
        }
    }
}
