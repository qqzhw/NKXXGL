﻿using ICIMS.Model.BusinessManages;
using ICIMS.Modules.BusinessManages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICIMS.Modules.BusinessManages.Views
{
    /// <summary>
    /// ItemDefineEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ItemDefineEditView : UserControl
    {
        public ItemDefineEditView(ItemDefineEditViewModel viewModel,ItemDefineList data)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.View = this;
        }

        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
