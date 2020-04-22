using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface IAssignmentRequester
    {
        void AssignmentComplete(AssignmentModel model);
    }
}
