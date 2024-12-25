using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        public static TechImplementCRMEntities1 db = new TechImplementCRMEntities1();

        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    AspNetUser userReg = db.AspNetUsers.Where(a => a.Email == "akhtar@abc.com").FirstOrDefault();
                    Console.WriteLine("user");

                    DateTime datetime = DateTime.Now;
                    string date = datetime.Date.ToString();
                    //IQueryable<AspNetUser> userReg = db.AspNetUsers.Where(a => a.Email == "akhtar@abc.com");
                    DateTime userRegDate = Convert.ToDateTime(userReg.LockoutEndDateUtc);
                    int day = Convert.ToInt32(userRegDate.Minute);
                    int currentmin = Convert.ToInt32(datetime.Minute);
                    if (currentmin - day == 13)
                    {
                        db.AspNetUsers.Remove(userReg);
                        db.SaveChanges();
                        if (db.SaveChanges() > 0)
                        {
                            Console.WriteLine("COlamba");

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
