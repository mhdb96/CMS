using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface ITeacherPanelRequester
    {
        TeacherModel GetTeacherInfo();
        void TeacherPanelClosed();
    }
}
