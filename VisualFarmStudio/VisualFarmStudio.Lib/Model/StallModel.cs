﻿using System.Collections.Generic;
using System.Linq;
using VisualFarmStudio.Core.Domain;
using VisualFarmStudio.Lib.ExtensionMethods;

namespace VisualFarmStudio.Lib.Model
{
    public class StallModel : BaseModel<Stall>
    {
        public IList<HestModel> Hester { get; set; }

        public StallModel(Stall stall) : base(stall)
        {
            Hester = stall.Hester.Select(hest => new HestModel(hest)).ToList();
        }

        protected override Stall MapTo(Stall stall)
        {
            Hester.Each(h => stall.AddHest(h.ToEntity()));
            return stall;
        }
    }
}