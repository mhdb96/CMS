﻿#pragma checksum "..\..\..\UserControls\StudentDataUserControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8E1008868C34A52F8738067B8E952B53A3D7D30A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CMSLibrary.Evaluation;
using CMSUI.UserControls;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CMSUI.UserControls {
    
    
    /// <summary>
    /// StudentDataUserControl
    /// </summary>
    public partial class StudentDataUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock number;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox firstName;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox lastName;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox regNo;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deleteStudentData;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel errorType;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\UserControls\StudentDataUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errorTypeText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CMSUI;component/usercontrols/studentdatausercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\StudentDataUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.number = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.firstName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.lastName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.regNo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.deleteStudentData = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\UserControls\StudentDataUserControl.xaml"
            this.deleteStudentData.Click += new System.Windows.RoutedEventHandler(this.DeleteStudentData_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.errorType = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.errorTypeText = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

