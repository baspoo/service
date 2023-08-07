using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace ExampleService
{
    public class ExampleData
    {


        public string ID;
        public string Name;
        public int Value;
        public List<int> Values;


        public string m_Data;


        public Formula Formula;

        public ExampleType Example;
        public enum ExampleType 
        { 
            A,B,C
        }

        public ClassExData ClassEx;
        public class ClassExData
        {
            public List<int> atk;
            public List<int> hp;
        }

        public List<TestData> TestDatas;
        public class TestData
        {
            public string Data;
            public int Number;
        }

        public int PriviteValue { get; private set; }



        public bool PushData(GameData row)
        {
            bool isEnable = System.Convert.ToBoolean(row.GetValue("isEnable"));
            if (isEnable)
            {

                //Style #0
                //Classic style manual get value.
                m_Data = row.GetValue("Data");
                ID      = row.GetValue("ID");
                Name    = row.GetValue("Name");
                Value   = row.GetValue("Value").ToInt();
  

                //Style #1
                //assign value.  "Data" == m_Data
                row.GetValue("Data", out m_Data);
                row.GetValue("ID", out ID);
                row.GetValue("Name", out Name);
                row.GetValue("Value", out Value);
                row.GetValue("Formula", out Formula);

                //Style #2
                //Variable names must match only. "ID" == ID
                row.GetValue(this, "ID");
                row.GetValue(this, "Name");
                row.GetValue(this, "Value");

                //Style #3
                //Variable names must match only. List{ "","" }
                row.GetValue(this, "ID", "Name", "Value");

                //Enum
                Example = row.GetValue("Example").ToEnum<ExampleType>();

                //Class
                ClassEx = ServiceJson.Json.DeserializeObject<ClassExData>(row.GetValue("ClassEx"));

                //List<int>
                Values = ServiceJson.Json.DeserializeObject<List<int>>(row.GetValue("Values"));

                //List<Class>
                TestDatas = ServiceJson.Json.DeserializeObject<List<TestData>>(row.GetValue("TestDatas"));

            }
            return isEnable;
        }












        #region FUNCTION CALL
        static List<ExampleData> m_datas = new List<ExampleData>();
        public static void InitDataTable(GameDataTable GameData)
        {
            m_datas.Clear();
            foreach (GameData row in GameData.GetTable())
            {
                ExampleData Content = new ExampleData();
                if (Content.PushData(row))
                    m_datas.Add(Content);
            }
        }
        public static List<ExampleData> ExampleDatas
        {
            get { return m_datas; }
        }

        public static ExampleData GetData(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return null;
            foreach (ExampleData data in m_datas)
            {
                if (data.ID == ID)
                    return data;
            }
            return null;
        }
        #endregion
    }

}