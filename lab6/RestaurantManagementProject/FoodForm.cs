using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;

namespace RestaurantManagementProject
{
    public partial class FoodForm : Form
    {
        List<Category> listCategories = new List<Category>();
        List<Food> listFood = new List<Food>();
        Food foodCurrent = new Food();
        public FoodForm()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtPrice.Text = "0";
            txtUnit.Text = "";
            txtNotes.Text = "";
            if (cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;
        }

        private void FoodForm_Load(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodDataToListView();
        }

        private void LoadCategory()
        {
            CategoryBL categoryBL = new CategoryBL();
            listCategories = categoryBL.GetAll();
            cboCategory.DataSource = listCategories;
            cboCategory.ValueMember = "ID";
            cboCategory.DisplayMember = "Name";
        }

        public void LoadFoodDataToListView()
        {
            FoodBL foodBL = new FoodBL();
            listFood = foodBL.GetAll();
            int count = 1;
            lvFood.Items.Clear();
            foreach(var food in listFood)
            {
                ListViewItem item = lvFood.Items.Add(count.ToString());
                item.SubItems.Add(food.Name);
                item.SubItems.Add(food.Unit);
                item.SubItems.Add(food.Price.ToString());
                string foodName = listCategories.Find(x => x.ID == food.FoodCategoryID).Name;
                item.SubItems.Add(foodName);
                item.SubItems.Add(food.Notes);
                count++;
            }
        }

        private void lvFood_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvFood.Items.Count; i++)
            {
                if (lvFood.Items[i].Selected)
                {
                    foodCurrent = listFood[i];
                    txtName.Text = foodCurrent.Name;
                    txtUnit.Text = foodCurrent.Unit;
                    txtPrice.Text = foodCurrent.Price.ToString();
                    txtNotes.Text = foodCurrent.Notes;

                    cboCategory.SelectedIndex = listCategories.FindIndex(x => x.ID == foodCurrent.FoodCategoryID);
                }
            }
        }

        public int InsertFood()
        {
            //Khai báo đối tượng Food từ tầng DataAccess
            Food food = new Food();
            food.ID = 0;
            // Kiểm tra nếu các ô nhập khác rỗng
            if (txtName.Text == "" || txtUnit.Text == "" || txtPrice.Text == "")
                MessageBox.Show("Chứa nhập dữ liệu cho các ô, vui lòng nhập lại");
            else
            {
                //Nhận giá trị Name, Unit, và Notes từ người dùng nhập vào
                food.Name = txtName.Text;
                food.Unit = txtUnit.Text;
                food.Notes = txtNotes.Text;
                // Giá trị price là giá trị số nên cần bắt lỗi khi người dùng nhập sai
                int price = 0;
                try
                {
                    // Cố gắng lấy giá trị
                    price = int.Parse(txtPrice.Text);
                }
                catch
                {
                    // Nếu sai, gán giá = 0
                    price = 0;
                }
                food.Price = price;
                // Giá trị FoodCategoryID được lấy từ ComboBox
                food.FoodCategoryID = int.Parse(cboCategory.SelectedValue.ToString());
                // Khao báo đối tượng FoodBL từ tầng Business
                FoodBL foodBL = new FoodBL();
                // Chèn dữ liệu vào bảng
                return foodBL.Insert(food);
            }
            return -1;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            int result = InsertFood();
            if (result > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công");
                LoadFoodDataToListView();
            }
            else MessageBox.Show("Thêm dữ liệu không thành công. Vui lòng kiểm tra lại dữ liệu nhập");
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xoá mẫu tín này?", "Thông báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Khai bảo đối tượng FoodBL từ BusinessLogic
                FoodBL foodBL = new FoodBL();
                if (foodBL.Delete(foodCurrent) > 0)
                {
                    MessageBox.Show("Xoá thực phẩm thành công");
                    LoadFoodDataToListView();
                }
                else MessageBox.Show("Xoá không thành công");
            }
        }

        public int UpdateFood()
        {
            //Khai báo đối tượng Food và Lấy đối tượng hiện hành
            Food food = foodCurrent;
            // Kiểm tra nếu các ở nhập khác rỗng
            if (txtName.Text == "" || txtUnit.Text == "" || txtPrice.Text == "")
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
            else
            {
                //Nhận giá trị Name, Unit, và Notes khi người dùng sửa
                food.Name = txtName.Text;
                food.Unit = txtUnit.Text;
                food.Notes = txtNotes.Text;
                //Giá trị price là giá trị số nên cần bất lỗi khi người dùng nhập sai
                int price = 0;
                try
                {
                    // Chuyển giá trị từ kiểu văn bản qua kiểu int
                    price = int.Parse(txtPrice.Text);
                }
                catch
                {
                    // Nếu sai, gán giá = 0
                    price = 0;
                }
                food.Price = price;
                // Giá trị FoodCategoryID được lấy từ ComboBox
                food.FoodCategoryID = int.Parse(cboCategory.SelectedValue.ToString());
                // Khao báo đối tượng FoodBL từ tầng Business
                FoodBL foodBL = new FoodBL();
                // Cập nhật dữ liệu trong bảng
                return foodBL.Update(food);
            }
            return -1;
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            int result = UpdateFood();
            if (result > 0)
            {
                MessageBox.Show("Cập nhật dữ liệu thành công");
                LoadFoodDataToListView();
            }
            else MessageBox.Show("Cập nhật dữ liệu không thành công. Vui lòng kiểm tra lại dữ liệu nhập");
        }


    }
}
