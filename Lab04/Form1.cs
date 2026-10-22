using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04
{
    public partial class Form1 : Form
    {
        private string[] folderFile = null;
        private int selected = 0;
        private int begin = 0;
        private int end = 0;
        public Form1()
        {
            InitializeComponent();
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnStart.Enabled = false;
        }
        private void showImage(string path)
        {
            if (File.Exists(path)) 
            { 
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Image imgtemp = Image.FromStream(stream);
                    pictureBox1.Image = new Bitmap(imgtemp);
                }
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // Bước 2: Sự kiện Click nút "Mở Thư Mục hình"
        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // Lấy tất cả các tệp tin đuôi jpg, jpeg, bmp
                string[] part1 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg");
                string[] part2 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpeg");
                string[] part3 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.bmp");

                // Khởi tạo mảng tổng và gộp dữ liệu
                folderFile = new string[part1.Length + part2.Length + part3.Length];
                Array.Copy(part1, 0, folderFile, 0, part1.Length);
                Array.Copy(part2, 0, folderFile, part1.Length, part2.Length);
                Array.Copy(part3, 0, folderFile, part1.Length + part2.Length, part3.Length);

                // Kiểm tra xem thư mục có hình hay không
                if (folderFile.Length > 0)
                {
                    selected = 0;
                    begin = 0;
                    end = folderFile.Length - 1;

                    // Hiển thị hình đầu tiên
                    showImage(folderFile[selected]);

                    // Bật các nút chức năng
                    btnPrevious.Enabled = true;
                    btnNext.Enabled = true;
                    btnStart.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Thư mục đã chọn không có hình ảnh (.jpg, .jpeg, .bmp)!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Bước 3: Hàm chuyển về hình phía trước
        private void prevImage()
        {
            if (folderFile == null || folderFile.Length == 0) return;

            if (selected == 0)
            {
                selected = folderFile.Length - 1; // Nếu ở đầu thì quay về ảnh cuối
            }
            else
            {
                selected = selected - 1;
            }
            showImage(folderFile[selected]);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            prevImage();
        }

        // Bước 4: Hàm chuyển sang hình tiếp theo
        private void nextImage()
        {
            if (folderFile == null || folderFile.Length == 0) return;

            if (selected == folderFile.Length - 1)
            {
                selected = 0; // Nếu ở cuối thì quay về ảnh đầu tiên
            }
            else
            {
                selected = selected + 1;
            }
            showImage(folderFile[selected]);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            nextImage();
        }

        // Bước 5: Xử lý sự kiện Tick của Timer để tự động chuyển ảnh
        private void timer1_Tick(object sender, EventArgs e)
        {
            nextImage();
        }

        // Bước 6: Xử lý sự kiện Click nút "Bắt đầu / Kết thúc Slide Show"
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                btnStart.Text = "Bắt đầu";
            }
            else
            {
                timer1.Enabled = true;
                btnStart.Text = "Kết thúc Slide Show";
            }
        }

    }
}
