using System;
using System.Collections.Generic;
using Sitecore.Diagnostics;
using Sitecore.EDS.Core.Reporting;
using Sitecore.EmailCampaign.Core.Services;
using Sitecore.EmailCampaign.Model.XConnect.Facets;
using Sitecore.Framework.Conditions;
using Sitecore.Modules.EmailCampaign.Validators;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Threading.Tasks;
using Sitecore.Modules.EmailCampaign.Core.Dispatch;

namespace FlaminGalahs.Foundation.Mail.Extensions
{
    public class RecipientValidator : Sitecore.Modules.EmailCampaign.Core.Dispatch.RecipientValidator
    {
        private readonly RegexValidator _emailRegexValidator;
        private readonly ISuppressionManager _suppressionManager;
        private readonly IEmailPeriodService _emailPeriodService;
        private readonly ICurrentDateProvider _currentDateProvider;

        public RecipientValidator(
          RegexValidator emailRegexValidator,
          ISuppressionManager suppressionManager,
          IEmailPeriodService emailPeriodService,
          ICurrentDateProvider currentDateProvider) : base(emailRegexValidator, suppressionManager, emailPeriodService, currentDateProvider)
        {
            Assert.ArgumentNotNull((object)emailRegexValidator, nameof(emailRegexValidator));
            Assert.ArgumentNotNull((object)suppressionManager, nameof(suppressionManager));
            Assert.ArgumentNotNull((object)emailPeriodService, nameof(emailPeriodService));
            Assert.ArgumentNotNull((object)currentDateProvider, nameof(currentDateProvider));
            this._emailRegexValidator = emailRegexValidator;
            this._suppressionManager = suppressionManager;
            this._emailPeriodService = emailPeriodService;
            this._currentDateProvider = currentDateProvider;
        }

        public bool IsValidEmail(Contact contact)
        {
            string smtpAddress = contact.Emails()?.PreferredEmail?.SmtpAddress;
            if (string.IsNullOrWhiteSpace(smtpAddress))
                return false;
            return this._emailRegexValidator.IsValid(smtpAddress);
        }
    }
}