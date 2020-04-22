using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace CMSUI.Requesters
{
    public interface IParentWindow
    {
        Task<MessageDialogResult> ShowMessage(string title, string message, MessageDialogStyle style);
    }
}
