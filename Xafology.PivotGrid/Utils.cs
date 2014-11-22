using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPivotGrid = DevExpress.Web.ASPxPivotGrid;
using AutoMapper;

namespace Xafology.PivotGrid
{
    public class Utils
    {
        public static PivotGridField MapPivotGridFieldToWin(PivotGridFieldBase source)
        {
            Mapper.CreateMap<PivotGridFieldBase, PivotGridField>()
                .ForMember(p => p.Name, m => m.MapFrom(p => p.Name));

            return Mapper.Map<PivotGridFieldBase, PivotGridField>(source);
        }
        public static WebPivotGrid.PivotGridField MapPivotGridFieldToWeb(PivotGridFieldBase source)
        {
            Mapper.CreateMap<PivotGridFieldBase, WebPivotGrid.PivotGridField>()
                .ForMember(p => p.Name, m => m.MapFrom(p => p.Name));

            return Mapper.Map<PivotGridFieldBase, WebPivotGrid.PivotGridField>(source);
        }

    }
}
