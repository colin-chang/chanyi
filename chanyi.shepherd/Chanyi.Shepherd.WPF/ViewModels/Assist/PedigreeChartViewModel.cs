using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel;
using Microsoft.Win32;
using System.IO;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class PedigreeChartViewModel : ListViewModel
    {
        public PedigreeChartViewModel(bool withinInitilization) { }

        public PedigreeChartViewModel(string header, string icon, string intro, Canvas canvas)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.CanvasDraw = canvas;
            InitializeBindData();

            Mapper.CreateMap<FamilyTree, FamilyTreeData>();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                this.Sheeps = this.Service.GetSheepBind(null);
                this.Sheeps.Insert(0, new SheepBind { SerialNumber = ConfigurationManager.AppSettings["formDefaultSelection"] });
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        private bool selectAnyone = false;

        public bool SelectAnyone
        {
            get { return selectAnyone; }
            set
            {
                selectAnyone = value;
                this.RaisePropertyChanged("SelectAnyone");
            }
        }


        #region 绑定查看字段

        /// <summary>
        /// 搜索
        /// </summary>

        private List<SheepBind> sheeps;
        public List<SheepBind> Sheeps
        {
            get { return sheeps; }
            set
            {
                sheeps = value;
                this.RaisePropertyChanged("Sheeps");
            }
        }

        private string sheepId;

        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.RaisePropertyChanged("SheepId");
            }
        }
        #endregion

        #region 右边栏列表数据
        protected override void LoadData()
        {
            if (string.IsNullOrWhiteSpace(this.SheepId) || this.Sheeps.Where(s => s.Id == this.SheepId).Count() <= 0)
            {
                this.SelectAnyone = false;
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                this.SelectAnyone = true;
                this.FamilyTreeList = new List<FamilyTreeData>();
                this.Service.GetFamilyTree(this.SheepId, 5).ForEach(t => this.FamilyTreeList.Add(Mapper.Map<FamilyTreeData>(t)));

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.SelectAnyone = true;
                    this.CanvasDraw.Children.Clear();
                    if (this.FamilyTreeList.Count > 0)
                    {
                        //调用画图
                        DrawFamilytree(this.FamilyTreeList.Max(t => t.Generation), 0, 1, this.FamilyTreeList, this.FamilyTreeList.Where(t => t.Generation == 0).FirstOrDefault());
                    }
                    //右边栏DataGrid
                    this.Table.ItemsSource = this.FamilyTreeList;
                    this.Table.SelectedIndex = 0;

                    this.ProgressRing.Visibility = Visibility.Hidden;
                }), null);
            };
            this.ProgressRing.Visibility = Visibility.Visible;
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

            this.GridMinHeight = CanvasDraw.ActualHeight;
            this.GridMinWidth = CanvasDraw.ActualWidth;
        }

        #endregion

        #region 画图

        public new Chanyi.Shepherd.WPF.UserControls.ProgressRing ProgressRing { get { return this.CanvasDraw.Tag as Chanyi.Shepherd.WPF.UserControls.ProgressRing; } }

        private List<FamilyTreeData> familyTreeList = new List<FamilyTreeData>();
        public List<FamilyTreeData> FamilyTreeList
        {
            get { return familyTreeList; }
            set { familyTreeList = value; }
        }

        private Canvas canvasDraw;
        public Canvas CanvasDraw
        {
            get { return canvasDraw; }
            set
            {
                canvasDraw = value;
                this.RaisePropertyChanged("CanvasDraw");
            }
        }


        private double gridMinHeight;
        public double GridMinHeight
        {
            get { return gridMinHeight; }
            set
            {
                gridMinHeight = value;
                this.RaisePropertyChanged("GridMinHeight");
            }
        }

        private double gridMinWidth;
        public double GridMinWidth
        {
            get { return gridMinWidth; }
            set
            {
                gridMinWidth = value;
                this.RaisePropertyChanged("GridMinWidth");
            }
        }

        public float WindowWidth
        {
            get
            {
                return (float)CanvasDraw.ActualWidth;// return 880;
            }
        }
        public float WindowHeight
        {
            get
            {
                return (float)CanvasDraw.ActualHeight;//return 500;
            }
        }
        public float BtnWidth
        {
            get
            {
                return (float)CanvasDraw.ActualWidth / 16 - 2; //50; 
            }
        }
        public float BtnHeight { get { return 20; } }
        public SolidColorBrush MaleColor { get { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1BCBFF")); } }
        public SolidColorBrush FemaleColor { get { return Brushes.OrangeRed; } }

        /// <summary>
        /// 称呼
        /// </summary>
        private Dictionary<string, string> appellations;
        private Dictionary<string, string> Appellations
        {
            get
            {
                if (appellations == null || appellations.Count <= 0)
                {
                    appellations = new Dictionary<string, string>();
                    //a代表父（第一层为自己）
                    //b代表母
                    appellations.Add("", "自己");
                    appellations.Add("a", "父本");
                    appellations.Add("aa", "父父本");
                    appellations.Add("aaa", "父父父本");
                    appellations.Add("aaaa", "父父父父本");
                    appellations.Add("aaab", "父父父母本");
                    appellations.Add("aab", "父父母本");
                    appellations.Add("aaba", "父父母父本");
                    appellations.Add("aabb", "父父母母本");
                    appellations.Add("ab", "父母本");
                    appellations.Add("aba", "父母父本");
                    appellations.Add("abaa", "父母父父本");
                    appellations.Add("abab", "父母父母本");
                    appellations.Add("abb", "父母母本");
                    appellations.Add("abba", "父母母父本");
                    appellations.Add("abbb", "父母母母本");
                    appellations.Add("b", "母本");
                    appellations.Add("ba", "母父本");
                    appellations.Add("baa", "母父父本");
                    appellations.Add("baaa", "母父父父本");
                    appellations.Add("baab", "母父父母本");
                    appellations.Add("bab", "母父母本");
                    appellations.Add("baba", "母父母父本");
                    appellations.Add("babb", "母父母母本");
                    appellations.Add("bb", "母母本");
                    appellations.Add("bba", "母母父本");
                    appellations.Add("bbaa", "母母父父本");
                    appellations.Add("bbab", "母母父母本");
                    appellations.Add("bbb", "母母母本");
                    appellations.Add("bbba", "母母母父本");
                    appellations.Add("bbbb", "母母母母本");
                }
                return appellations;
            }
        }

        /// <summary>
        /// 绘制系谱(深度遍历)
        /// </summary>
        /// <param name="MaxDepth">最大深度</param>
        /// <param name="childCenter">left基准点</param>
        /// <param name="power">2的几次方（每进一层乘以2）</param>
        /// <param name="list">要处理的集合</param>
        /// <param name="curModel">当前节点</param>
        /// <param name="callMark">称呼标记（a代表公（默认代表自己），b代表母）</param>
        void DrawFamilytree(float maxDepth, float childCenter, int power, List<FamilyTreeData> list, FamilyTreeData curModel, string callMark = "")
        {
            if (curModel == null || curModel.Generation > maxDepth)
                return;

            float top = WindowHeight - WindowHeight / (maxDepth + 1) / 2 - BtnHeight - WindowHeight / (maxDepth + 1) * curModel.Generation;
            float left = 0;

            power = power * 2;

            bool isMale = curModel.Gender == GenderEnum.Male;

            //根节点处理不分男女
            if (childCenter == 0 || curModel.Generation == 0)
            {
                left = (WindowWidth - BtnWidth) / 2;
                //添加提示的两个矩形
                AddInfo(top);
            }
            else
            {
                callMark += isMale ? "a" : "b";
                left = childCenter - BtnWidth / 2 + (isMale ? (-1) : 1) * WindowWidth / power;
                //从第二级开始划线
                AddLine(left, top, childCenter, isMale, maxDepth);
            }

            if (Appellations.ContainsKey(callMark))
                curModel.Appellations = Appellations[callMark];//添加称呼

            FamilyTreeButton btn = new FamilyTreeButton(BtnWidth, BtnHeight, curModel.SerialNumber, curModel.Id, left, top);
            btn.IsEnabled = curModel.Status != SheepStatusEnum.Outer;

            btn.Background = isMale ? MaleColor : FemaleColor;

            //if (curModel.Gender == Gender.Male)
            //{
            //    btn.Style = (Style)new FrameworkElement().Resources["btnMale"];
            //}

            this.CanvasDraw.Children.Add(btn);

            childCenter = left + BtnWidth / 2;
            FamilyTreeData father = list.Where(t => (t.Id == curModel.FatherId) && (t.Generation == curModel.Generation + 1) && (string.IsNullOrEmpty(t.Appellations))).FirstOrDefault();
            DrawFamilytree(maxDepth, childCenter, power, list, father, callMark);

            FamilyTreeData mother = list.Where(t => (t.Id == curModel.MotherId) && (t.Generation == curModel.Generation + 1) && (string.IsNullOrEmpty(t.Appellations))).FirstOrDefault();
            DrawFamilytree(maxDepth, childCenter, power, list, mother, callMark);
        }
        /// <summary>
        /// 添加公母提示信息
        /// </summary>
        /// <param name="top"></param>
        private void AddInfo(float top)
        {
            //提示信息
            Rectangle recMale = new Rectangle();
            recMale.Margin = new Thickness(WindowWidth - BtnWidth * 2, top, 0, 0);
            recMale.Fill = MaleColor;
            recMale.Width = BtnWidth / 2;
            recMale.Height = BtnHeight;

            TextBlock tblMale = new TextBlock();
            tblMale.Text = "公";
            tblMale.Margin = new Thickness(WindowWidth - BtnWidth - 20, top + 3, 0, 0);

            Rectangle recFemale = new Rectangle();
            recFemale.Margin = new Thickness(WindowWidth - BtnWidth * 2, top + BtnHeight * 3 / 2, 0, 0);
            recFemale.Fill = FemaleColor;
            recFemale.Width = BtnWidth / 2;
            recFemale.Height = BtnHeight;

            TextBlock tblFemale = new TextBlock();
            tblFemale.Text = "母";
            tblFemale.Margin = new Thickness(WindowWidth - BtnWidth - 20, top + BtnHeight * 3 / 2 + 3, 0, 0);

            this.CanvasDraw.Children.Add(recMale);
            this.CanvasDraw.Children.Add(recFemale);
            this.CanvasDraw.Children.Add(tblMale);
            this.CanvasDraw.Children.Add(tblFemale);
        }

        /// <summary>
        /// 划线
        /// </summary>
        private void AddLine(float left, float top, float childCenter, bool isMale, float maxDepth)
        {
            Line line = new Line();
            line.X1 = left + BtnWidth / 2 + (isMale ? (1) : (-1)) * 2;
            line.Y1 = top + BtnHeight;
            line.X2 = childCenter;
            line.Y2 = top + WindowHeight / (maxDepth + 1);
            line.Stroke = Brushes.Orange;
            line.StrokeThickness = 3;

            this.CanvasDraw.Children.Add(line);
        }

        /// <summary>
        /// 系谱图节点点击事件
        /// </summary>
        public DelegateCommand<string> LoadCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    if (string.IsNullOrEmpty(id) || this.SheepId.ToUpper().Equals(id.ToUpper()))
                        return;

                    this.SheepId = id;
                    LoadData();
                });
            }
        }

        #endregion

        public override DelegateCommand ExportCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.FamilyTreeList == null || this.FamilyTreeList.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有任何数据，无法进行数据导出！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel 97-2003 工作簿(*.xls)|*.xls|Excel 工作簿(*.xlsx)|*.xlsx" };
                    if (sfd.ShowDialog() != true)
                        return;
                    Action export = () =>
                    {
                        var workbook = this.CreateExportSheet(this.FamilyTreeList.Count(), System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".xls" ? ExcelExtension.XLS : ExcelExtension.XLSX);
                        using (FileStream stream = System.IO.File.OpenWrite(sfd.FileName))
                        {
                            workbook.Write(stream);
                        }
                        this.UIDispatcher.Invoke(new Action(() => MessageBox.Show(Application.Current.MainWindow, "数据导出成功，成功导出" + this.FamilyTreeList.Count() + "条数据！", "消息", MessageBoxButton.OK, MessageBoxImage.Information)), null);
                    };
                    export.BeginInvoke(ar => export.EndInvoke(ar as IAsyncResult), export);
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.FamilyTreeList.ToArray();
        }
    }
}
