using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.EForm
{
    public interface IClsEFPortalType : IClsApplication
    {
        List<EFPortalType> ListPortalType { get; set; }

        string OnGridModifyPortalType(EFPortalType MModel, string Action);
        void OnIndexLoading();
        void OnIndexLoadingPortalType();
    }
}