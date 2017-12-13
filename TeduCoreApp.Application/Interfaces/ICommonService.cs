using System;
using System.Collections.Generic;
using System.Text;
using TeduCoreApp.Application.ViewModels.Common;

namespace TeduCoreApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
