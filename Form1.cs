using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {


        //private List<GroupModel> groups;
        private List<string> groups;
        private List<MemberModel> members;
        private List<LicenseListModel> licenseLists;
        private List<InoutListModel> inoutLists;

        private string licenseStart;
        private string licenseEnd;
        private string selectedGroup;
        private string selectedMember;
        private string reason;

        private string inoutFlag;

        private string InText = "入室";
        private string OutText = "退室";

        public Form1()
        {


            InitializeComponent();

            try {
                startICCardReader();
            }
            catch
            {

            }

            DataClass.editedDate = DateTime.Now;

            try { 
                initList();

                InText = ConfigurationManager.AppSettings["InText"];
                OutText = ConfigurationManager.AppSettings["OutText"];

                button2.Text = InText;
                button4.Text = OutText;
            }
            catch
            {
                MessageBox.Show("kintoneと正常に通信ができません。設定画面を開きます。");

                Form3 form3 = new Form3();

                form3.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if(selectedMember == null)
            {
                MessageBox.Show("氏名を選択してください。");
                return;
            }


            string selectedNumber = NameToNumber(selectedMember);

            

            RegisterList(selectedNumber,DateTime.Now);
           

        }



        private void startICCardReader()
        {
            var monitorFactory = MonitorFactory.Instance;
            var monitor = monitorFactory.Create(SCardScope.System);

            var contextFactory = ContextFactory.Instance.Establish(SCardScope.System);
            var readerNames = contextFactory.GetReaders();

 


            if (readerNames == null || readerNames.Length == 0)
            {
                label5.Text = "ICカードリーダーが接続されていません";
            }
            else
            {
                foreach (var readerName in readerNames)
                {


                    monitor.Start(readerName);

                    monitor.StatusChanged += (sender, args) =>
                    {

                        try
                        {
                            var isoReader = new IsoReader(contextFactory, readerName, SCardShareMode.Shared, SCardProtocol.Any, false);

                            //MFへカレントディレクトリを設定
                            var set_df = new CommandApdu(IsoCase.Case3Short, isoReader.ActiveProtocol)
                            {
                                CLA = 0x00,// Class
                                           //Instruction = InstructionCode.GetChallenge,
                                INS = 0xA4,
                                P1 = 0x00, // Parameter 1
                                P2 = 0x00, // Parameter 2
                                           //Lc = 0x02,
                                Data = new byte[] { 0x3F, 0x00 }
                            };

                            //カレントディレクトリを共通要素へ移動（パスコードなし）
                            var set_mf = new CommandApdu(IsoCase.Case3Short, isoReader.ActiveProtocol)
                            {
                                CLA = 0x00,// Class
                                INS = 0xA4,
                                P1 = 0x02,
                                P2 = 0x0C,
                                //Le = 0x02,
                                //ExpectedResponseLength = 0x03,
                                Data = new byte[] { 0x2F, 0x01 }
                            };

                            //データ読み出し要求
                            var read = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
                            {
                                CLA = 0x00,// Class
                                INS = 0xB0,
                                P1 = 0x00,
                                P2 = 0x00,
                                Le = 0x11,
                                //ExpectedResponseLength = 0x03,
                                //Data = new byte[] { 0x2F, 0x01 }
                            };


                            var response = isoReader.Transmit(set_df);
                            //Console.WriteLine("SW1 SW2 = {0:X2} {1:X2}", response.SW1, response.SW2);

                            //Console.WriteLine("-----MF Current End-----");


                            response = isoReader.Transmit(set_mf);
                            //Console.WriteLine("SW1 SW2 = {0:X2} {1:X2}", response.SW1, response.SW2);

                            //Console.WriteLine("-----Common Current End-----");


                            response = isoReader.Transmit(read);
                            //Console.WriteLine("SW1 SW2 = {0:X2} {1:X2}", response.SW1, response.SW2);

                            if (response.HasData)
                            {
                                String resposeText = BitConverter.ToString(response.GetData());
                                resposeText = resposeText.Replace("-", "");

                                licenseStart = resposeText.Substring(10, 8);
                                licenseEnd = resposeText.Substring(18, 8);

                                //Console.WriteLine( matchLicenseList() );

                                string number = matchLicenseList();

                                selectedGroup = NumberToName(number);
                                selectedMember = NumberToGroup(number);

                                if(number == "-1")
                                {
                                    MessageBox.Show("一致する免許証情報が見つかりません。\n手動で処理を行ってください。");
                                    return;
                                }
                                else
                                {
                                    //Thread subTheread = new Thread(RegisterList(number) );
                                    RegisterList(number,DateTime.Now);

                                    Thread.Sleep(2000);
                                    //別スレッドで処理して停止させるほうが良いので将来的には処理を変える
                                }


                            }


                        }
                        catch
                        {

                        }


                    };
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            initList();
                
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            inoutFlag = "in";

            button2.BackColor = Color.YellowGreen;
            button2.ForeColor = Color.White;

            button4.BackColor = Color.White;
            button4.ForeColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            inoutFlag = "out";

            button4.BackColor = Color.OrangeRed;
            button4.ForeColor = Color.White;
            
            button2.BackColor = Color.White;
            button2.ForeColor = Color.Black;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            members = MemberModel.FindAll<MemberModel>();

            listBox2.Items.Clear();

            object selectedItem = listBox1.SelectedItem;
            selectedGroup = selectedItem.ToString();

            //members = MemberModel.FindAll<MemberModel>();
            //グループはMEMBERから生成する。グループを個別に呼び出すようにするかは検討

            foreach (MemberModel member in members)
            {
                if(member.GROUP== selectedItem.ToString() )
                    listBox2.Items.Add(member.NAME.ToString());

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private string matchLicenseList()
        {
            foreach(LicenseListModel licenseList in licenseLists )
            {

                string temp_start = licenseList.LICENSE_START.ToString("yyyyMMdd");

                string temp_end = licenseList.LICENSE_END.ToString("yyyyMMdd");


                //↓取得した日付データはハイフンが入っているので成型して比較する
                if (licenseStart== temp_start&& licenseEnd== temp_end)
                {

                    return licenseList.NUMBER;
                }
            }

            return "-1";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedItem = listBox2.SelectedItem;
            selectedMember = selectedItem.ToString();
        }

        private string NameToNumber(string name)
        {
            foreach(MemberModel temp in members)
            {

                if (temp.NAME == name)
                    return temp.NUMBER;
            }

                return "-1";
            

        }
        private string NumberToName(string number)
        {
            foreach(MemberModel temp in members)
            {
                if (temp.NUMBER == number)
                    return temp.NAME;
            }

            return "-1";
        }

        private string NumberToGroup(string number)
        {
            foreach (MemberModel temp in members)
            {
                if (temp.NUMBER == number)
                    return temp.GROUP;
            }

            return "-1";
        }


        private void initList()
        {
            inoutFlag = "";
            button2.BackColor = Color.White;
            button2.ForeColor = Color.Black;
            button4.BackColor = Color.White;
            button4.ForeColor = Color.Black;

            if(licenseLists != null)
                licenseLists.Clear();

            licenseLists = LicenseListModel.FindAll<LicenseListModel>();

            if (members != null)
                members.Clear();

            members = MemberModel.FindAll<MemberModel>();

            if (groups != null)
                groups.Clear();


            List<string> temp_group = new List<string>();

            foreach (MemberModel member in members)
            {
                temp_group.Add(member.GROUP);
            }

            groups = temp_group.Distinct().ToList();

            listBox1.Items.Clear();

            foreach (string temp in groups)
                listBox1.Items.Add(temp);

            listBox2.Items.Clear();

            label2.Text = "－－－－－－";

            reason = button5.Text.ToString();
            button5.BackColor = Color.LightBlue;
           
        }

        private void RegisterList(String tempNumber, DateTime date)
        {
            if (inoutFlag == "")
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("【"+InText+"】もしくは【"+OutText+"】を選択してください。");
                return;
            }

            if (inoutLists != null && 1 <= inoutLists.Count)
                inoutLists.Clear();

            /*List<InoutListModel> */
            inoutLists = InoutListModel.Find<InoutListModel>(x => x.NUMBER == tempNumber);
            //List<InoutListModel> aaa = InoutListModel.FindAll<InoutListModel>();


            foreach (InoutListModel inoutList in inoutLists)
            {


                if (inoutList.CHECK == "△")
                {
                    if (inoutFlag == "in")
                    {
                        System.Media.SystemSounds.Hand.Play();
                        MessageBox.Show("直前の記録が【"+InText+"】です。\n一度、【"+OutText+"】で登録してください。");
                        return;
                    }
                    else
                    {
                        inoutList.OUT_DATE = date;
                        inoutList.OUT_TIME = date;
                        inoutList.CHECK = "◯";

                        inoutList.Update();

                        System.Media.SystemSounds.Asterisk.Play();
                        label2.Text="【"+OutText+"】"+selectedMember+" 様";
                        timer2.Start();

                        return;
                    }
                }

                else
                {
                    if (inoutFlag == "out")
                    {
                        System.Media.SystemSounds.Hand.Play();
                        MessageBox.Show("直前の記録が【"+OutText+"】です。\n一度、【"+InText+"】で登録してください。");
                        return;
                    }

                    else
                    {
                        InoutListModel addModel = new InoutListModel();

                        addModel.GROUP = selectedGroup;
                        addModel.NAME = selectedMember;
                        addModel.NUMBER = tempNumber;
                        addModel.IN_DATE = date;
                        addModel.IN_TIME = date;
                        addModel.CHECK = "△";
                        addModel.REASON = reason;
                        addModel.Create();

                        System.Media.SystemSounds.Asterisk.Play();
                        label2.Text = "【"+InText+"】" + selectedMember + " 様";
                        timer2.Start();

                        return;
                    }
                }

            }



        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            initList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.BackColor = Color.LightBlue;
            button6.BackColor = Color.White;

            reason = button5.Text.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.BackColor = Color.LightBlue;
            button5.BackColor = Color.White;

            reason = button6.Text.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string selectedNumber;

            if (selectedMember == null)
            {
                MessageBox.Show("氏名を選択してください。");
                return;
            }

            selectedNumber = NameToNumber(selectedMember);

            Form2 form2 = new Form2();

            form2.ShowDialog();

            if (form2.DialogResult == DialogResult.OK)
                RegisterList(selectedNumber, DataClass.editedDate);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();

            form3.ShowDialog();
        }
    }
}
