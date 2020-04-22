using CMSLibrary.Models;

namespace CMSUI.Requesters
{
    public interface IActiveTermRequester
    {
        void ActiveTermComplete(ActiveTermModel model);
    }
}
