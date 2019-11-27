using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IDeliveryProcessor
    {
        //void ProcessOrder(Cart cart, ShippingDatails shippingDatails, OrdersSummary os, string subject, string header, string[] contentManagersEmails);

        void EmailRecovery(User user, string host);

        void EmailActivation(User user, string host);

        void FeedBackRequest(Message message);

        //void FeedBackRequestForContentManagers(FeedBack feedBack, string[] emails);

        //void MassMailingDelivery(string subject, string body, IEnumerable<string> emails);
    }
}
