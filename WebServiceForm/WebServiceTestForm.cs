using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebServiceForm
{
    public partial class WebServiceTestForm : Form
    {
        //Создаем ссылку на сервис
        ServiceApp.SearchReportService serviceObj = new ServiceApp.SearchReportService();
        //Создание обьекта класса
        SQLiteWorker SQ = new SQLiteWorker();

        public WebServiceTestForm()
        {
            InitializeComponent();
        }
        //Обработчик кнопки        
        private void button1_Click(object sender, EventArgs e)
        {
            //Чистим Базу
            SQ.clearTable();
            //Чистим лист
            listView1.Items.Clear();
            //Создаем список типа Settings{Key, Value}, и получаем в него ответ сервиса
            List<ServiceApp.Settings> respList = serviceObj.Search(textBox1.Text).ToList();
            //Для каждого элемента списка, заполняем ListView и БД
            foreach (ServiceApp.Settings variable in respList)
            {
                ListViewItem LVitem = new ListViewItem(new string[] { variable.Key, variable.Value });
                //Добавляем построчно{Key, Value} поcледний запрос в базу
                SQ.addToTable(variable.Key, variable.Value);
                //Добавляем построчно{Key, Value} поcледний запрос в список
                listView1.Items.Add(LVitem);
            }  
        }
        /// <summary>
        /// При загрузке формы, коннект к базе, и получаем из нее инфо о последнем запросе, заполняем этим инфо лист
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebServiceTestForm_Load(object sender, EventArgs e)
        {
            SQ.CreateDatabase();
            string[] rezList = SQ.selectFromTable().Split('|');
            for (int i = 0; i < rezList.Length - 1; i = i + 2)
            {
                ListViewItem LVitem = new ListViewItem(new string[] { rezList[i].ToString(), rezList[i + 1].ToString() });
                listView1.Items.Add(LVitem);
            }
            

        }
    }
}
