using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSUI.Requesters
{
    public interface IAdminPanelRequester
    {
        AdminModel GetAdminInfo();
    }
}
