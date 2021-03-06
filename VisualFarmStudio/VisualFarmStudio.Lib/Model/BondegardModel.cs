﻿using System.Collections.Generic;
using System.Linq;
using VisualFarmStudio.Common.ExtensionMethods;
using VisualFarmStudio.Core.Domain;

namespace VisualFarmStudio.Lib.Model
{
    public class BondegardModel : BaseModel<Bondegard>
    {
        public BondeModel Bonde { get; set; }
        public string Navn { get; set; }
        public IList<FjosModel> Fjoser { get; set; }
        public IList<StallModel> Staller { get; set; }
        public IList<TraktorModel> Traktorer { get; set; }

        public BondegardModel()
        {
            Fjoser = new List<FjosModel>();
            Staller = new List<StallModel>();
            Traktorer = new List<TraktorModel>();
        }

        public BondegardModel(Bondegard bondegard) : base(bondegard)
        {
            Bonde = new BondeModel(bondegard.Bonde);
            Navn = bondegard.Navn;
            Fjoser = bondegard.Fjoses.Select(fjos => new FjosModel(fjos)).ToList();
            Staller = bondegard.Stalls.Select(stall => new StallModel(stall)).ToList();
            Traktorer = bondegard.Traktors.Select(traktor => new TraktorModel(traktor)).ToList();
        }

        protected override Bondegard MapTo(Bondegard bondegard)
        {
            bondegard.Bonde = Bonde.ToEntity();
            bondegard.Navn = Navn;
            Staller.Each(s => bondegard.AddStall(s.ToEntity()));
            Traktorer.Each(t => bondegard.AddTraktor(t.ToEntity()));
            Fjoser.Each(f => bondegard.AddFjos(f.ToEntity()));
            return bondegard;
        }
    }
}