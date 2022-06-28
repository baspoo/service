using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Service;


#region Formula

[System.Serializable]
public class Formula : FormulaToolsService
{
	public int meta;
	public Formula()
	{

	}
	public Formula(string key, Formula f)
	{
		AddFormula(key, f);
	}
	public Formula(string key, object obj)
	{
		AddFormula(key, obj);
	}
	public Formula(object serializable)
	{
		if (serializable != null)
		{
			if (serializable is string)
			{
				var f = Json.JsonToJFormula((string)serializable);
				AddFormulas(f);
			}
			else
			{
				var f = Json.JsonToJFormula(serializable);
				AddFormulas(f);
			}

		}
	}
	[System.Serializable]
	public class FormulaData
	{

		public enum datatype
		{
			none, num, nums, str, bol, vector, transform, enums, formula, obj, json, list, formulaData
		}
		public FormulaData(string name)
		{
			this.Name = name;
		}
		public FormulaData(string name, bool uni = false)
		{
			this.Name = name;
			if (uni)
				m_uniID = String.RandomUniKey(0, true);
		}
		public FormulaData(string name, string uni)
		{
			this.Name = name;
			if (!string.IsNullOrEmpty(uni))
				m_uniID = uni;
		}
		public FormulaData(string name, object obj)
		{
			this.Name = name;
			if (obj != null)
			{
				isHave = true;
				var define = new Service.Tools.ObjectDefine(obj);
				var type = Service.Tools.ObjectDefine.DefineType(obj);
				switch (type)
				{
					case datatype.str: Text = define.String; break;
					case datatype.num: Value = Service.Tools.ObjectDefine.NumericToDouble(obj); break;
					case datatype.bol: Status = define.Bool; break;
					case datatype.formula: SubFormula = (Formula)obj; break;
					case datatype.obj: Object = obj; break;
				}
			}
		}



		public string uniID { get { return m_uniID; } }
		private string m_uniID;

		public string FormulaName { get { return Name; } }
		public void Rename(string newName)
		{
			Name = newName;
		}

		[SerializeField]
		private string Name;
		[SerializeField]
		string m_Text;
		[SerializeField]
		double m_Value;
		[SerializeField]
		double[] m_Range;
		[SerializeField]
		bool m_Status;
		[SerializeField]
		[Newtonsoft.Json.JsonIgnore]
		object m_object;
		[SerializeField]
		[Newtonsoft.Json.JsonIgnore]
		Formula m_formula;
		[SerializeField]
		[Newtonsoft.Json.JsonIgnore]
		Formula m_litsformula;
		string m_tag;
		public List<argsData> args = new List<argsData>();
		public class argsData
		{
			public string argName = string.Empty;
			public string argData = null;
		}
		public string Text
		{
			get { return m_Text; }
			set { m_datatype = datatype.str; m_Text = value; }
		}
		public string Json
		{
			get { return m_Text; }
			set { m_datatype = datatype.json; m_Text = value; }
		}
		public double Value
		{
			get { return m_Value; }
			set { m_datatype = datatype.num; m_Value = value; }
		}
		public int ValueInt
		{
			get { return (int)m_Value; }
		}
		public double[] Range
		{
			get
			{
				if (m_Range == null || m_Range.Length == 0)
				{
					m_Range = String.PassStringToRange(m_Text);
				}
				return m_Range;
			}
			set { m_datatype = datatype.nums; m_Range = value; }
		}
		public Formula SubFormula
		{
			get { if (m_formula == null) m_formula = new Formula(); return m_formula; }
			set { m_datatype = datatype.formula; m_formula = value; }
		}
		public Formula ListFormulaDatas
		{
			get { return m_litsformula; }
			set { m_datatype = datatype.list; m_litsformula = value; }
		}
		public object Object
		{
			get { return m_object; }
			set { m_datatype = datatype.obj; m_object = value; }
		}
		public bool Status
		{
			get { return m_Status; }
			set { m_datatype = datatype.bol; m_Status = value; }
		}
		public object GetDataByType()
		{
			if (m_datatype == datatype.str) return m_Text;
			if (m_datatype == datatype.num) return m_Value;
			if (m_datatype == datatype.bol) return m_Status;
			if (m_datatype == datatype.formula) return SubFormula;
			if (m_datatype == datatype.obj) return m_object;
			if (m_datatype == datatype.list) return ListFormulaDatas;
			return null;
		}
		public System.Enum Enum(object Default)
		{
			return String.ToEnum(Text, Default);
		}
		public double Relative(double increase)
		{
			if (GetDataType == datatype.num)
				m_Value += increase;
			return m_Value;
		}

		[HideInInspector]
		public bool isHave = false;
		public bool Equals(string findname, bool isSensitive = true)
		{
			if (isSensitive)
				return (Name == findname);
			else
				return (Name.ToLower() == findname.ToLower());
		}
		[Newtonsoft.Json.JsonIgnore]
		datatype m_datatype = datatype.none;
		public datatype GetDataType
		{
			get { return m_datatype; }
		}
		public FormulaData SetTag(string tagName)
		{
			m_tag = tagName;
			return this;
		}
		public FormulaData SetUniID(string uniID)
		{
			m_uniID = uniID;
			return this;
		}
		public string GetTag()
		{
			return m_tag;
		}

		public argsData AddArg(string argName, object argData)
		{
			return AddArg(argName, (argData == null) ? string.Empty : argData.ToString());
		}
		public argsData AddArg(string argName, int argData)
		{
			return AddArg(argName, argData.ToString());
		}
		public argsData AddArg(string argName, float argData)
		{
			return AddArg(argName, argData.ToString());
		}
		public argsData AddArg(string argName, bool argData)
		{
			return AddArg(argName, argData.ToString());
		}
		public argsData AddArg(string argName, string argData)
		{
			argsData a = new argsData();
			a.argName = argName;
			a.argData = argData;
			args.Add(a);
			return a;
		}
		public Service.String.StringDefine GetArg(string argName, bool isSensitive = true)
		{
			foreach (argsData arg in args)
			{
				if ((isSensitive) ? (arg.argName == argName) : (arg.argName.ToLower() == argName.ToLower()))
					return Service.String.StringDefine.ToStringDefine(arg.argData);
			}
			return Service.String.StringDefine.ToStringDefine(string.Empty);
		}
		public bool UpdateArg(string argName, string value)
		{
			foreach (argsData arg in args)
				if (arg.argName == argName)
				{
					arg.argData = value;
					return true;
				}
			return false;
		}
		public void AddOrUpdateArg(string argName, object value)
		{
			foreach (argsData arg in args)
				if (arg.argName == argName)
				{
					arg.argData = value.ToString();
					return;
				}
			AddArg(argName, value.ToString());
		}
		public void AddArgs(List<argsData> argsDatas)
		{
			args.AddRange(argsDatas);
		}
		public void RemoveArg(string argName)
		{
			foreach (argsData arg in args)
				if (arg.argName == argName)
				{
					args.Remove(arg);
					return;
				}
		}
		public bool isHaveArg(string argName)
		{
			foreach (argsData arg in args)
				if (arg.argName == argName)
					return true;
			return false;
		}
		public void UpdateData(object value)
		{
			if (m_datatype == datatype.str) m_Text = new Service.Tools.ObjectDefine(value).String;
			if (m_datatype == datatype.num) m_Value = new Service.Tools.ObjectDefine(value).Double;
			if (m_datatype == datatype.bol) m_Status = new Service.Tools.ObjectDefine(value).Bool;
			if (m_datatype == datatype.obj) m_object = value;
		}
		public void UpdateData(Formula f)
		{
			if (m_datatype == datatype.formula) SubFormula = f;
		}
		public void AddList(object value)
		{
			ListFormulaDatas.AddFormula(null, value);
		}
	}


	public FormulaData this[int index] { get { return GetFormulaDatas[index]; } }
	public FormulaData this[string key] { get { return GetFormula(key); } }
	public FormulaData this[string key, string tag] { get { return GetFormula(key, tag); } }

	public FormulaData this[string key, object value] { get { return AddOrUpdateFormula(key, value); } }


	private string m_Nickname = string.Empty;
	public string Nickname
	{
		get
		{
			return m_Nickname;
		}
		set
		{
			m_Nickname = value;
		}
	}

	//[Newtonsoft.Json.JsonIgnore]
	//public FormulaToolsService.LocalPackageContent LocalContent = new LocalPackageContent();

	[SerializeField]
	[Newtonsoft.Json.JsonIgnore]
	List<FormulaData> FormulaDatas = new List<FormulaData>();
	public List<FormulaData> GetFormulaDatas
	{
		get
		{
			return FormulaDatas;
		}
	}
	public void Clear()
	{
		FormulaDatas.Clear();
	}
	public FormulaData AddFormula(string f_name, string text)
	{
		FormulaData fData = new FormulaData(f_name);
		fData.Text = text;
		fData.isHave = true;
		FormulaDatas.Add(fData);
		return fData;
	}
	public FormulaData AddFormula(string f_name, double value)
	{
		FormulaData fData = new FormulaData(f_name);
		fData.Value = value;
		fData.isHave = true;
		FormulaDatas.Add(fData);
		return fData;
	}
	public FormulaData AddFormula(string f_name, bool status)
	{
		FormulaData fData = new FormulaData(f_name);
		fData.Status = status;
		fData.isHave = true;
		FormulaDatas.Add(fData);
		return fData;
	}
	public FormulaData AddFormula(string f_name, Formula f)
	{
		FormulaData fData = new FormulaData(f_name);
		fData.SubFormula = f;
		fData.isHave = true;
		FormulaDatas.Add(fData);
		return fData;
	}
	public FormulaData AddFormula(FormulaData formulaData)
	{
		if (formulaData != null)
		{
			formulaData.isHave = true;
			FormulaDatas.Add(formulaData);
		}
		return formulaData;
	}
	protected FormulaData AddFormula(string f_name, object obj)
	{
		FormulaData fData = new FormulaData(f_name, obj);
		if (fData.isHave) FormulaDatas.Add(fData);
		return fData;
	}
	public FormulaData AddFormula(string f_name, object obj, FormulaData.datatype type)
	{
		FormulaData fData = new FormulaData(f_name);
		if (obj != null)
		{
			switch (type)
			{
				case FormulaData.datatype.str: fData.Text = (string)obj; break;
				case FormulaData.datatype.json: fData.Json = (string)obj; break;
				case FormulaData.datatype.num: fData.Value = Tools.ObjectDefine.NumericToDouble(obj); break;
				case FormulaData.datatype.bol: fData.Status = (bool)obj; break;
				case FormulaData.datatype.obj: fData.Object = obj; break;
			}
			fData.isHave = true;
			FormulaDatas.Add(fData);
		}
		return fData;
	}
	public void AddFormulas(Formula formulaData)
	{
		if (formulaData == null)
			return;
		AddFormulas(formulaData.GetFormulaDatas);
	}
	public void AddFormulas(List<Formula.FormulaData> formulaDatas)
	{
		if (formulaDatas == null)
			return;
		foreach (Formula.FormulaData data in formulaDatas)
		{
			if (data.isHave)
			{
				FormulaDatas.Add(data);
			}
		}
	}
	public void AddAndUpdateFormulas(Formula formulaData)
	{
		if (formulaData == null)
			return;
		foreach (Formula.FormulaData data in formulaData.GetFormulaDatas)
		{
			if (data.isHave)
			{
				Formula.FormulaData FD = GetFormula(data.FormulaName);
				if (FD.isHave)
				{
					FD.UpdateData(data.GetDataByType());
				}
				else
					FormulaDatas.Add(data);
			}
		}
	}
	public FormulaData AddOrUpdateFormula(FormulaData formulaData)
	{
		for (int i = 0; i < FormulaDatas.Count; i++)
		{
			if (FormulaDatas[i].FormulaName == formulaData.FormulaName)
			{
				FormulaDatas[i] = formulaData;
				return formulaData;
			}
		}
		AddFormula(formulaData);
		return formulaData;
	}
	public FormulaData AddOrUpdateFormula(string f_name, object obj)
	{
		FormulaData fData = GetFormula(f_name);
		if (GameObj.isObjectNotNull(obj))
		{
			if (fData != null)
			{
				if (fData.isHave)
				{
					if (fData.GetDataType == FormulaData.datatype.num) fData.Value = Tools.ObjectDefine.NumericToDouble(obj);
					if (fData.GetDataType == FormulaData.datatype.bol) fData.Status = (bool)obj;
					if (fData.GetDataType == FormulaData.datatype.str) fData.Text = (string)obj;
					if (fData.GetDataType == FormulaData.datatype.formula) fData.SubFormula = (Formula)obj;
					if (fData.GetDataType == FormulaData.datatype.obj) fData.Object = obj;
				}
				else fData = AddFormula(f_name, obj);
			}
			else fData = AddFormula(f_name, obj);
		}
		return fData;
	}
	public FormulaData AddOrUpdateFormula(string f_name, Formula obj)
	{
		FormulaData fData = GetFormula(f_name);
		if (GameObj.isObjectNotNull(obj))
		{
			if (fData != null)
			{
				if (fData.isHave)
				{
					fData.SubFormula = obj;
				}
				else fData = AddFormula(f_name, obj);
			}
			else fData = AddFormula(f_name, obj);
		}
		return fData;
	}
	public double Relative(string f_name, double increase)
	{
		FormulaData fData = GetFormula(f_name);
		if (fData.isHave) fData.Relative(increase);
		else AddFormula(f_name, increase);
		return fData.Value;
	}


	public FormulaData AddList(string f_name)
	{
		FormulaData fData = new FormulaData(f_name);
		fData.ListFormulaDatas = new Formula();
		fData.isHave = true;
		FormulaDatas.Add(fData);
		return fData;
	}
	public void DesAllFormula(string f_name)
	{
		FormulaDatas.RemoveAll(x => x.FormulaName == f_name);
	}
	public void DesFormula(string f_name)
	{
		FormulaData f = GetFormula(f_name);
		if (f.isHave)
		{
			FormulaDatas.Remove(f);
		}
	}
	public void DesFormula(FormulaData f)
	{
		if (f.isHave)
		{
			FormulaDatas.Remove(f);
		}
	}
	public void DesFormulas(Formula f_remove)
	{
		foreach (FormulaData fd in f_remove.GetFormulaDatas)
		{
			DesFormula(fd.FormulaName);
		}
	}
	public Formula DesFormulasAndCopy(Formula f_remove)
	{
		Formula copy = new Formula();
		foreach (FormulaData fd in FormulaDatas)
		{
			if (!f_remove.GetFormula(fd.FormulaName).isHave)
				copy.AddFormula(fd);
		}
		return copy;
	}
	public List<FormulaData> GetTag(string TagName)
	{
		List<FormulaData> tag = new List<FormulaData>();
		foreach (FormulaData formula in FormulaDatas)
		{
			if (formula.GetTag() == TagName)
				tag.Add(formula);
		}
		return tag;
	}
	public Formula SplitTag(string TagName)
	{
		Formula f = new Formula();
		foreach (FormulaData fd in GetTag(TagName))
		{
			f.AddFormula(fd);
		}
		return f;
	}
	public List<FormulaData> GetAllisHaveArg(string ArgName)
	{
		List<FormulaData> args = new List<FormulaData>();
		foreach (FormulaData formula in FormulaDatas)
		{
			if (formula.isHaveArg(ArgName))
				args.Add(formula);
		}
		return args;
	}
	public FormulaData GetFormula(string FormulaName, bool isSensitive = true)
	{
		foreach (FormulaData formula in FormulaDatas)
		{
			if (formula.Equals(FormulaName, isSensitive))
			{
				return formula;
			}
		}
		return new FormulaData(string.Empty);
	}
	public FormulaData GetFormula(string FormulaName, string tag)
	{
		foreach (FormulaData formula in FormulaDatas)
		{
			if ((formula.Equals(FormulaName)) && (formula.GetTag() == tag))
			{
				return formula;
			}
		}
		return new FormulaData(string.Empty);
	}
	public List<FormulaData> GetFormulas(string FormulaName)
	{
		List<FormulaData> fDatas = new List<FormulaData>();
		foreach (FormulaData formula in FormulaDatas)
		{
			if (formula.Equals(FormulaName))
			{
				fDatas.Add(formula);
			}
		}
		return fDatas;
	}
	public List<FormulaData> GetFormulas(string FormulaName, string tag)
	{
		List<FormulaData> fDatas = new List<FormulaData>();
		foreach (FormulaData formula in FormulaDatas)
		{
			if (formula.Equals(FormulaName) && (formula.GetTag() == tag))
			{
				fDatas.Add(formula);
			}
		}
		return fDatas;
	}
	public double RelativeSubPath(string path, double increase)
	{
		FormulaData fData = GetSubPath(path);
		if (fData.isHave) fData.Relative(increase);
		else AddSubPath(path, increase);
		return fData.Value;
	}
	public FormulaData AddSubPath(string path, object value)
	{
		return Json.AddSubPath(this, path, value);
	}
	public FormulaData GetSubPath(string path)
	{
		return Json.GetSubPath(this, path);
	}


	public void Shuffle()
	{
		for (int i = 0; i < FormulaDatas.Count; i++)
		{
			FormulaData temp = FormulaDatas[i];
			int randomIndex = Random.Range(i, FormulaDatas.Count);
			FormulaDatas[i] = FormulaDatas[randomIndex];
			FormulaDatas[randomIndex] = temp;
		}
	}

	public Formula Clone()
	{
		string json = FormulaToolsService.Json.FormulaToJson(this);
		return FormulaToolsService.Json.JsonToJFormula(json);
	}

	/// text:'hello world',index:5,enable:false
	/// 
	///		[output]
	///     formuladata.name = text
	/// 	formuladata.text = "hello world";
	/// 
	/// 	formuladata.name = index
	/// 	formuladata.value = 5;
	/// 
	/// 	formuladata.name = enable
	/// 	formuladata.bool = false;

	public static Formula TextSetup(string Datas)
	{
		if (string.IsNullOrEmpty(Datas))
			return new Formula();
		try
		{
			Formula formula = new Formula();
			string NewJson = Datas;
			List<string> values = String.strCropValues(NewJson, "'", "'");
			int index = 0;
			foreach (string dataStr in values)
			{
				string newValue = dataStr.Replace(",", "<&comma>");
				NewJson = NewJson.Replace(dataStr, newValue);
			}
			string[] splitData = NewJson.Split(',');
			foreach (string KeyValue in splitData)
			{
				if (!string.IsNullOrEmpty(KeyValue))
				{
					Formula.FormulaData FData = ToFormulaData(KeyValue.Replace("<&comma>", ","));
					if (FData != null)
						formula.AddFormula(FData);
				}
			}
			return formula;
		}
		catch (System.Exception e)
		{
			Debug.LogError("Formula Can't Setup : {" + Datas + "}" + "\n\n\n\n" + e.StackTrace);
		}
		return new Formula();
	}


	public static string single = "<#key-single#>";
	static Formula.FormulaData ToFormulaData(string KeyValue)
	{
		//baspoo:50
		//test:'ccc'

		//**Remove Text 'message' to the Data FirstStep ==> You Can Enter All Char || String to the Formula Ok..!
		string removeText = KeyValue;
		string text = string.Empty;
		if (removeText.IndexOf("'") != -1)
		{
			text = String.strCropValue(removeText, "'", "'");
			if (!string.IsNullOrEmpty(text))
				removeText = removeText.Replace("'" + text + "'", "''");
			else
				text = string.Empty;
		}
		//**-----------------------------------------------------------------------------------------------------
		string args = string.Empty;
		if (removeText.IndexOf("<arg>") != -1)
		{
			args = String.strCropValue(removeText, "<arg>", "</arg>");
			if (!string.IsNullOrEmpty(args))
				removeText = removeText.Replace("<arg>" + args + "</arg>", "");
		}
		//**-----------------------------------------------------------------------------------------------------
		string uniID = string.Empty;
		if (removeText.IndexOf("<uniID>") != -1)
		{
			uniID = String.strCropValue(removeText, "<uniID>", "</uniID>");
			if (!string.IsNullOrEmpty(uniID))
				removeText = removeText.Replace("<uniID>" + uniID + "</uniID>", "");
		}
		//**-----------------------------------------------------------------------------------------------------
		string range = string.Empty;
		if (removeText.IndexOf("[") != -1)
			if (removeText.IndexOf("]") != -1)
			{
				range = String.strCropValue(removeText, "[", "]");
				if (!string.IsNullOrEmpty(range))
					removeText = removeText.Replace(range, "");
			}
		//**-----------------------------------------------------------------------------------------------------



		//Debug.Log (KeyValue);
		string[] splitKeyValue = removeText.Split(':');
		if (splitKeyValue.Length >= 2)
			if (!string.IsNullOrEmpty(splitKeyValue[0]))
				if (!string.IsNullOrEmpty(splitKeyValue[1]))
				{

					//CleanKey
					string key = splitKeyValue[0];
					key = String.RemoveSpecialCharater(key);
					FormulaData fData = new FormulaData(key);

					//** String
					if (!string.IsNullOrEmpty(text))
					{
						text = text.Replace(single, "'");
						fData.Text = text; //function_string.strCropValue (splitKeyValue [1], '\'', '\'');
						fData.isHave = true;
					}
					else
					{
						//** Bool
						if ((splitKeyValue[1].ToLower().IndexOf("true") != -1) || (splitKeyValue[1].ToLower().IndexOf("false") != -1))
						{
							bool valuebool;
							bool isBool = bool.TryParse(splitKeyValue[1], out valuebool);
							if (isBool)
							{
								fData.Status = valuebool;
								fData.isHave = true;
							}
						}
						//** Double
						else
						{
							//**Doubles / Rang [0/1] / [2/3/4/5/8/9.....]
							if (!string.IsNullOrEmpty(range))
							{

								/*
								string[] r = range.Split ('/');
								fData.Range = new double[r.Length];
								for (int i = 0 ; i < r.Length ; i++) {
									fData.Range[i] = System.Convert.ToDouble (r[i]);
								}
								fData.isHave = true;
								*/

								fData.Range = String.PassStringToRange(range);
								fData.isHave = true;

							}
							//**Double
							else
							{
								double valueDouble = 0;
								bool isDouble = double.TryParse(splitKeyValue[1], out valueDouble);
								if (isDouble)
								{
									fData.Value = valueDouble;
									fData.isHave = true;
								}
							}
						}
					}
					//** Tag
					if (splitKeyValue.Length >= 3)
					{
						if (!string.IsNullOrEmpty(splitKeyValue[2]))
							fData.SetTag(splitKeyValue[2]);
					}
					//**Agus
					if (!string.IsNullOrEmpty(args))
					{
						string[] args_split = args.Split('&');
						foreach (string arg in args_split)
						{
							string argName = String.strCropValue(arg, "</args_", ">");
							string begin = "<args_" + argName + ">";
							string end = "</args_" + argName + ">";
							if (args.IndexOf(begin) != -1)
							{
								string argData = String.strCropValue(args, begin, end);
								fData.AddArg(argName, argData);
							}
						}
					}
					//**uniID
					if (!string.IsNullOrEmpty(uniID))
					{
						fData.SetUniID(uniID);
					}


					return fData;
				}
		return null;
	}
	public string PassToString()
	{
		string str = "";
		foreach (FormulaData formula in FormulaDatas)
		{
			string data = "";
			//** Main
			if (formula.GetDataType == FormulaData.datatype.str) data = formula.FormulaName + ":'" + formula.Text.Replace("'", single) + "'";
			if (formula.GetDataType == FormulaData.datatype.num) data = formula.FormulaName + ":" + formula.Value.ToString();
			if (formula.GetDataType == FormulaData.datatype.bol) data = formula.FormulaName + ":" + formula.Status.ToString();
			if (formula.GetDataType == FormulaData.datatype.nums)
			{
				/*
				string sum = "[";
				for (int i = 0; i < formula.Range.Length; i++) {
					if (i != 0)
						sum += "/";
					sum += formula.Range [i].ToString ();
				}
				sum += "]";
				*/
				string sum = String.PassRangeToString(formula.Range);
				data = formula.FormulaName + ":" + sum;
			}



			//** Tag
			if (!string.IsNullOrEmpty(formula.GetTag())) data += ":" + formula.GetTag();

			//** Args
			string args = string.Empty;
			if (formula.args.Count != 0)
			{
				foreach (FormulaData.argsData arg in formula.args)
				{
					if (arg.argData != null)
					{
						if (args != string.Empty) args += "&";
						args += "<args_" + arg.argName + ">" + arg.argData + "</args_" + arg.argName + ">";
					}
				}
			}
			if (args != string.Empty)
			{
				data += "<arg>" + args + "</arg>";
			}


			//** uniID
			if (!string.IsNullOrEmpty(formula.uniID))
				data += "<uniID>" + formula.uniID + "</uniID>";



			//**Add data to datas[all].
			if (!string.IsNullOrEmpty(data))
			{
				str += data + ",";
			}
		}
		return str;
	}
	public void PassToClass(object obj)
	{
		foreach (Formula.FormulaData fd in GetFormulaDatas)
		{
			Var.ToClass(obj, fd.FormulaName, fd.GetDataByType());
		}
	}
	public string PassToJson(Formula meta = null)
	{
		return Json.FormulaToJson(this, meta);
	}
	public object PassToJsonObject(Formula meta = null)
	{
		return Json.FormulaToJson(this, meta).DeserializeObject<object>();
	}
	public void SaveToLocal(string FormulaThisName = null)
	{
		if (FormulaThisName == null) FormulaThisName = Nickname;
		PlayerPrefs.SetString($"{FormulaThisName}@formula.localsave", PassToJson());
	}
	public static Formula GetByLocal(string FormulaThisName)
	{
		var json = PlayerPrefs.GetString($"{FormulaThisName}@formula.localsave");
		var f = JsonToFormula(json);
		f.Nickname = FormulaThisName;
		return f;
	}


	public static Formula JsonToFormula(string raw_json) => FormulaToolsService.Json.JsonToJFormula(raw_json);
	public static Formula JsonToFormula(object json) => FormulaToolsService.Json.JsonToJFormula(json);

	public static List<Formula> JsonToFormulas(string raw_json) => FormulaToolsService.Json.JsonArrayToJFormulas(raw_json);
	public static List<Formula> JsonToFormulas(object json) => FormulaToolsService.Json.JsonArrayToJFormulas(json);
}





#endregion


