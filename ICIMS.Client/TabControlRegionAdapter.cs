using ICIMS.Metro.Controls;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Client
{
    class TabControlRegionAdapter : RegionAdapterBase<MetroAnimatedSingleRowTabControl>
    {
        private ITabItemView startupTab = null;

        public TabControlRegionAdapter(IRegionBehaviorFactory factory)
            : base(factory)
        {

        }

        protected override void Adapt(IRegion region, MetroAnimatedSingleRowTabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var items = regionTarget.Items;

                    foreach (ITabItemView tab in e.NewItems)
                    {
                        if (tab.TabItemIndex > items.Count)
                            items.Add(tab);
                        else
                            items.Insert(tab.TabItemIndex, tab);

                        if (tab.IsStartupTab)
                        {
                            if (tab != startupTab && startupTab != null)
                                throw new InvalidOperationException("More than one tab is the startup tab.");

                            startupTab = tab;

                            regionTarget.SelectedItem = tab;
                        }
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

    }
}
