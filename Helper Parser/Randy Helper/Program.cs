using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Randy_Helper.Helper;
using System.Windows.Forms;
using System.IO;

namespace Randy_Helper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string logfilepath = HelperClass.GetRootPath() + @"\GeneratedFile\logfile.aspx";
                if (File.Exists(logfilepath))
                {
                    File.Delete(logfilepath);
                }

                string CurrentDirectory = Environment.CurrentDirectory;
                Console.WriteLine("Browse Select Command Script File ");
                Console.WriteLine("Press any key to Browse File.");
                System.Console.ReadKey();
                OpenFileDialog SelectCommandFile = new OpenFileDialog();
                SelectCommandFile.ShowDialog();
                Console.WriteLine(SelectCommandFile.FileName);
                System.IO.StreamReader BrowseFile = new System.IO.StreamReader(SelectCommandFile.FileName);
                string SQLSelectCommand = BrowseFile.ReadToEnd();
                string MainSQLSelectCommand = SQLSelectCommand;
                BrowseFile.Close();
                Environment.CurrentDirectory = CurrentDirectory;
                SQLSelectCommand = SQLSelectCommand.Replace(System.Environment.NewLine, " ");
                SQLSelectCommand = SQLSelectCommand.Substring(SQLSelectCommand.ToLower().IndexOf("select") + 6).Trim();
                SQLSelectCommand = SQLSelectCommand.Substring(0, SQLSelectCommand.ToLower().LastIndexOf("from")).Trim();
                List<string> lstColumns = new List<string>();
                lstColumns = SQLSelectCommand.Split(',').ToList();
                WriteHeaderFile();
                WriterUpdateRecord();
                LoadCssANDGrid();
                WriteAllGridColumn(lstColumns);
                WriteFooterGridTemplateANDOthers();

                HelperClass.GenerateLog("<asp:SqlDataSource ID=\"SqlDataSource1\" runat=\"server\" ConnectionString=\"<%$ ConnectionStrings:pjengConnectionString1 %>\" SelectCommand=\"" + MainSQLSelectCommand + "\"></asp:SqlDataSource>");

                WriteLoadRestFIle();
            }
            catch (Exception ex)
            {
                HelperClass.GenerateLog(ex.Message);
            }

        }

        public static void WriteHeaderFile()
        {
            System.IO.StreamReader HeaderFile = new System.IO.StreamReader(HelperClass.GetRootPath() + @"\HelperFiles\LoadFileHeaders.txt");
            string HeaderFileText = HeaderFile.ReadToEnd();
            HeaderFile.Close();
            HelperClass.GenerateLog(HeaderFileText);
        }

        public static void WriterUpdateRecord()
        {
            string CurrentDirectory = Environment.CurrentDirectory;
            HelperClass.GenerateLog("void UpdateRecord(object sender, GridRecordEventArgs e)");
            HelperClass.GenerateLog("{");
            HelperClass.GenerateLog("try");
            HelperClass.GenerateLog("{");
            Console.WriteLine("Browse Update table Select Command Script File ");
            Console.WriteLine("Press any key to Browse File.");
            Console.ReadKey();
            OpenFileDialog UpdateCommandFile = new OpenFileDialog();
            UpdateCommandFile.ShowDialog();
            Console.WriteLine(UpdateCommandFile.FileName);
            System.IO.StreamReader BrowseFile = new System.IO.StreamReader(UpdateCommandFile.FileName);
            string SQLSelectCommand = BrowseFile.ReadToEnd();
            BrowseFile.Close();
            Environment.CurrentDirectory = CurrentDirectory;
            SQLSelectCommand = SQLSelectCommand.Replace(System.Environment.NewLine, " ");
            SQLSelectCommand = SQLSelectCommand.Substring(SQLSelectCommand.ToLower().IndexOf("select") + 6).Trim();
            string TableName = SQLSelectCommand.Substring(SQLSelectCommand.ToLower().LastIndexOf("from") + 4).Trim();
            SQLSelectCommand = SQLSelectCommand.Substring(0, SQLSelectCommand.ToLower().LastIndexOf("from")).Trim();


            List<string> columns = new List<string>();
            columns = SQLSelectCommand.Split(',').ToList();

            string UpdateQuery = " update " + TableName + "  SET  ";
            foreach (string col in columns)
            {
                string CurrCol = col.Replace('[', ' ');
                CurrCol = CurrCol.Replace(']', ' ').Trim();

                if (CurrCol.ToLower() != "icount")
                {
                    UpdateQuery += "[" + CurrCol + "]" + "=@" + CurrCol + ", ";

                }
            }
            UpdateQuery = UpdateQuery.Substring(0, UpdateQuery.Length - 2);
            HelperClass.GenerateLog(" string Sql = \"" + UpdateQuery + "\";");
            HelperClass.GenerateLog(" SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"pjengConnectionString1\"].ConnectionString);");
            HelperClass.GenerateLog("SqlCommand comm = new SqlCommand(Sql, conn);");
            HelperClass.GenerateLog(" comm.CommandType = CommandType.Text;");
            HelperClass.GenerateLog(" conn.Open();");


            foreach (string col in columns)
            {
                string CurrCol = col.Replace('[', ' ');
                CurrCol = CurrCol.Replace(']', ' ').Trim();
                HelperClass.GenerateLog("comm.Parameters.AddWithValue(\"@" + CurrCol + "\", e.Record[\"" + CurrCol + "\"]);");

            }

            HelperClass.GenerateLog(" comm.ExecuteNonQuery();");
            HelperClass.GenerateLog(" conn.Close();");
            HelperClass.GenerateLog("  }");
            HelperClass.GenerateLog(" catch");
            HelperClass.GenerateLog(" {");
            HelperClass.GenerateLog(" }");
            HelperClass.GenerateLog(" }");

        }


        public static void LoadCssANDGrid()
        {
            System.IO.StreamReader HeaderFile = new System.IO.StreamReader(HelperClass.GetRootPath() + @"\HelperFiles\LoadCssFileANDGrid.txt");
            string HeaderFileText = HeaderFile.ReadToEnd();
            HeaderFile.Close();
            HelperClass.GenerateLog(HeaderFileText);

        }

        public static void WriteAllGridColumn(List<string> lstColumn)
        {
            int Count = 1;
            foreach (string col in lstColumn)
            {
                string CurrCol = col.Replace('[', ' ');
                CurrCol = CurrCol.Replace(']', ' ').Trim();
                HelperClass.GenerateLog(" <cc1:Column ID=\"Column" + Count + "\" DataField=\"" + CurrCol + "\" HeaderText=\"" + CurrCol + "\"  runat=\"server\">");
                HelperClass.GenerateLog("   <%--    <TemplateSettings RowEditTemplateControlId=\"template_edit_id\" RowEditTemplateControlPropertyName=\"value\" /> --%> ");
                HelperClass.GenerateLog(" </cc1:Column>");
                Count++;
            }
        }

        public static void WriteFooterGridTemplateANDOthers()
        {
            System.IO.StreamReader HeaderFile = new System.IO.StreamReader(HelperClass.GetRootPath() + @"\HelperFiles\FooterGridTemplateANDOthers.txt");
            string HeaderFileText = HeaderFile.ReadToEnd();
            HeaderFile.Close();
            HelperClass.GenerateLog(HeaderFileText);

        }

        public static void WriteLoadRestFIle()
        {
            System.IO.StreamReader HeaderFile = new System.IO.StreamReader(HelperClass.GetRootPath() + @"\HelperFiles\LoadRestFIle.txt");
            string HeaderFileText = HeaderFile.ReadToEnd();
            HeaderFile.Close();
            HelperClass.GenerateLog(HeaderFileText);

        }
    }
}
