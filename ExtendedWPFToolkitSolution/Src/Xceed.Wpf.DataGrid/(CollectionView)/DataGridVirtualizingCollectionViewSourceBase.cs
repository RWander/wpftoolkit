﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   This program can be provided to you by Xceed Software Inc. under a
   proprietary commercial license agreement for use in non-Open Source
   projects. The commercial version of Extended WPF Toolkit also includes
   priority technical support, commercial updates, and many additional 
   useful WPF controls if you license Xceed Business Suite for WPF.

   Visit http://xceed.com and follow @datagrid on Twitter.

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Xceed.Wpf.DataGrid
{
  public abstract class DataGridVirtualizingCollectionViewSourceBase : DataGridCollectionViewSourceBase
  {
    public DataGridVirtualizingCollectionViewSourceBase()
    {

      this.Source = null;

      m_isInitialized = true;
      this.DataSourceProvider.DelayRefresh( this.Dispatcher, System.Windows.Threading.DispatcherPriority.DataBind );
    }

    #region PageSize Property

    public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
      "PageSize", typeof( int ), typeof( DataGridVirtualizingCollectionViewSourceBase ),
      new UIPropertyMetadata( DataGridVirtualizingCollectionViewBase.DefaultPageSize ) );

    public int PageSize
    {
      get
      {
        return ( int )this.GetValue( DataGridVirtualizingCollectionViewSourceBase.PageSizeProperty );
      }
      set
      {
        this.SetValue( DataGridVirtualizingCollectionViewSourceBase.PageSizeProperty, value );

        if( this.DataSourceProvider != null )
          this.DataSourceProvider.DelayRefresh( this.Dispatcher, System.Windows.Threading.DispatcherPriority.DataBind );
      }
    }

    #endregion PageSize Property

    #region MaxRealizedItemCount

    public static readonly DependencyProperty MaxRealizedItemCountProperty = DependencyProperty.Register(
      "MaxRealizedItemCount", typeof( int ), typeof( DataGridVirtualizingCollectionViewSourceBase ),
      new UIPropertyMetadata( DataGridVirtualizingCollectionViewBase.DefaultMaxRealizedItemCount ) );

    public int MaxRealizedItemCount
    {
      get
      {
        return ( int )GetValue( DataGridVirtualizingCollectionViewSourceBase.MaxRealizedItemCountProperty );
      }
      set
      {
        this.SetValue( DataGridVirtualizingCollectionViewSourceBase.MaxRealizedItemCountProperty, value );

        if( this.DataSourceProvider != null )
          this.DataSourceProvider.DelayRefresh( this.Dispatcher, System.Windows.Threading.DispatcherPriority.DataBind );
      }
    }

    #endregion MaxRealizedItemCount


    #region PreemptivePageQueryRatio

    public static readonly DependencyProperty PreemptivePageQueryRatioProperty = DependencyProperty.Register(
      "PreemptivePageQueryRatio", typeof( double ), typeof( DataGridVirtualizingCollectionViewSourceBase ),
      new UIPropertyMetadata( DataGridVirtualizingCollectionViewBase.DefaultPreemptivePageQueryRatio,
        new PropertyChangedCallback( DataGridCollectionViewSourceBase.OnDataGridCollectionViewSourceBaseDependencyPropertyChanged ) ) );

    public double PreemptivePageQueryRatio
    {
      get { return ( double )GetValue( DataGridVirtualizingCollectionViewSourceBase.PreemptivePageQueryRatioProperty ); }
      set { this.SetValue( DataGridVirtualizingCollectionViewSourceBase.PreemptivePageQueryRatioProperty, value ); }
    }

    #endregion PreemptivePageQueryRatio

    #region CommitMode

    public static readonly DependencyProperty CommitModeProperty = DependencyProperty.Register(
      "CommitMode", typeof( CommitMode ), typeof( DataGridVirtualizingCollectionViewSourceBase ),
      new UIPropertyMetadata( CommitMode.PageReleasedFromMemory,
        new PropertyChangedCallback( DataGridCollectionViewSourceBase.OnDataGridCollectionViewSourceBaseDependencyPropertyChanged ) ) );

    public CommitMode CommitMode
    {
      get { return ( CommitMode )GetValue( DataGridVirtualizingCollectionViewSourceBase.CommitModeProperty ); }
      set { this.SetValue( DataGridVirtualizingCollectionViewSourceBase.CommitModeProperty, value ); }
    }

    #endregion CommitMode

    #region DATA VIRTUALIZATION EVENTS

    public event EventHandler<CommitItemsEventArgs> CommitItems;

    internal void OnCommitItems( CommitItemsEventArgs e )
    {
      if( this.CommitItems != null )
        this.CommitItems( this, e );
    }

    #endregion DATA VIRTUALIZATION EVENTS


    #region INTERNAL PROPERTIES

    internal bool IsInitialized
    {
      get
      {
        return m_isInitialized;
      }
    }

    #endregion INTERNAL PROPERTIES


    #region INTERNAL METHODS

    internal override void ApplyExtraPropertiesToView( DataGridCollectionViewBase currentView )
    {
      base.ApplyExtraPropertiesToView( currentView );

      DataGridVirtualizingCollectionViewBase dataGridVirtualizingCollectionView = currentView as DataGridVirtualizingCollectionViewBase;

      dataGridVirtualizingCollectionView.PreemptivePageQueryRatio = this.PreemptivePageQueryRatio;
      dataGridVirtualizingCollectionView.CommitMode = this.CommitMode;
    }

    #endregion INTERNAL METHODS


    #region PRIVATE FIELDS

    private bool m_isInitialized;

    #endregion PRIVATE FIELDS

  }
}