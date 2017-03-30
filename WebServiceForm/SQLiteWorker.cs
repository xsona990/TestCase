using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceForm
{
    /// <summary>
    /// Класс для работы с базой данных
    /// </summary>
    class SQLiteWorker
    {
        private string databaseName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "webService.db";
        public SQLiteWorker()
        {
        }

        /// <summary>
        /// Проверяет, есть ли файл базы данных в директории проэкта, если нет, создает БД, и таблицу lastquery.
        /// </summary>
        public void CreateDatabase()
        {
            
            if (!File.Exists(databaseName))
            { 
                SQLiteConnection.CreateFile(databaseName);
                CreateTable();
            }
        }
      /// <summary>
      /// Метод создания таблицы
      /// </summary>
       private void CreateTable()
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE lastquery (key TEXT, value TEXT);", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// Метод удаления всех записей из таблицы, необходим для того, что бы в БД хранились лишь результаты последнего запроса.
        /// </summary>
        public void clearTable()
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("DELETE FROM 'lastquery'", connection);
            command.ExecuteNonQuery();
            connection.Close();

        }
        /// <summary>
        /// Метод добавления записей в базу, построчно {Key, Value}
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void addToTable(string key, string value)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'lastquery' ('key', 'value') VALUES ('"+key+ "','" + value + "');", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// Метод получения всех записей в базе, возвращает строку(Лечше DataTable/DataSet, но нет времени).
        /// </summary>
        /// <returns></returns>
        public string selectFromTable()
        {
            string rezString = "";
           // int i = 0;
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'lastquery';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {

                rezString += record["key"] + "|" + record["value"]+"|";
            }
        
            connection.Close();
              return rezString;
           
        }
    }
}
