using BusinessLogic;
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RestaurantManagementProject
{
    public partial class CategoryForm : Form
    {
        List<Category> listCategory = new List<Category>();
        Category categoryCurrent = new Category();
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            cboCategoryType.Items.Add("Thức ăn");
            cboCategoryType.Items.Add("Đồ uống");
            cboCategoryType.SelectedIndex = 0;

            LoadCategoryDataToListView();
        }

        public void LoadCategoryDataToListView()
        {
            CategoryBL categoryBL = new CategoryBL();
            listCategory = categoryBL.GetAll();
            int count = 1;
            lvCategory.Items.Clear();
            foreach (var category in listCategory)
            {
                ListViewItem item = lvCategory.Items.Add(count.ToString());
                item.SubItems.Add(category.Name);

                // Hiển thị text thay vì số
                string typeText = (category.Type == 0) ? "Đồ uống" : "Thức ăn";
                item.SubItems.Add(typeText);

                item.Tag = category.ID;
                count++;
            }
        }
        private void lvCategory_Click(object sender, EventArgs e)
        {
            if (lvCategory.SelectedItems.Count > 0)
            {
                int selectedIndex = lvCategory.SelectedItems[0].Index;
                categoryCurrent = listCategory[selectedIndex]; // GIỜ KHÔNG LỖI NỮA
                txtCategoryName.Text = categoryCurrent.Name;

                // Chọn đúng item trong ComboBox dựa trên Type
                if (categoryCurrent.Type == 0)
                    cboCategoryType.SelectedItem = "Đồ uống";
                else
                    cboCategoryType.SelectedItem = "Thức ăn";
            }
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtCategoryName.Text = "";
            if (cboCategoryType.Items.Count > 0)
                cboCategoryType.SelectedIndex = 0;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            int result = InsertCategory();
            if (result > 0)
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadCategoryDataToListView();
            }
            else
                MessageBox.Show("Thêm danh mục không thành công");
        }
        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            int result = UpdateCategory();
            if (result > 0)
            {
                MessageBox.Show("Cập nhật danh mục thành công");
                LoadCategoryDataToListView();
            }
            else
                MessageBox.Show("Cập nhật danh mục không thành công");
        }
        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xoá danh mục này?", "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                CategoryBL categoryBL = new CategoryBL();
                if (categoryBL.Delete(categoryCurrent) > 0)
                {
                    MessageBox.Show("Xoá danh mục thành công");
                    LoadCategoryDataToListView();
                }
                else
                    MessageBox.Show("Xoá không thành công");
            }
        }

        public int InsertCategory()
        {
            Category category = new Category();
            category.ID = 0;

            if (txtCategoryName.Text == "" || cboCategoryType.SelectedItem == null)
            {
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
                return -1;
            }

            category.Name = txtCategoryName.Text;

            int type = 0;
            if (cboCategoryType.SelectedItem.ToString() == "Đồ uống")
                type = 0;
            else
                type = 1;
            category.Type = type;

            CategoryBL categoryBL = new CategoryBL();
            return categoryBL.Insert(category);
        }

        public int UpdateCategory()
        {
            Category category = categoryCurrent;

            if (txtCategoryName.Text == "" || cboCategoryType.SelectedItem == null)
            {
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
                return -1;
            }

            category.Name = txtCategoryName.Text;

            int type = 0;
            if (cboCategoryType.SelectedItem.ToString() == "Đồ uống")
                type = 0;
            else
                type = 1;
            category.Type = type;

            CategoryBL categoryBL = new CategoryBL();
            return categoryBL.Update(category);
        }


    }
}
