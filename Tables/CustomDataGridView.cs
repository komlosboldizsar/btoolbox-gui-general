﻿using BToolbox.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BToolbox.GUI.Tables
{
    public class CustomDataGridView<T> : DataGridView
    {

        private IEnumerable<T> boundCollection;

        public IEnumerable<T> BoundCollection
        {
            get => boundCollection;
            set
            {
                if (boundCollection is IObservableEnumerable<T> oldBoundCollection)
                {
                    oldBoundCollection.ItemsAdded -= itemsAddedHandler;
                    oldBoundCollection.ItemsRemoved -= itemsRemovedHandler;
                }
                boundCollection = value;
                InvokeIfRequired(loadItems);
                if (boundCollection is IObservableEnumerable<T> newBoundCollection)
                {
                    newBoundCollection.ItemsAdded += itemsAddedHandler;
                    newBoundCollection.ItemsRemoved += itemsRemovedHandler;
                }
            }
        }

        private List<CustomDataGridViewColumnDescriptor<T>> columnDescriptors = new();
        public IReadOnlyList<CustomDataGridViewColumnDescriptor<T>> ColumnDescriptors => columnDescriptors;

        public CustomDataGridView() => init();

        public CustomDataGridView(IObservableEnumerable<T> boundCollection)
        {
            BoundCollection = boundCollection;
            init();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                BoundCollection = null;
        }

        private void init()
        {
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = false;
            saveHeaderProperties();
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            base.OnCellEndEdit(e);
            if ((e.RowIndex < 0) || (e.RowIndex >= Rows.Count))
                return;
            CustomDataGridViewRow<T> row = Rows[e.RowIndex] as CustomDataGridViewRow<T>;
            row?.HandleEndEdit(e);
        }

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            base.OnCellValueChanged(e);
            if ((e.RowIndex < 0) || (e.RowIndex >= Rows.Count))
                return;
            CustomDataGridViewRow<T> row = Rows[e.RowIndex] as CustomDataGridViewRow<T>;
            row?.HandleValueChanged(e);
        }

        protected override void OnCellContentClick(DataGridViewCellEventArgs e)
        {
            base.OnCellContentClick(e);
            if ((e.RowIndex < 0) || (e.RowIndex >= Rows.Count))
                return;
            CustomDataGridViewRow<T> row = Rows[e.RowIndex] as CustomDataGridViewRow<T>;
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            if (cell is DataGridViewCheckBoxCell checkBoxCell)
                cell.Value = !(bool)cell.Value;
            row?.HandleContentClick(e);
        }

        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            base.OnCellDoubleClick(e);
            if ((e.RowIndex < 0) || (e.RowIndex >= Rows.Count))
                return;
            CustomDataGridViewRow<T> row = Rows[e.RowIndex] as CustomDataGridViewRow<T>;
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            if (cell is DataGridViewCheckBoxCell checkBoxCell)
                cell.Value = !(bool)cell.Value;
            row?.HandleDoubleClick(e);
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);
            if ((e.RowIndex < 0) || (e.RowIndex >= Rows.Count))
                return;
            CustomDataGridViewRow<T> row = Rows[e.RowIndex] as CustomDataGridViewRow<T>;
            DataGridViewColumn column = Columns[e.ColumnIndex];
            CustomDataGridViewDragSourceEventArgs<T> dragEventArgs = new()
            {
                Table = this,
                ColumnIndex = e.ColumnIndex,
                Column = column,
                RowIndex = e.RowIndex,
                Row = row,
                Cell = row.Cells[e.ColumnIndex]
            };
            DragHandlers.GetDragData(dragEventArgs, out DragDropEffects allowedEffects, out object draggedObject);
            if (allowedEffects != DragDropEffects.None)
                DoDragDrop(draggedObject, allowedEffects);
        }

        public CustomDataGridViewDragHandlerCollection<T> DragHandlers { get; } = new();

        private void itemsAddedHandler(IEnumerable<IObservableCollection<T>.ItemWithPosition> affectedItemsWithPositions)
            => InvokeIfRequired(() => affectedItemsWithPositions.Foreach(aiwp => Rows.Insert(aiwp.Position, new CustomDataGridViewRow<T>(this, aiwp.Item))));

        private void itemsRemovedHandler(IEnumerable<IObservableCollection<T>.ItemWithPosition> affectedItemsWithPositions)
            => InvokeIfRequired(() => affectedItemsWithPositions.OrderByDescending(aiwp => aiwp.Position).Foreach(aiwp => Rows.RemoveAt(aiwp.Position)));

        private void loadItems()
        {
            Rows.Clear();
            if (boundCollection == null)
                return;
            foreach (T item in boundCollection)
                Rows.Add(new CustomDataGridViewRow<T>(this, item));
        }

        public DataGridViewColumn AddColumn(CustomDataGridViewColumnDescriptor<T> columnDescriptor)
        {
            columnDescriptors.Add(columnDescriptor);
            DataGridViewColumn column = getColumnByType(columnDescriptor.Type, columnDescriptor.CustomTypeDescriptor);
            column.Tag = new CustomDataGridViewColumnTag()
            {
                ID = columnDescriptor.ID
            };
            column.HeaderText = columnDescriptor.Header;
            column.Width = columnDescriptor.Width + columnDescriptor.DividerWidth;
            column.DividerWidth = columnDescriptor.DividerWidth;
            if(columnDescriptor.CellStyle != null)
                column.DefaultCellStyle = columnDescriptor.CellStyle;
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Columns.Add(column);
            columnDescriptor.Extensions?.Foreach(ext => ext.ColumnReady(this, column));
            return column;
        }

        public void ColumnChangeReady() => loadItems();

        private static DataGridViewColumn getColumnByType(DataGridViewColumnType type, CustomDataGridViewCustomColumnTypeDescriptor customTypeDescriptor)
        {
            switch (type)
            {
                case DataGridViewColumnType.TextBox:
                    return new DataGridViewTextBoxColumn();
                case DataGridViewColumnType.CheckBox:
                    return new DataGridViewCheckBoxColumn();
                case DataGridViewColumnType.Image:
                    return new DataGridViewImageColumn();
                case DataGridViewColumnType.Button:
                    return new DataGridViewButtonColumn();
                case DataGridViewColumnType.ComboBox:
                    return new DataGridViewComboBoxColumn();
                case DataGridViewColumnType.Link:
                    return new DataGridViewLinkColumn();
                case DataGridViewColumnType.DisableButton:
                    return new DataGridViewButtonColumn();
                case DataGridViewColumnType.ImageButton:
                    return new DataGridViewButtonColumn();
                case DataGridViewColumnType.SmallIcon:
                    return new DataGridViewTextBoxColumn();
                case DataGridViewColumnType.Custom:
                    return customTypeDescriptor.CreateColumn();
            }
            return null;
        }

        private bool verticalHeader = false;

        public bool VerticalHeader
        {
            get => verticalHeader;
            set
            {
                verticalHeader = value;
                if (value)
                {
                    setHeaderPropertiesToVertical();
                    CellPainting += cellPaintHandlerOnVerticalHeader;
                }
                else
                {
                    restoreHeaderProperties();
                    CellPainting -= cellPaintHandlerOnVerticalHeader;
                }
                Invalidate();
            }
        }

        DataGridViewColumnHeadersHeightSizeMode _columnHeadersHeightSizeMode;
        int _columnHeadersHeight;
        DataGridViewAutoSizeColumnsMode _autoSizeColumnsMode;

        private void saveHeaderProperties()
        {
            _columnHeadersHeightSizeMode = ColumnHeadersHeightSizeMode;
            _columnHeadersHeight = ColumnHeadersHeight;
            _autoSizeColumnsMode = AutoSizeColumnsMode;
        }

        private void restoreHeaderProperties()
        {
            ColumnHeadersHeightSizeMode = _columnHeadersHeightSizeMode;
            ColumnHeadersHeight = _columnHeadersHeight;
            AutoSizeColumnsMode = _autoSizeColumnsMode;
        }

        private void setHeaderPropertiesToVertical()
        {
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 50;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
        }

        // @source https://stackoverflow.com/a/5783099
        void cellPaintHandlerOnVerticalHeader(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                Rectangle rect = GetColumnDisplayRectangle(e.ColumnIndex, true);
                Size titleSize = TextRenderer.MeasureText(e.Value.ToString(), e.CellStyle.Font);
                if (ColumnHeadersHeight < titleSize.Width)
                    ColumnHeadersHeight = titleSize.Width + 15;
                e.Graphics.TranslateTransform(0, titleSize.Width);
                e.Graphics.RotateTransform(-90.0F);
                e.Graphics.DrawString(e.Value.ToString(), this.Font, Brushes.Black, new PointF(rect.Y - (ColumnHeadersHeight - titleSize.Width), rect.X + rect.Width/2 - titleSize.Height/2));
                e.Graphics.RotateTransform(90.0F);
                e.Graphics.TranslateTransform(0, -titleSize.Width);
                e.Handled = true;
            }
        }

        public void InvokeIfRequired(Action action)
        {
            if (InvokeSafe && InvokeRequired)
            {
                Invoke(action);
                return;
            }
            action();
        }

        public bool InvokeSafe { get; set; } = false;

    }
}
