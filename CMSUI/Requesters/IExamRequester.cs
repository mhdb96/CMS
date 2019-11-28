using CMSLibrary.Models;


namespace CMSUI.Requesters
{
    public interface IExamRequester
    {
        void ExamComplete(ExamModel model);
        UserModel GetUserInfo();
        AssignmentModel GetAssignment();
    }
}
