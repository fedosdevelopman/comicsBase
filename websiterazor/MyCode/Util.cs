using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Data.SqlClient;
namespace websiterazor{
    public class Util{
        const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=comics;Integrated Security=True;"; 
        public string getDataById(string id, string typ, string db_name) {
            if (id != null && typ != null) {  //срабатывает, если параметры не пустые
                string queryString = "SELECT * FROM dbo."+db_name+" WHERE id=" + id + ";";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    string a;
                    if (reader.Read()){ a = reader[typ].ToString(); }
                    else{ a = "данные не найдены"; }  //сообщение о неудаче
                    reader.Close();
                    return a;
                }
            } else { return ""; }
        }
        public string DateToday(){
            return DateTime.Now.ToLongDateString();
        }
        public string getIdFromQuest(){
            return HttpContext.Current.Request.QueryString["id"];
        }
        public string autorizeee(string Name, string mail, string pass ){
            if(Name != null && mail != null && pass != null){
                if(Name != null){
                    bool mail_used = false;
                    string queryString = "SELECT * FROM dbo.users WHERE mail = '" + mail + "';";
                    string a;
                    using (SqlConnection connection = new SqlConnection(connectionString)){
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read()){
                            a = reader["mail"].ToString();
                        } else { a = ""; }
                        reader.Close();
                        if (a != "") { mail_used = true; }
                    }
                    if (!mail_used){
                        string RegQuery = "INSERT INTO dbo.users (name,mail,password,date) VALUES (N'" + Name + "','" + mail + "','" + pass + "','" + DateTime.Today.ToString("d") + "');";
                        using (SqlConnection connection = new SqlConnection(connectionString)){
                            SqlCommand command = new SqlCommand(RegQuery, connection);
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                        }
                        return $"{Name}~{mail}~{DateTime.Today.ToString("d")}~" ;
                    } else { return "почта "+a+" уже используется! "; }
                } else { return ""; }
            } else {
                if (Name == null && mail !=null && pass !=null){
                    string queryString = "SELECT * FROM dbo.users WHERE mail = '" + mail + "';", a;
                    using (SqlConnection connection = new SqlConnection(connectionString)){
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read()){ a = reader["mail"].ToString(); } else { a = ""; }
                        if( a == "") { return "Такой email не зарегистрирован"; } else{
                            a = reader["password"].ToString();
                            if(pass == a){
                                return reader["name"].ToString()+"~"+mail+"~"+reader["date"].ToString()+"~";
                            } else{
                                return "Неверный пароль";
                            }
                        }
                        reader.Close();
                    }
                } else { return ""; }
            } 

        }
        public string getAllComicsNames(){
            string queryString = "SELECT * FROM dbo.comicbooks;";
            string a, names = "";
            using (SqlConnection connection = new SqlConnection(connectionString)){
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read()){
                    a = reader["name"].ToString();
                    names += a + ";";
                }
                reader.Close();
                return names;
            }
        }
        public string getSearched(string s){
            string all_found_data = "";
            if (s != null) {
                string queryString = "SELECT * FROM dbo.comicbooks;";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()){
                        string a = reader["name"].ToString();
                        a = a.ToLower();
                        string d = "";
                        string[] data = s.Split(';');
                        bool lever = true;
                        for (int i = 0; i < data.Length; i++) {
                            if (a.Contains(data[i].ToLower())){
                                continue;
                            } else { lever = false; break; }
                        }
                        if (lever){all_found_data += reader["id"].ToString() + ";" + reader["name"].ToString() + ";" + reader["descript"].ToString() + ";" + reader["preview"].ToString() + ";";}
                    }
                    reader.Close();
                }
                return all_found_data;
            } else{
                return "";
            }
        }
        public string getAllDataType(string type){
            string queryString = "SELECT * FROM dbo.comicbooks;";
            string a, prews = "";
            using (SqlConnection connection = new SqlConnection(connectionString)){
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()){
                    a = reader[type].ToString();
                    prews += a + "~";
                }
                reader.Close();
                return prews;
            }
        }
    }
}