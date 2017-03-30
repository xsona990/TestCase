using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace SearchReportWebService
{   /// <summary>
    ///Класс поиска, содержит метод для поиска значений 
    /// </summary>
    [WebService(Description = "Поисковый класс")]
    public class SearchReportService : WebService
    {
        /// <summary>
        /// settingsList - список обьектов класса Settings, содержит данные из файла настроек(XMLData)
        /// searchRezultList - список найденных настроек по значению вхождений Key в searchField (насколько я понимаю, по значению Value искать не надо)
        /// Принимает на вход searchField - текст запроса
        /// Возвращает List<Settings> - список обьектов класса Settings
        /// </summary>
        public List<Settings> settingsList = new List<Settings>();
       public List<Settings> searchRezultList = new List<Settings>();

        [WebMethod(Description = "Принимает текстовую строку запроса, возвращает список обьектов типа Setting")]
        public List<Settings> Search(string searchField)
        {
            LoadFile(); // подгрузка всех возможных пар {Key, Value}
            string[] splitedSearchField = searchField.Split(' ');
            if (String.IsNullOrEmpty(searchField))    //Проверка на пустое поле запроса.
                return null;
            else
            {
                //Проверка на совпадения между текстом запроса, и имеющимися значениями Key(все значения в файле XMLData.xml)
                foreach (Settings item in settingsList)
                {
                    for (int i = 0; i < splitedSearchField.Length; i++)
                    {
                        if (item.Key.Contains(splitedSearchField[i]) )
                            if(!searchRezultList.Contains(item))
                            searchRezultList.Add(item);
                    }
                }
                

                return searchRezultList; 
            }
           
        }
        /// <summary>
        /// Метод парсит файл XMLData, и возвращает список обьектов класса Settings {Key, Value}
        /// </summary>
        private void LoadFile()
        {
            XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory.ToString()+"XMLData.xml");
            foreach (XElement settingsElement in xdoc.Element("ArrayOfSetting").Elements("Setting"))
            {
                XElement key = settingsElement.Element("Key");
                XElement value = settingsElement.Element("Value");

                if (key != null && value != null)
                {
                    settingsList.Add(new Settings() { Key = key.Value, Value = value.Value });  
                }
            }
        }
    }

    /// <summary>
    /// Класс, необходимый для создания списка обьектов, и удобства работы с ними.
    /// </summary>
    public class Settings
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}