using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface IAdminPanelRequester
    {
        AdminModel GetAdminInfo();
        void AdminPanelClosed();
    }
}
