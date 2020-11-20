using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleService
{
    public class ExampleData 
    {

        public string ID;
        public string Name;
        public int Value;
        public List<int> Values;

        public Service.Formula Formula;

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





        public bool PushData(GameData row)
        {
            bool isEnable = System.Convert.ToBoolean(row.GetValue("isEnable"));
            if (isEnable)
            {
                //Var Data
                Service.String.To(row.GetValue("ID"), out ID);
                Service.String.To(row.GetValue("Name"), out Name);
                Service.String.To(row.GetValue("Value"), out Value);

                //Formula
                Service.String.To(row.GetValue("Formula"), out Formula , Service.String.formulaToType.json );

                //Enum
                Example = (ExampleType) Service.String.ToEnum(row.GetValue("Example"), ExampleType.A );

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