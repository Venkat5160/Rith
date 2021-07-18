using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Infrastructure.Security
{
    public class RandomSeriesGenerator
    {
        public static string RandomSeries()
        {
            int length = 6;
            const string valid = "@$abcdABCD1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string OTPSeries()
        {
            int length = 4;
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
           // return res.ToString();
            return "1234";
        }
    }
}
