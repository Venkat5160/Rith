using SDAE.Data.Model;
using System.Threading.Tasks;

namespace SDAE.Common.Notification
{
    public interface INotification
    {
        Task IOSNotification(PushNotificationDto loginModel);
    }
}
