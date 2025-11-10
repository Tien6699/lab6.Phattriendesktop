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

namespace RestaurantManagementProject
{
    public partial class RolesForm : Form
    {
        private List<Account> accounts = new List<Account>();
        private List<Role> roles = new List<Role>();
        private RoleBL roleBL = new RoleBL();
        private AccountBL accountBL = new AccountBL();
        public RolesForm()
        {
            InitializeComponent();
        }

        private void LoadAccounts()
        {
            accounts = accountBL.GetAll();
            lbxAccount.Items.Clear();
            foreach (var acc in accounts)
            {
                lbxAccount.Items.Add(acc.AccountName);
            }
        }

        private void LoadRoles()
        {
            roles = roleBL.GetAllRoles();
            chlbxRoles.Items.Clear();
            foreach (var role in roles)
            {
                chlbxRoles.Items.Add(role.RoleName, false); 
            }
        }

        private void RolesForm_Load(object sender, EventArgs e)
        {
            LoadAccounts();
            LoadRoles();
        }

        private void LoadUserRoles(string accountName)
        {
            for (int i = 0; i < chlbxRoles.Items.Count; i++)
            {
                chlbxRoles.SetItemChecked(i, false);
            }

            var userRoles = roleBL.GetUserRoles(accountName);
            foreach (var userRole in userRoles)
            {
                if (userRole.Actived)
                {
                    var role = roles.Find(r => r.ID == userRole.RoleID);
                    if (role != null)
                    {
                        int index = chlbxRoles.Items.IndexOf(role.RoleName);
                        if (index >= 0)
                        {
                            chlbxRoles.SetItemChecked(index, true);
                        }
                    }
                }
            }
        }

        private void lbxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxAccount.SelectedIndex >= 0)
            {
                string selectedAccount = lbxAccount.SelectedItem.ToString();
                var account = accounts.Find(a => a.AccountName == selectedAccount);

                if (account != null)
                {
                    txtUsername.Enabled = false; // Khóa không cho sửa tên đăng nhập
                    txtUsername.Text = account.AccountName;
                    txtName.Text = account.FullName;
                    txtEmail.Text = account.Email ?? "";
                    txtPhone.Text = account.Tell ?? "";
                    LoadUserRoles(selectedAccount);
                }
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chlbxRoles.Items.Count; i++)
            {
                chlbxRoles.SetItemChecked(i, false);
            }
            txtUsername.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            lbxAccount.ClearSelected();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            // THÊM TÀI KHOẢN MỚI TRỰC TIẾP
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập hoặc họ tên", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra tên đăng nhập đã tồn tại chưa
            var existingAccount = accountBL.GetByID(txtUsername.Text);
            if (existingAccount != null)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên khác.", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo tài khoản mới với mật khẩu mặc định "123"
            Account newAccount = new Account
            {
                AccountName = txtUsername.Text,
                Password = "123", // Mật khẩu mặc định
                FullName = txtName.Text,
                Email = txtEmail.Text,
                Tell = txtPhone.Text,
                DateCreated = DateTime.Now
            };

            int result = accountBL.Insert(newAccount);
            if (result > 0)
            {
                MessageBox.Show($"Đã thêm tài khoản thành công!\nTên đăng nhập: {txtUsername.Text}\nMật khẩu mặc định: 123",
                               "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear form nhập liệu
                cmdClear.PerformClick();

                // Load lại danh sách tài khoản
                LoadAccounts();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdAssignRoles_Click(object sender, EventArgs e)
        {
            if (lbxAccount.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để gán quyền!");
                return;
            }

            SaveUserRoles();
            MessageBox.Show("Đã gán quyền thành công!");
        }

        private void SaveUserRoles()
        {
            if (lbxAccount.SelectedIndex < 0) return;

            string accountName = lbxAccount.SelectedItem.ToString();
            List<RoleAccount> userRoles = new List<RoleAccount>();

            for (int i = 0; i < chlbxRoles.Items.Count; i++)
            {
                if (chlbxRoles.GetItemChecked(i))
                {
                    string roleName = chlbxRoles.Items[i].ToString();
                    var role = roles.Find(r => r.RoleName == roleName);

                    if (role != null)
                    {
                        userRoles.Add(new RoleAccount
                        {
                            RoleID = role.ID,
                            AccountName = accountName,
                            Actived = true,
                            Notes = ""
                        });
                    }
                }
            }

            int result = roleBL.UpdateUserRoles(accountName, userRoles);
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (lbxAccount.SelectedIndex < 0 || string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản hoặc nhập họ tên", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string accountName = lbxAccount.SelectedItem.ToString();

            // Lấy thông tin tài khoản hiện tại
            var account = accountBL.GetByID(accountName);
            if (account == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật thông tin mới
            account.FullName = txtName.Text;
            account.Email = txtEmail.Text;
            account.Tell = txtPhone.Text;

            int result = accountBL.Update(account);
            if (result > 0)
            {
                MessageBox.Show("Đã cập nhật thông tin tài khoản thành công!", "Thành công",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Load lại danh sách để cập nhật
                LoadAccounts();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin thất bại", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (lbxAccount.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string accountName = lbxAccount.SelectedItem.ToString();

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{accountName}'?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int result = accountBL.Delete(accountName);
                if (result > 0)
                {
                    MessageBox.Show("Đã xóa tài khoản thành công!", "Thành công",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAccounts();
                    cmdClear.PerformClick();
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại", "Lỗi",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
