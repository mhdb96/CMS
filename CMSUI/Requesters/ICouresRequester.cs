using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface ICouresRequester
    {
        void CourseComplete(CourseModel model);
        void CourseUpdateComplete(CourseModel model);
    }
}
