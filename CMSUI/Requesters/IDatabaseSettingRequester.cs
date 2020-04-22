using System.Threading.Tasks;

namespace CMSUI.Requesters
{
    public interface IDatabaseSettingRequester
    {
        Task DatabaseSettingSaved();
    }
}
