﻿using System;
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
using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for TestSelectionPage.xaml
    /// </summary>
    public partial class TestSelectionPage : BasePage
    {
        public TestSelectionPage()
        {
            InitializeComponent();
            ViewModel = new TestSelectionViewModel();
        }
    }
}
