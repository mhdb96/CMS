using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface IDepartmentRequester
    {
        void DepartmentComplete(DepartmentModel model);
        void DepartmentUpdateComplete(DepartmentModel model);
    }
}
