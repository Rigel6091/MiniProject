using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using Xceed.Wpf.Toolkit;

namespace MiniProject.Models
{
    public class ResisterUser
    {
        public List<SelectListItem> drdncity;
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Choose Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                           ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please enter CityId")]
        public int CityId { get; set; }

        public static SortedDictionary<string, List<ResisterUser>> marsViewdata()
        {
            SortedDictionary<string, List<ResisterUser>> list = new SortedDictionary<string, List<ResisterUser>>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;MultipleActiveResultSets=True";
            cn.Open();
            SqlCommand cmdcity = new SqlCommand();
            cmdcity.Connection = cn;
            cmdcity.CommandType = CommandType.Text;
            cmdcity.CommandText = "Select * from City";

            SqlCommand cmduser = new SqlCommand();
            cmduser.Connection = cn;
            cmduser.CommandType = CommandType.Text;

            SqlDataReader drcity = cmdcity.ExecuteReader();
            while (drcity.Read())
            {


                cmduser.CommandText = $"Select * from RegisterUser where CityId = " + drcity["CityId"];
                SqlDataReader druser = cmduser.ExecuteReader();
                string dc = drcity.GetString("CityName");
                List<ResisterUser> user1 = new List<ResisterUser>();
                while (druser.Read())
                {
                    user1.Add(new ResisterUser
                    {
                        FullName = druser.GetString("FullName"),
                        Gender = druser.GetString("Gender"),
                        EmailId = druser.GetString("EmailId"),
                        PhoneNo = druser.GetString("PhoneNo")

                    });
                }

                druser.Close();
                list.Add(dc, user1);
            }
            drcity.Close();
            cn.Close();
            return list;
        }
        public static ResisterUser UserByLoginname(string LoginName)
        {
            try
            {

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ReadUser";
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                SqlDataReader dr = cmd.ExecuteReader();

                ResisterUser user = new ResisterUser();
                dr.Read();
                user.LoginName = dr.GetString("LoginName");
                //user.Password = dr.GetString("Password");
                user.FullName = dr.GetString("FullName");
                user.Gender = dr.GetString("Gender");
                user.EmailId = dr.GetString("EmailID");
                user.PhoneNo = dr.GetString("PhoneNo");
                user.CityId = dr.GetInt32("CityId");

                dr.Close();
                cn.Close();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void registerUser(ResisterUser c)
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertUser";
                cmd.Parameters.AddWithValue("@CityId", c.CityId);
                cmd.Parameters.AddWithValue("@LoginName", c.LoginName);
                cmd.Parameters.AddWithValue("@FullName", c.FullName);
                cmd.Parameters.AddWithValue("@Password", c.Password);
                cmd.Parameters.AddWithValue("@Gender", c.Gender);
                cmd.Parameters.AddWithValue("@PhoneNo", c.PhoneNo);
                cmd.Parameters.AddWithValue("@EmailID", c.EmailId);

                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string isvalidlnlp(string LoginName, string Password)
        {
            string FullName = null;
            try
            {           
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();

                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = $"Select FullName from RegisterUser where LoginName='{LoginName}' and Password='{Password}'";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == false)
                {
                    throw new Exception("UserName and Password are Invalid");
                }
                dr.Read();
                FullName = dr.GetString("FullName");
                dr.Close();
                cn.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return FullName;
        }

        public static void UpdateUser(string LoginName, ResisterUser ruc)
        {
            try
            {
                SqlConnection sql = new SqlConnection();
                sql.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                sql.Open();

                SqlCommand cmd = sql.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateUser";
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                cmd.Parameters.AddWithValue("@FullName", ruc.FullName);
                cmd.Parameters.AddWithValue("@Gender", ruc.Gender);
                cmd.Parameters.AddWithValue("@EmailId", ruc.EmailId);
                cmd.Parameters.AddWithValue("@CityId", ruc.CityId);
                cmd.Parameters.AddWithValue("@PhoneNo", ruc.PhoneNo);
                cmd.ExecuteNonQuery();
                sql.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void ShowCityEntry(int CityId)
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from RegisterUser where CityId=@CityId";
                cmd.Parameters.AddWithValue("@CityId", CityId);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

