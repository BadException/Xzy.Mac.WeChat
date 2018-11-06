using IPADDemo.Model;
using IPADDemo.WeChat;
using Newtonsoft.Json;
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

namespace IPADDemo
{
    public partial class FormDemo : Form
    {
        public static FormDemo _formDemo;
        XzyWeChatThread weChatThread;

        public FormDemo()
        {
            InitializeComponent();
            WxDelegate.qrCode += new QrCode(this.calback_qrcode);
            WxDelegate.show += new Show(this.calback_show);
            WxDelegate.getContact += new GetContact(this.calback_getContact);
            WxDelegate.getGroup += new GetGroup(this.calback_getGroup);
            CheckForIllegalCrossThreadCalls = false;
            _formDemo = this;
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {
         
        }

        void calback_qrcode(string qrcode)
        {
            byte[] arr2 = Convert.FromBase64String(qrcode);
            using (MemoryStream ms2 = new MemoryStream(arr2))
            {
                System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                pictureBox1.Image = bmp2;
            }
        }

        void calback_show(string str)
        {
            textBox1.Text = str;
        }

        void calback_getContact(Contact contact)
        {
            string str = $"{contact.UserName}-{contact.NickName}-{contact.Country}-{contact.Provincia}-{contact.Remark}";
            lb_friend.Items.Add(str);
        }

        void calback_getGroup(Contact contact)
        {
            string str = $"{contact.UserName}-{contact.NickName}-{contact.Country}-{contact.Provincia}-{contact.Remark}";
            lb_group.Items.Add(str);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_SendMsg(txt_msgWxid.Text, txt_msgText.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "图片文件 |*.jpg;*.png";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string FileName = ofd.FileName;
                weChatThread.Wx_SendImg(txt_msgWxid.Text, FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_SendMoment(txt_snsText.Text);
        }



        private void button4_Click(object sender, EventArgs e)
        {
            List<string> imgPath = new List<string>();
            foreach (var a in listBox1.Items) {
                imgPath.Add(ImgToBase64String(a.ToString()));
            }
            weChatThread.Wx_SendMoment(txt_snsText.Text, imgPath);

        }
        private string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.AddRange(ofd.FileNames);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_CreateChatRoom(txt_GroupUsers.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_AddChatRoomMember(txt_groupwxid.Text,txt_groupuserwxid.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_DeleteChatRoomMember(txt_groupwxid.Text, txt_groupuserwxid.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_SetChatroomName(txt_groupwxid.Text, txt_groupname.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_SetChatroomAnnouncement(txt_groupwxid.Text, txt_groupgg.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            weChatThread.Wx_QuitChatRoom(txt_groupwxid.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string str= weChatThread.Wx_GenerateWxDat();
            WxDat wxDat = JsonConvert.DeserializeObject<WxDat>(str);
            txt_loginToken.Text = wxDat.data;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            weChatThread = new XzyWeChatThread();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            weChatThread = new XzyWeChatThread(txt_loginToken.Text,txt_loginName.Text,txt_loginPassword.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
           txt_positionReturn.Text= weChatThread.Wx_GetPeopleNearby(float.Parse(txt_Lat.Text), float.Parse(txt_Lng.Text));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string [] types= cb_addtype.Text.Split('-');
            int type = int.Parse(types[0]);
            string result = weChatThread.Wx_AddUser(txt_v1.Text, txt_v2.Text, type, txt_hellotext.Text);
        }
    }
}
