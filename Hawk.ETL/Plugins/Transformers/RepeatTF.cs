﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.WpfPropertyGrid.Attributes;
using Hawk.Core.Connectors;
using Hawk.Core.Utils;
using Hawk.Core.Utils.Plugins;

namespace Hawk.ETL.Plugins.Transformers
{
    public enum RepeatType
    {
        OneRepeat,
        ListRepeat,
    }
    [XFrmWork("重复当前值", "对当前行进行重复性生成")]
    public class RepeatTF : TransformerBase
    {
        public RepeatTF()
        {
            RepeatCount = "1";
        }

        [LocalizedDisplayName("重复模式")]
        public RepeatType RepeatType { get; set; }

        [LocalizedDisplayName("重复次数")]
        public string RepeatCount { get; set; }

        public override bool Init(IEnumerable<IFreeDocument> docus)
        {
            IsMultiYield = true;
            return base.Init(docus);
        }

        public override IEnumerable<IFreeDocument> TransformManyData(IEnumerable<IFreeDocument> datas)
        {
            switch (RepeatType)
            {
                    case RepeatType.ListRepeat:
                    var count = int.Parse(RepeatCount);
                    while (count>0)
                    {
                        foreach (var data in datas)
                        {
                            yield return data.Clone();
                        }
                        count--;
                    }
                    break;
                    case RepeatType.OneRepeat:
                    foreach (var data in datas)
                    {
                        var c = data.Query(RepeatCount);
                        var c2 = int.Parse(c);
                        while (c2 > 0)
                        {
                            yield return data;
                            c2--;
                        }
                    }

                    break;
            } 
        }
    }
}
